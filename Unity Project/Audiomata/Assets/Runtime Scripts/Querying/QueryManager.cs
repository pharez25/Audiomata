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

        /// <summary>
        /// Creates an instance of a class that can query tags with in buil or custom operations
        /// </summary>
        /// <param name="audioData">an array of all audio data that can be queried</param>
        /// <param name="customOperators">A dictionary of all custom ops</param>
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
            OP and, or, not;

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

            queryOpDict = new Dictionary<char, OP>()
                    {
                        { '&', and},
                        {'|',or },
                        {'!',not }
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
        /// An Operation to be done with the query manager
        /// </summary>
        /// <param name="currentSet"> the current result of all previous operations, modify this to apply ops </param>
        /// <param name="targetTag">the results of a dicionary tag query to the right hand side of operator of ids</string></param>
        /// <remarks>All IEnumerables are of type List<string>, a change of type can cause unintended faults</remarks>        

        public delegate void QueryOperation(ref IEnumerable<string> currentSet, IEnumerable<string> targetTag);

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
            
            IEnumerable<string> currentQueryCumlation = new List<string>(tagsToGuidsDict.Keys);
            MatchCollection opMatches = opRegex.Matches(query);

            string selectedGuid;

            if (opMatches.Count > 0)
            {
                //predicate allows chained prefixes where usable e.g.(although not techincally workable) !!tag
                Queue<string> tagsInQuery = new Queue<string>(opRegex.Split(query).Where(tag => !string.IsNullOrEmpty(tag)));
                string previousTag;
                string nextTag = tagsInQuery.Dequeue();

                for (int i = 0; i < opMatches.Count; i++)
                {
                    Match nextMatch = opMatches[i];
                    
                    OP nextOp = queryOpDict[nextMatch.Value[0]];

                    switch (nextOp.opType)
                    {

                        case OpType.Postfix:
                            previousTag = nextTag;
                            //all postfix operators  must be between 2 tags
                            // 2 tags are the only reason why tags can be dequed
                            nextTag = tagsInQuery.Dequeue();

                            if(i == 0)
                            {
                                currentQueryCumlation = tagsToGuidsDict[previousTag];
                            }
                            else
                            {
                                while (opMatches.Count - i > 1 && tagsInQuery.Count > 0)
                                {
                                    OP lookAheadOp = queryOpDict[opMatches[i + 1].Value[0]];

                                    if(lookAheadOp.opType == OpType.Prefix)
                                    {
                                        IEnumerable<string> prefIxResults = new List<string>(guidToClipDict.Keys);
                                         
                                        lookAheadOp.opSymbol()

                                        i++;
                                    }
                                    else break;
                                }
                            }

                            nextOp.opFunction(ref currentQueryCumlation, tagsToGuidsDict[nextTag]);
                            break;

                        case OpType.Prefix:
                            nextOp.opFunction(ref currentQueryCumlation, tagsToGuidsDict[nextTag]);
                            break;

                        case OpType.Group:
                            throw new System.NotImplementedException("The group feature, including brackets \"()\" has not yet been completed!");
                            break;

                        default:
                            throw new System.InvalidOperationException("Optype not recognised: "+nextOp.opType.ToString());


                    }
                }
                    
            }
            else //assumes there is just one tag 
            {
                Debug.LogWarning("Should only use QueryAudio where multiple tags are involved");

                if (tagsToGuidsDict.TryGetValue(query, out List<string> guidList))
                {
                    if (guidList.Count > 0)
                    {
                        selectedGuid = guidList[Random.Range(0, guidList.Count)];

                        return guidToClipDict[selectedGuid];
                    }
                    else return null;

                }
                else return null;
            }

            if (currentQueryCumlation.Count() < 1)
            {
                Debug.LogWarning("query: \"" + query + "\" did not return results");
                return null;
            }

            List<string> validGuids = currentQueryCumlation.ToList();
            selectedGuid = validGuids[Random.Range(0, validGuids.Count)];

            return guidToClipDict[selectedGuid];
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
                else return opSymbol  opEnder;
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

        private void OrLstSet(ref IEnumerable<string> currentSet, IEnumerable<string> nextTagResult)
        {
            currentSet = currentSet.Union(nextTagResult);
        }

        private void AndLstSet(ref IEnumerable<string> currentSet, IEnumerable<string> nextTagResult)
        {
            currentSet = currentSet.Intersect(nextTagResult);
        }

        private void NotLstSet(ref IEnumerable<string> currentSet, IEnumerable<string> nextTagResult)
        {
            currentSet = currentSet.Except(nextTagResult);
        }

        private void SingleRandSelection(ref IEnumerable<string> currentSet, IEnumerable<string> nextTagResult = null)
        {
            if (currentSet.Count() > 0)
            {
                List<string> setToList = currentSet.ToList();
                currentSet = new List<string>() { setToList[Random.Range(0, setToList.Count)] };
            }
        }
    }
}