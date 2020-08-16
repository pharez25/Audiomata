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
        private Dictionary<char, OP> queryOpDict;
        private Regex opRegex;
        private const char noOpChar = '\0';

        private readonly List<string> allGuids;

        //done to make a "readonly" acessor


        /// <summary>
        /// An Operation to be done with the query manager
        /// </summary>
        /// <param name="guidSetLeft"> the current result of all previous operations, modify this to apply ops </param>
        /// <param name="guidSetRight">the results of a dicionary tag query to the right hand side of operator of ids</string></param>
        /// <remarks>All IEnumerables are of type List<string>, a change of type can cause unintended faults</remarks>       
        public delegate IEnumerable<string> QueryOperation(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qmInstance = null);

        /// <summary>
        /// Creates an instance of a class that can query tags with in buil or custom operations
        /// </summary>
        /// <param name="audioData">an array of all audio data that can be queried</param>
        /// <param name="customOperators">A dictionary of custom Operators</param>
        /// <param name="overwriteDictionary">Whether in built operations should be overrided </param>
        /// <remarks>If replace operations is true, the dictionary will be completely replaced set false for merge</remarks>

        public QueryManager(AudioData[] audioData, Dictionary<char, OP> customOperators = null, bool overwriteDictionary = false)
        {
            tagsToGuidsDict = new Dictionary<string, List<string>>();
            guidToClipDict = new Dictionary<string, AudioClip>();
            queryOpDict = new Dictionary<char, OP>();

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
                    foreach (KeyValuePair<char, OP> opKvp in customOperators)
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
            OP and, or, not, rand, xor, contains;

            and = new OP()
            {
                opFunction = AndLstSet,
                opSymbol = '&',
                opType = OpType.Postfix
            };

            or = new OP()
            {
                opFunction = OrLstSet,
                opSymbol = '|',
                opType = OpType.Postfix
            };

            not = new OP()
            {
                opFunction = NotLstSet,
                opSymbol = '!',
                opType = OpType.Prefix
            };

            xor = new OP
            {
                opFunction = XorLstSet,
                opSymbol = '^',
                opType = OpType.Postfix
            };

            rand = new OP()
            {
                opFunction = SingleRandSelection,
                opSymbol = '?',
                opType = OpType.Prefix
            };

            queryOpDict = new Dictionary<char, OP>()
                    {
                        {and.opSymbol, and},
                        {or.opSymbol,or },
                        {not.opSymbol,not },
                        {xor.opSymbol,xor },
                        { rand.opSymbol,rand}
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
                string nextTag = tagsInQuery.Dequeue();
                for (int i = 0; i < opMatches.Count; i++)
                {
                    Match nextOpMatch = opMatches[i];
                    OP nextOp = queryOpDict[nextOpMatch.Value[0]];
                    currentGuidSet = DoOperation(nextOp, opMatches, tagsInQuery, currentGuidSet, ref nextTag, query, ref i);
                }
            }
            else //assumes there is just one tag 
            {
                Debug.LogWarning("Should only use QueryManager.QueryAudio for single tag queries, it will call there instead anyway!");
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

        public void QueryAudio(string query, out IEnumerable<string> result)
        {
            throw new System.NotImplementedException("IEnumerator also not done");
        }

        public IEnumerable<string> DoOperation(OP op, MatchCollection allMatches, Queue<string> tagQ, IEnumerable<string> currentResults, ref string currentTag, string q, ref int matchIdx, int bracketDepth = 0)
        {
            if (currentResults == null)
            {
                currentResults = new List<string>(tagsToGuidsDict[currentTag]);
            }
            IEnumerable<string> rhsTaggedList;
            switch (op.opType)
            {
                //this case should only be entered when prefix is used first
                case OpType.Prefix:
                    rhsTaggedList = tagsToGuidsDict[currentTag];
                    currentResults = op.opFunction(currentResults, rhsTaggedList, this);
                    break;


                case OpType.Postfix:
                    //check and execute prefixes before continuing
                    string rightTag = tagQ.Dequeue();
                    Debug.Log(rightTag);
                    rhsTaggedList = tagsToGuidsDict[rightTag];
                    int lookAheadOpIdx = matchIdx + 1;
                    if (lookAheadOpIdx >= allMatches.Count)
                    {
                        rhsTaggedList = op.opFunction(currentResults, rhsTaggedList, this);
                        break;
                    }
                    OP lookAheadOp = queryOpDict[allMatches[lookAheadOpIdx].Value[0]];
                    if (lookAheadOp.opType != OpType.Prefix)
                    {
                        break;
                    }
                    do
                    {
                        rhsTaggedList = lookAheadOp.opFunction(currentResults, rhsTaggedList, this);
                        lookAheadOpIdx++;
                        if (lookAheadOpIdx >= allMatches.Count)
                        {
                            break;
                        }
                        lookAheadOp = queryOpDict[allMatches[lookAheadOpIdx].Value[0]];
                    } while (lookAheadOp.opType == OpType.Prefix);

                    matchIdx = lookAheadOpIdx + 1;
                    currentResults = op.opFunction(currentResults, rhsTaggedList, this);
                    break;


                case OpType.Group:
                    break;


                default:
                    Debug.LogError("Operation Failed, unknown optype. If this a customisation please update this class");
                    return null;
            }
            return currentResults;
        }

        /// <summary>
        /// A structure class used to store data for query manager operations
        /// </summary>
        public class OP
        {
            /// <summary>
            /// delegate function which performs this operation
            /// </summary>
            public QueryOperation opFunction;
            /// <summary>
            /// The type of operation
            /// </summary>
            public OpType opType;
            /// <summary>
            /// the simple that indicates the operation or the start of it
            /// </summary>
            public char opSymbol;
            /// <summary>
            /// the symbol that indicates the end of the operation (optional and not always required
            /// </summary>
            public char opEnder = noOpChar;

            public string GetString()
            {
                if (opEnder == noOpChar)
                {
                    return opSymbol.ToString();
                }
                else return "" + opSymbol + opEnder;
            }
        }

        /// <summary>
        /// The way in which the operation character is inseterted into a query
        /// </summary>
        public enum OpType
        {
            /// <summary>
            /// Before the tag only operates on one tag
            /// </summary>
            Prefix,
            /// <summary>
            /// After the tag, only operates on 2 tags
            /// </summary>
            Postfix,
            /// <summary>
            /// As a seperator between multiple tags e.g. [example&other]&something-else
            /// </summary>
            /// <remarks>Does the operations within first then returns the values, this may change in the future</remarks>
            Group
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

        public char[] GetAllOpSymbols()
        {
            char[] outOps = new char[queryOpDict.Count];
            queryOpDict.Keys.CopyTo(outOps, 0);
            return outOps;
        }

        public bool TagExists(string tag) => tagsToGuidsDict.ContainsKey(tag);

        public bool AddOperation(QueryOperation opFunction, char opChar, OpType opType)
        {
            if (opChar == noOpChar)
            {
                Debug.Log(noOpChar + " is used as a placeholder character and as such may not be used in the dictionary");
            }
            //tag regex check here
            if (queryOpDict.ContainsKey(opChar))
            {
                Debug.LogError("Attempted query operation cannot be added as op symbol is already taken: " + opChar);
                return false;
            }



            OP op = new OP()
            {
                opFunction = opFunction,
                opType = opType,
                opSymbol = opChar
            };

            queryOpDict.Add(opChar, op);
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
            var tmpLst = outQuery.ToList();

            return outQuery;
        }

        private IEnumerable<string> SingleRandSelection(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qm)
        {

            if (guidSetLeft.Count() > 0)
            {
                List<string> setToList = guidSetLeft.ToList();
                return new List<string>() { setToList[Random.Range(0, setToList.Count)] };
            }
            else
            {
                return new List<string>();
            }
        }

        private IEnumerable<string> Bracket(IEnumerable<string> guidSetLeft, IEnumerable<string> guidSetRight, QueryManager qm)
        {
            //this is done to action the queries that have occured between them, may be possible to just do nothing
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
