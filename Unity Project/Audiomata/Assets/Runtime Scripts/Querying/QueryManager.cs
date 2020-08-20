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
        /// <summary>
        /// a dictionary of tags to clip guids, this is done because of the idea of adding a clip name based dictionary in the future
        /// </summary>
        private Dictionary<string, List<string>> tagsToGuidsDict;
        private Dictionary<string, string> nameToGuidDict;
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
            nameToGuidDict = new Dictionary<string, string>();

            //populate audio data
            for (int i = 0; i < audioData.Length; i++)
            {
                AudioData nextAD = audioData[i];
                nameToGuidDict[nextAD.name] = nextAD.guid;

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
                opType = OpType.CaptureGroup
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

        /// <summary>
        /// Gets a track by clip name
        /// </summary>
        /// <param name="name">The name of the clip</param>
        /// <returns>The audio clip if found</returns>
        /// <remarks>Names get overwritten (by the one lowest down the list) and will throw exceptions for invalid names</remarks>
        public AudioClip GetTrackbyName(string name) => guidToClipDict[nameToGuidDict[name]];
        

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

            IEnumerable<string> resultGuidList = null;
            MatchCollection opMatches = opRegex.Matches(query);

            if (opMatches.Count > 0)
            {

                resultGuidList = PerformOps(query, opMatches);
            }
            else //assumes there is just one tag if there are no ops 
            {
                Debug.LogWarning("\"" + query + "\" may have invalid operators as none were detected if just one tag was required, use GetClipByTag");
                return GetClipByTag(query);
            }

            if (resultGuidList == null)
            {
                return null;
            }
            if (resultGuidList.Count() < 1)
            {
                return null;
            }

            List<string> queryResultLst = resultGuidList.ToList();
            if (queryResultLst.Count < 1)
            {
                Debug.LogWarning("query: \"" + query + "\", returned no results");
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
                currentGuidSet = PerformOps(query, opMatches);
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
                    resultTarget = new AudioClip[0];
                    return;
                }

            }

            List<string> searchResults = currentGuidSet.ToList();

            if (searchResults == null)
            {
                Debug.LogWarning("query: \"" + query + "\", returned no results");
                resultTarget = null;
                return;
            }
            if (searchResults.Count < 1)
            {
                Debug.LogWarning("query: \"" + query + "\", returned no results");
                resultTarget = new AudioClip[0];
                return;
            }

           
            resultTarget = new AudioClip[searchResults.Count];
            for (int i = 0; i < searchResults.Count; i++)
            {
                resultTarget[i] = guidToClipDict[searchResults[i]];
            }

        }

        private IEnumerable<string> PerformOps(string query, MatchCollection opMatches)
        {
            string[] tags = opRegex.Split(query).Where(q => !string.IsNullOrEmpty(q)).ToArray();

            IEnumerable<string> lhs = null;
            IEnumerable<string> rhs = null;

            Stack<string> tagStk = new Stack<string>(tags);
            Stack<Match> opStack = new Stack<Match>();

            for (int i = 0; i < opMatches.Count; i++)
            {
                opStack.Push(opMatches[i]);

            }

            string currentTag = tagStk.Pop();
            rhs = tagsToGuidsDict[currentTag];

            do
            {
                Op nextOp = PopToOp(opStack, out Match nextMatch);
                switch (nextOp.opType)
                {
                    case OpType.Prefix:
                        rhs = nextOp.opDelegate(null, rhs, this);
                        continue;

                    case OpType.Postfix:
                        
                        lhs = GetLhs(tagStk, opStack);
                        //handle postfix
                        break;

                    case OpType.CaptureGroup:
                        if(nextMatch.Value[0] == nextOp.opEnder)
                        {
                            lhs = GetGroup(tagStk, nextOp, opStack, null);
                        }
                        break;
                    default:
                        break;
                }

                rhs = nextOp.opDelegate(lhs, rhs, this);
            } while (opStack.Count > 0);

            return rhs;
        }

        private IEnumerable<string> GetLhs(Stack<string> tagStack, Stack<Match> opStack)
        {
            string nextTag = tagStack.Pop();
            
            IEnumerable<string> nextSet = tagsToGuidsDict[nextTag];

            //ungrouped postfix always marks the end of a set of operations

            while (opStack.Count > 0)
            {
                Op nextOp = PeekToOp(opStack, out Match nextMatch);
                switch (nextOp.opType)
                {
                    case OpType.Prefix:
                        opStack.Pop();
                        nextSet = nextOp.opDelegate(null, nextSet, this);
                        break;
                    //all postfixes found here should not be inside brackets
                    //therefore they will be handled by the outerloop
                    case OpType.Postfix:
                        return nextSet;

                    case OpType.CaptureGroup:
                        opStack.Pop(); //need to consider group ends
                        if (nextOp.opSymbol == nextMatch.Value[0])
                        {
                            return nextSet;
                        }
                        nextSet = GetGroup(tagStack, nextOp, opStack, nextTag);
                        return nextSet;
                    default:
                        break;
                }
            }
            return nextSet;
        }

        private Op PopToOp(Stack<Match> opStack)
        {
            if (opStack.Count > 0)
            {
                Match nextMatch = opStack.Pop();
                Op op = queryOpDict[nextMatch.Value[0]];
                return op;
            }

            return null;
        }

        private Op PopToOp(Stack<Match> opStack, out Match opMatch)
        {
            if (opStack.Count > 0)
            {
                opMatch = opStack.Pop();
                Op op = queryOpDict[opMatch.Value[0]];

                return op;
            }
            opMatch = null;

            return null;
        }

        private Op PeekToOp(Stack<Match> opStack)
        {
            if (opStack.Count > 0)
            {
                Match nextMatch = opStack.Peek();
                Op op = queryOpDict[nextMatch.Value[0]];
                return op;
            }

            return null;
        }

        private Op PeekToOp(Stack<Match> opStack, out Match opMatch)
        {
            if (opStack.Count > 0)
            {
                opMatch = opStack.Peek();
                Op op = queryOpDict[opMatch.Value[0]];

                return op;
            }
            opMatch = null;

            return null;
        }

        private IEnumerable<string> GetGroup(Stack<string> tagStack, Op groupOp, Stack<Match> opStack, string currentTag = null)
        {
            if (currentTag == null)
            {
                currentTag = tagStack.Pop();
            }
            IEnumerable<string> currentResults = tagsToGuidsDict[currentTag];


            while (opStack.Count > 0)
            {
                Match nextMatch = opStack.Pop();
                Op nextOp = queryOpDict[nextMatch.Value[0]];

                switch (nextOp.opType)
                {
                    case OpType.Prefix:
                        
                        currentResults = nextOp.opDelegate(null, currentResults, this);
                        break;

                    case OpType.Postfix:
                        IEnumerable<string> lhs = GetLhs(tagStack, opStack);
                        currentResults = nextOp.opDelegate(lhs, currentResults, this);
                        break;

                    case OpType.CaptureGroup:
                        
                        //whenever an end bracket is met, the system should stop;
                        if (nextMatch.Value[0] == nextOp.opSymbol)
                        {
                            return nextOp.opDelegate(null, currentResults, this);
                        }
                        currentResults = GetGroup(tagStack, nextOp, opStack, currentTag);
                        break;

                    default:
                        break;
                }
            }

            //get operators until closing operator is found
            // if another group, call this function again (and increment depth by 1?)
            //otherwise do prefixes and call getlhs where needed
            //perform group operation
            return groupOp.opDelegate(null,currentResults,this);
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
        /// <returns>The active array of characters used (as keys) to reference operations</returns>
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
            if (opType == OpType.CaptureGroup)
            {
                op.opEnder = endChar;
                queryOpDict.Add(endChar, op);
            }
            BuildRegex();
            return true;
        }

        private IEnumerable<string> OrLstSet(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qm)
        {
            var outQ = guidSetLeft.Union(guidSetRight);
            Debug.Log("OR Results");
            DebugPrint(outQ);
            return outQ;
        }

        private IEnumerable<string> XorLstSet(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qm)
        {
            IEnumerable<string> left = guidSetLeft.Except(guidSetRight);
            IEnumerable<string> right = guidSetRight.Except(guidSetLeft);

            return left.Union(right);
        }

        private IEnumerable<string> AndLstSet(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qm)
        {
            var outQ = guidSetLeft.Intersect(guidSetRight);
            Debug.Log("AND Results");
            DebugPrint(outQ);
            return outQ;
        }

        private IEnumerable<string> NotLstSet(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qm)
        {
            IEnumerable<string> outQuery = qm.AllGuids.Except(guidSetRight); ;
            Debug.Log("NOT Results");
            DebugPrint(outQuery);
            return outQuery;
        }

        private void DebugPrint(IEnumerable<string> set)
        {
            Debug.Log("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
            List<string> setLst = set.ToList();

            foreach (string guid in setLst)
            {
                AudioClip next = guidToClipDict[guid];

                Debug.Log(next.name);
            }
            Debug.Log("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
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
            //this is done to action the queries that have occured between them (Enumerables remain as a set of queries until values requested)
            //may be possible to just do nothing though may cause issues later on

            var outSet = guidSetRight.ToList();

            Debug.Log("Out Results: ");
            return outSet;
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
