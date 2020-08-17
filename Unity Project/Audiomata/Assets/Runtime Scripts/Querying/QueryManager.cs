using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


namespace Audiomata
{
    /// <summary>
    ///  Given an Array of AudioData, will automatically manage querying to them in a similar manner
    ///  to a database using System.Linq
    /// </summary>
    public class QueryManager
    {
        private Dictionary<string, List<string>> tagsToGuidsDict;
        private Dictionary<string, AudioClip> guidToClipDict;
        private Dictionary<char, Op> queryOpDict;
        private Regex opRegex;
        private const char noOpChar = '\0';

        private readonly List<string> allGuids;
        /// <summary>
        /// Creates an instance of a class that can query tags with in buil or custom operations
        /// </summary>
        /// <param name="audioData">an array of all audio data that can be queried</param>
        /// <param name="customOperators">A dictionary of custom Operators</param>
        /// <param name="overwriteDictionary">Whether in built operations should be overrided </param>
        /// <remarks>If replace operations is true, the dictionary will be completely replaced set false for merge</remarks>

        public QueryManager(AudioData[] audioData, Dictionary<char, Op> customOperators = null, bool overwriteDictionary = false)
        {
            tagsToGuidsDict = new Dictionary<string, List<string>>();
            guidToClipDict = new Dictionary<string, AudioClip>();
            queryOpDict = new Dictionary<char, Op>();
            //populate audio data
            for (int i = 0; i < audioData.Length; i++)
            {
                AudioData nextAD = audioData[i];
                guidToClipDict.Add(nextAD.guid, nextAD.clip);

                List<string> tagList = nextAD.tags;

                for (int j = 0; j < tagList.Count; j++)
                {
                    string nextTag = tagList[j];
                    List<string> guidList;

                    if (tagsToGuidsDict.TryGetValue(nextTag, out guidList))
                    {
                        guidList.Add(nextAD.guid);
                    }
                    else
                    {
                        guidList = new List<string>();
                        guidList.Add(nextAD.guid);
                        tagsToGuidsDict.Add(nextTag, guidList);
                    }
                }

            }
            allGuids = new List<string>(guidToClipDict.Keys);

            if (customOperators != null)
            {
                if (overwriteDictionary)
                {
                    queryOpDict = customOperators;
                }
                else
                {
                    BuildDefaultOPs();
                    Debug.Log("Created ops");
                    foreach (KeyValuePair<char, Op> opKvp in customOperators)
                    {
                        queryOpDict.Add(opKvp.Key, opKvp.Value);
                    }
                }
            }
            else
            {
                BuildDefaultOPs();
            }
            BuildRegex();
        }

        private void BuildRegex()
        {
            string opsRegex = '[' + new string(queryOpDict.Keys.ToArray()) + ']';
            opRegex = new Regex(opsRegex);
        }

        private void BuildDefaultOPs()
        {
            Op and, or, not, rand, xor, brackets;//,contains;

            and = new Op()
            {
                opDelegate = AndLstSet,
                opSymbol = '&',
                opType = OpType.Postfix
            };

            or = new Op()
            {
                opDelegate = OrLstSet,
                opSymbol = '|',
                opType = OpType.Postfix
            };

            not = new Op()
            {
                opDelegate = NotLstSet,
                opSymbol = '!',
                opType = OpType.Prefix
            };

            xor = new Op
            {
                opDelegate = XorLstSet,
                opSymbol = '^',
                opType = OpType.Postfix
            };

            rand = new Op()
            {
                opDelegate = SingleRandSelection,
                opSymbol = '?',
                opType = OpType.Prefix
            };

            brackets = new Op()
            {
                opDelegate = Bracket,
                opSymbol = '(',
                opEnder = ')',
                opType = OpType.Group
            };

            queryOpDict = new Dictionary<char, Op>()
                    {
                        {and.opSymbol, and},
                        {or.opSymbol,or },
                        {not.opSymbol,not },
                        {xor.opSymbol,xor },
                        { rand.opSymbol,rand},
                        { brackets.opSymbol, brackets},
                        { brackets.opEnder,brackets}
                    };
        }

        public AudioClip GetClipByTag(string tag)
        {
            List<string> guids;

            if (tagsToGuidsDict.TryGetValue(tag, out guids))
            {
                string guidChoice = guids[UnityEngine.Random.Range(0, guids.Count)];
                return guidToClipDict[guidChoice];
            }

            return null;
        }

        public AudioClip GetTrackById(string id) => guidToClipDict[id];

        public Op GetOp(char c) => queryOpDict[c];

        /// <summary>
        /// Queries all the tags and returns a random audioclip from the filtered set
        /// </summary>
        /// <param name="query">string to query, follows c# syntax e.g. "example|!Unity&Audiomata", all ops are single chars </param>
        /// <remarks>Does not use safety beyond checking tags exist</remarks>
        /// <returns>An AudioClip that matches the query criteria </returns>
        public AudioClip QueryAudio(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                Debug.LogError("Query was empty");
                return null;
            }

            IEnumerable<string> currentGuidSet = null;
            MatchCollection opMatches = opRegex.Matches(query);

            if (opMatches.Count > 0)
            {
                //predicate allows chained prefixes where usable e.g.(although not techincally workable) !!tag
                Queue<string> tagsInQuery = new Queue<string>(opRegex.Split(query).Where(tag => !string.IsNullOrEmpty(tag)));
                currentGuidSet = PerformOps(opMatches, tagsInQuery, query);
            }
            else //assumes there is just one tag if there are no ops 
            {
                Debug.LogWarning("\"" + query + "\" may have invalid operators as none were detectedm if just one tag was required, use GetClipByTag");
                return GetClipByTag(query);
            }

            if (currentGuidSet == null)
            {
                return null;
            }
            if (currentGuidSet.Count() < 1)
            {
                return null;
            }

            List<string> queryResultLst = currentGuidSet.ToList();
            Debug.Log(queryResultLst.Count);
            if (queryResultLst.Count < 1)
            {
                return null;
            }
            string selectedGuid = queryResultLst[Random.Range(0, queryResultLst.Count)];
            return guidToClipDict[selectedGuid];
        }

        public void QueryAudio(string query, out AudioClip[] resultTarget)
        {
            if (string.IsNullOrEmpty(query))
            {
                Debug.LogError("Query was empty");
                resultTarget = null;
                return;
            }
            Debug.Log("Started Query");

            IEnumerable<string> currentGuidSet = null;
            MatchCollection opMatches = opRegex.Matches(query);

            if (opMatches.Count > 0)
            {
                //predicate allows chained prefixes where usable e.g.(although not techincally workable) !!tag
                Queue<string> tagsInQuery = new Queue<string>(opRegex.Split(query).Where(tag => !string.IsNullOrEmpty(tag)));
                currentGuidSet = PerformOps(opMatches, tagsInQuery, query);
            }
            else //assumes there is just one tag if there are no ops 
            {
                Debug.LogWarning("\"" + query + "\" may have invalid operators as none were detected if just one tag was required, use GetClipByTag");
                List<string> guids;
                if (tagsToGuidsDict.TryGetValue(query, out guids))
                {
                    resultTarget = new AudioClip[guids.Count];

                    for (int i = 0; i < guids.Count; i++)
                    {
                        resultTarget[i] = guidToClipDict[guids[i]];
                    }
                }
                else
                {
                    resultTarget = null;
                    return;
                }

            }

            if (currentGuidSet == null)
            {
                resultTarget = null;
                return;
            }
            if (currentGuidSet.Count() < 1)
            {
                resultTarget = null;
                return;
            }

            List<string> searchResults = currentGuidSet.ToList();
            resultTarget = new AudioClip[searchResults.Count];
            for (int i = 0; i < searchResults.Count; i++)
            {
                resultTarget[i] = guidToClipDict[searchResults[i]];
            }

        }

        private IEnumerable<string> PerformOps(MatchCollection matches, Queue<string> tagQ, string query)
        {
            string currentTag = tagQ.Dequeue();
            IEnumerable<string> currentResults = tagsToGuidsDict[currentTag];

            for (int i = 0; i < matches.Count; i++)
            {
                Match nextOpMatch = matches[i];
                Op nextOP = queryOpDict[nextOpMatch.Value[0]];

                switch (nextOP.opType)
                {
                    case OpType.Prefix:
                        currentResults = ActionPrefixOP(currentResults, ref currentTag, tagQ, nextOP, matches, ref i);
                        break;
                    case OpType.Postfix:
                        currentResults = ActionPostFixOp(currentResults, ref currentTag, tagQ, matches, nextOpMatch, ref i);
                        break;
                    case OpType.Group:
                        currentResults = ProcessGroup(currentResults, ref currentTag, tagQ, nextOP, matches, ref i);
                        break;
                    default:
                        throw new System.NotImplementedException("Unknown op type:" + nextOP.opType.ToString());
                }
            }

            return currentResults;
        }

        private IEnumerable<string> ActionPostFixOp(IEnumerable<string> currentResults, ref string currentTag, Queue<string> tagQ, MatchCollection regexOpMatches, Match nextOPRegex, ref int matchIdx)
        {
            string rightTag = tagQ.Dequeue();
            IEnumerable<string> rhsTaggedList = tagsToGuidsDict[rightTag];

            Op op = queryOpDict[nextOPRegex.Value[0]];

            int lookAheadOpIdx = matchIdx + 1;
            if (lookAheadOpIdx >= regexOpMatches.Count)
            {
                return op.opDelegate(currentResults, rhsTaggedList, this);

            }
            Op lookAheadOp = queryOpDict[regexOpMatches[lookAheadOpIdx].Value[0]];
            Match nextLkAhdMatch = regexOpMatches[lookAheadOpIdx];
            if (lookAheadOp.opType == OpType.Postfix)
            {
                return op.opDelegate(currentResults, rhsTaggedList, this);
            }
            do
            {
                if (lookAheadOp.opType == OpType.Prefix)
                {
                    rhsTaggedList = ActionPrefixOP(rhsTaggedList, ref rightTag, tagQ, lookAheadOp, regexOpMatches, ref lookAheadOpIdx);
                }
                else if (lookAheadOp.opType == OpType.Group)
                {
                    if (lookAheadOp.opEnder != nextLkAhdMatch.Value[0])
                    {
                        rhsTaggedList = ProcessGroup(rhsTaggedList, ref rightTag, tagQ, lookAheadOp, regexOpMatches, ref lookAheadOpIdx);
                    }
                    else
                    {
                        break;
                    }
                }
                lookAheadOpIdx++;
                if (lookAheadOpIdx >= regexOpMatches.Count)
                {
                    break;
                }
                nextLkAhdMatch = regexOpMatches[lookAheadOpIdx];
                lookAheadOp = queryOpDict[nextLkAhdMatch.Value[0]];

            } while (lookAheadOp.opType != OpType.Postfix);

            currentTag = rightTag;
            matchIdx = lookAheadOpIdx + 1;

            return currentResults = op.opDelegate(currentResults, rhsTaggedList, this);
        }

        private IEnumerable<string> ActionPrefixOP(IEnumerable<string> currentResults, ref string currentTag, Queue<string> tagQ, Op op, MatchCollection regexOpMatches, ref int matchIdx )
        {
            IEnumerable<string> rhsTaggedList = tagsToGuidsDict[currentTag];

            int lookAheadOpIdx = matchIdx + 1;

            if (lookAheadOpIdx >= regexOpMatches.Count)
            {
                return op.opDelegate(currentResults, rhsTaggedList, this);
            }
            Match nextLkAhdMatch = regexOpMatches[lookAheadOpIdx];
            Op lookAheadOp = queryOpDict[nextLkAhdMatch.Value[0]];
            if (lookAheadOp.opType == OpType.Postfix)
            {
                return op.opDelegate(currentResults, rhsTaggedList, this);
            }
            do
            {
                if (lookAheadOp.opType == OpType.Prefix)
                {
                    rhsTaggedList = ActionPrefixOP(rhsTaggedList, ref currentTag, tagQ, lookAheadOp, regexOpMatches, ref lookAheadOpIdx);
                }
                else if (lookAheadOp.opType == OpType.Group)
                {
                    if (lookAheadOp.opEnder != nextLkAhdMatch.Value[0])
                    {
                        rhsTaggedList = ProcessGroup(rhsTaggedList, ref currentTag, tagQ, lookAheadOp, regexOpMatches, ref lookAheadOpIdx);
                    }
                    else
                    {
                        break;
                    }
                    rhsTaggedList = ProcessGroup(rhsTaggedList, ref currentTag, tagQ, lookAheadOp, regexOpMatches, ref lookAheadOpIdx);
                }
                lookAheadOpIdx++;
                if (lookAheadOpIdx >= regexOpMatches.Count)
                {
                    break;
                }
                nextLkAhdMatch = regexOpMatches[lookAheadOpIdx];
                lookAheadOp = queryOpDict[nextLkAhdMatch.Value[0]];

            } while (lookAheadOp.opType != OpType.Postfix);

            matchIdx = lookAheadOpIdx + 1;

            return op.opDelegate(currentResults, rhsTaggedList, this);
        }

        private IEnumerable<string> ProcessGroup(IEnumerable<string> currentResults, ref string currentTag, Queue<string> tagQ, Op op, MatchCollection regexOpMatches, ref int matchIdx)
        {
            IEnumerable<string> rhsTaggedList = tagsToGuidsDict[currentTag];
            int lookAheadIdx = matchIdx + 1;
            Match lookAhdMatch = regexOpMatches[lookAheadIdx];
            Op lookAhdOp = queryOpDict[lookAhdMatch.Value[0]];
            bool groupEndFlag = false;

            do
            {
                switch (lookAhdOp.opType)
                {
                    case OpType.Prefix:
                        rhsTaggedList = ActionPrefixOP(rhsTaggedList, ref currentTag, tagQ, op, regexOpMatches, ref lookAheadIdx);
                        break;
                    case OpType.Postfix:
                        rhsTaggedList = ActionPostFixOp(rhsTaggedList, ref currentTag, tagQ, regexOpMatches, lookAhdMatch, ref lookAheadIdx);
                        break;
                    case OpType.Group:
                        if (lookAhdOp.opEnder != op.opEnder)
                        {// as they find an opening before the end for nested ones, this should work. A stack which pops whenever an end bracket is found does the same thing
                            rhsTaggedList = ProcessGroup(rhsTaggedList, ref currentTag, tagQ, lookAhdOp, regexOpMatches, ref lookAheadIdx);
                        }
                        else
                        {
                            groupEndFlag = true;
                        }
                        break;
                    default:
                        throw new System.InvalidOperationException("Uknown OpType: " + lookAhdOp.opType.ToString());
                }

            } while (groupEndFlag);

            matchIdx = lookAheadIdx + 1;

            return op.opDelegate(currentResults, rhsTaggedList, this);
        }

        /// <summary>
        /// Gets all tags in the current Query instance
        /// </summary>
        /// <returns>An array of the tags in the query</returns>
        public string[] GetAllTags()
        {
            string[] allTags = new string[tagsToGuidsDict.Count];
            tagsToGuidsDict.Keys.CopyTo(allTags, 0);
            return allTags;
        }

        /// <summary>
        /// Gets All the op symbols within the dictionary
        /// </summary>
        /// <returns>An active array of characters used to reference operations</returns>
        public char[] GetAllOpSymbols()
        {
            char[] outOps = new char[queryOpDict.Count];
            queryOpDict.Keys.CopyTo(outOps, 0);
            return outOps;
        }

        public bool TagExists(string tag) => tagsToGuidsDict.ContainsKey(tag);

        public bool AddOperation(Op.QueryOperation opFunction, char opChar, OpType opType, char endChar = noOpChar)
        {
            if (opChar == noOpChar)
            {
                Debug.LogError(" the null character '\0' is used as a placeholder character and may not be used in the dictionary");
                return false;
            }
            //tag regex check here
            if (queryOpDict.ContainsKey(opChar))
            {
                Debug.LogError("Attempted query operation cannot be added as op symbol is already taken: " + opChar);
                return false;
            }

            Op op = new Op()
            {
                opDelegate = opFunction,
                opType = opType,
                opSymbol = opChar
            };

            queryOpDict.Add(opChar, op);
            if (opType == OpType.Group)
            {
                op.opEnder = endChar;
                queryOpDict.Add(endChar, op);
            }
            BuildRegex();
            return true;
        }

        private IEnumerable<string> OrLstSet(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qm)
        {
            return guidSetLeft.Union(guidSetRight);
        }

        private IEnumerable<string> XorLstSet(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qm)
        {
            return guidSetLeft.Except(guidSetRight);
        }

        private IEnumerable<string> AndLstSet(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qm)
        {
            return guidSetLeft.Intersect(guidSetRight);
        }

        private IEnumerable<string> NotLstSet(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qm)
        {
            IEnumerable<string> outQuery = qm.AllGuids.Except(guidSetRight); ;
            return outQuery;
        }

        private IEnumerable<string> SingleRandSelection(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qm)
        {

            if (guidSetLeft.Count() > 0)
            {
                List<string> setToList = guidSetRight.ToList();
                return new List<string>() { setToList[Random.Range(0, setToList.Count)] };
            }
            else
            {
                return new List<string>();
            }
        }

        private IEnumerable<string> Bracket(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qm)
        {
            //this is done to action the queries that have occured between them, may be possible to just do nothing though may cause issues later on
            return guidSetLeft.ToList();
        }

        public IEnumerable<string> AllGuids
        {
            get
            {
                return allGuids;
            }
        }
    }
}
