using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Audiomata
{
    public class QueryManager
    {
        //USE NORMAL DICTIONARIES O(1) vs O(n)
        private SortedDictionary<string, List<string>> tagsToGuidsDict;
        private SortedDictionary<string, AudioClip> guidToClipDict;
       
        
        public QueryManager(AudioData[] audioData)
        {
            tagsToGuidsDict = new SortedDictionary<string, List<string>>();
            guidToClipDict = new SortedDictionary<string, AudioClip>();

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
                string guidChoice = guids[Random.Range(0, guids.Count)];
                return guidToClipDict[guidChoice];
            }

            return null;
        }

        public AudioClip GetTrackById(string id) => guidToClipDict[id];

        public AudioClip QueryAudio(string query)
        {
            char op = '3';
            //work out the above then complete the below : )
            switch (op)
            {
                case '&':

                    break;
                default:
                    throw new InvalidOperationException("Unknown Op Character, Audiomata query invalid");
                   
            }

            throw new System.NotImplementedException();
        }
        
        private IQueryable<string> XorTagQ(string subQString, int opIndex, IQueryable<string> current)
        {
            string tag1 = subQString.Substring(0, opIndex);
            int t2StartIdx = opIndex + 1;
            string tag2 = subQString.Substring(t2StartIdx, subQString.Length - t2StartIdx);

            IQueryable<string> nextQuery = from a in current
                                          where (a == tag1) ^ (a == tag2)
                                          select a;
            return nextQuery;
        }

        private IQueryable<string> OrTagQ(string subQString, int opIndex, IQueryable<string> current)
        {
            string tag1 = subQString.Substring(0, opIndex);
            int t2StartIdx = opIndex + 1;
            string tag2 = subQString.Substring(t2StartIdx, subQString.Length - t2StartIdx);

            IQueryable<string> nextQuery = from a in current
                                           where a == tag1 || a == tag2
                                           select a;
            return nextQuery;
        }

        private IQueryable<string> AndTagQ(string subQString, int opIndex, IQueryable<string> current)
        {
            string tag1 = subQString.Substring(0, opIndex);
            int t2StartIdx = opIndex + 1;
            string tag2 = subQString.Substring(t2StartIdx, subQString.Length - t2StartIdx);

            IQueryable<string> nextQuery = from a in current
                                           where a == tag1 && a == tag2
                                           select a;
            return nextQuery;
        }

        private IQueryable<string>EqualTagQ(string subQString, int opIndex, IQueryable<string> current)
        {
            IQueryable<string> nextQuery = from a in current
                                           where a == subQString
                                           select a;
            return nextQuery;
        }
    }

   
}