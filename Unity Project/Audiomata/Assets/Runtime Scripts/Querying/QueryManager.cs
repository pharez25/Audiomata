using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Audiomata
{
    public class QueryManager
    {
        //REFACTOR JUST USE AUDIODATA OR CLIPS
        private Dictionary<string, List<string>> tagsToGuidsDict;
        private Dictionary<string, AudioClip> guidToClipDict;
        private readonly Dictionary<char, PerformOperation> queryOpDict;

        private const char noOpChar = '~';

        public QueryManager(AudioData[] audioData, bool queryWarnings = true)
        {
            tagsToGuidsDict = new Dictionary<string, List<string>>();
            guidToClipDict = new Dictionary<string, AudioClip>();
            queryOpDict = new Dictionary<char, PerformOperation>()
            {
                ['|'] = OrLstSet,
                ['&'] = AndLstSet,
              //  ['^'] = XorTagQ,
                ['!'] = NotLstSet,
               // ['='] = EqualTagQ
            };

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

        public delegate void PerformOperation(ref IEnumerable<string> currentSet, string targetTag);

        /// <summary>
        /// Queries all the tags and returns a random audioclip from the filtered set
        /// </summary>
        /// <param name="query">string to query, follows c# syntax e.g. "example|!Unity&Audiomata"</param>
        /// <returns>An AudioClip that matches the query criteria </returns>
        public AudioClip QueryAudio(string query)
        {
            string nextTag = GetNextTag(query, out char currentOp, out int lastStopIdx);
            if (nextTag == null)
            {
                Debug.LogError("Querier Failed to find operators");
                return null;
            }
            Debug.Log(nextTag);

            IEnumerable<string> currentSet;

            string[] tmpGuids = new string[guidToClipDict.Count];
            guidToClipDict.Keys.CopyTo(tmpGuids, 0);
            currentSet = tmpGuids;
            queryOpDict[currentOp](ref currentSet, nextTag);

            for (int i = lastStopIdx; i < query.Length; i++)
            {
                nextTag = GetNextTag(query, out currentOp, out i, i);
                Debug.Log(nextTag);
                if(nextTag == null)
                {
                    break;
                }

                queryOpDict[currentOp](ref currentSet, nextTag);
            }

            List<string> filteredGuids = currentSet.ToList();
            if (filteredGuids.Count > 0)
            {
                string selectedGuid = filteredGuids[UnityEngine.Random.Range(0, filteredGuids.Count)];
                return guidToClipDict[selectedGuid];

            }
            return null;
        }

        

        private string GetNextTag(string query, out char nextOpChar, out int stoppageIdx, int startIdx = 0)
        {
            string nextTag = null;

            for (int i = startIdx; i < query.Length; i++)
            {
                char next = query[i];

                if (IsOp(next))
                {

                    if (next == '!')
                    {
                        int nextOpIdx = i + 1;

                        while (nextOpIdx < query.Length && !IsOp(query[nextOpIdx]))
                        {
                            nextOpIdx++;
                        }

                        nextTag = query.Substring(startIdx, i - startIdx);
                    }
                    else
                    {
                        nextTag = query.Substring(startIdx, i);
                    }

                    nextOpChar = next;
                    stoppageIdx = i;
                    return nextTag;

                }
            }

            nextOpChar = noOpChar;
            stoppageIdx = query.Length;
            return nextTag;
        }

        private bool IsOp(char c) => queryOpDict.ContainsKey(c);
        
        public string[] GetAllTags()
        {
            string[] allTags = new string[tagsToGuidsDict.Count];
            tagsToGuidsDict.Keys.CopyTo(allTags, 0);
            return allTags;
        }

        //ops where done this way to allow very easy expansion where necessary. Arguably would be more efficient to use a swith statement
        private void OrLstSet(ref IEnumerable<string> currentSet, string targetTag)
        {
            List<string> tags;

            if (tagsToGuidsDict.TryGetValue(targetTag, out tags))
            {
                currentSet =  currentSet.Union(tagsToGuidsDict[targetTag]);
            }
            else
            {
                Debug.LogWarning("Could not perform an OR ('|') step due to tag not being present");
            }
        }

        private void AndLstSet(ref IEnumerable<string> currentSet, string targetTag)
        {
            List<string> tags = tagsToGuidsDict[targetTag];

            if(tagsToGuidsDict.TryGetValue(targetTag, out tags))
            {
                currentSet = currentSet.Where(t1 => tags.Contains(t1));
            }
            else
            {
                Debug.LogWarning("Could not perform an AND ('&') step due to the tags not being present");
            }
          
        }

        private void NotLstSet(ref IEnumerable<string> currentSet, string targetTag)
        {
            List<string> tags;

            if(tagsToGuidsDict.TryGetValue(targetTag, out tags))
            {
                currentSet = currentSet.Except(tagsToGuidsDict[targetTag]);
            }
            else
            {
                Debug.LogWarning("! did not return a taglist");
            }
        }
    }
}