using System.Collections.Generic;
using UnityEngine;

namespace Audiomata
{
    public class QueryManager
    {
        private Dictionary<string, List<string>> tagsToGuidsDict;
        private Dictionary<string, AudioClip> guidToClipDict;

        public QueryManager(AudioData[] audioData)
        {
            tagsToGuidsDict = new Dictionary<string, List<string>>();
            guidToClipDict = new Dictionary<string, AudioClip>();

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
                string guidChoice = guids[Random.Range(0, guids.Count)];
                return guidToClipDict[guidChoice];
            }

            return null;
        }

        public AudioClip GetTrackById(string id) => guidToClipDict[id];
    }
}