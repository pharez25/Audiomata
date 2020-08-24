using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Audiomata
{
    public static class MetaDataManager
    {
        private static readonly string dataDirectory = @"Assets\Audiomata\Generated Data\Track Metadata\";
        public static string FileNamePrefix { get{return "SampleData-";} }

        public static AudioData[] GenerateAndLoadAllAudioData()
        {
            if (!Application.isEditor)
            {
                Debug.LogError("Cannot Use AssetImporter outside of Unity Editor!");
                return null;
            }

            string[] audioClipGuids = AssetDatabase.FindAssets("t:AudioClip");
            string[] clipFilePaths = new string[audioClipGuids.Length];

            AudioData[] samples = new AudioData[audioClipGuids.Length];

            for (int i = 0; i < audioClipGuids.Length; i++)
            {
                string nextGuid = audioClipGuids[i];


                string nextPath = AssetDatabase.GUIDToAssetPath(nextGuid);
                clipFilePaths[i] = nextPath;
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(nextPath);
                samples[i] = CreateMetaData(nextGuid, new string[0], clip);
            }

            return samples;
        }

        /// <summary>
        /// Removes any excess files from metadata folder, will also remove all files that are not AudioData 
        /// </summary>
        public static void CleanAudioData()
        {
            string[] guids = AssetDatabase.FindAssets("t:AudioData");

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                AudioData next = AssetDatabase.LoadAssetAtPath<AudioData>(path);

                if (!next.clip)
                {
                    AssetDatabase.DeleteAsset(path);
                }
            }



        }

        public static AudioData[] LoadAllAudioData()
        {
            string[] guids = AssetDatabase.FindAssets("t:AudioData");
            AudioData[] allAudioData = new AudioData[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string nextGuid = guids[i];

                AudioData nextAudioData = AssetDatabase.LoadAssetAtPath<AudioData>(AssetDatabase.GUIDToAssetPath(nextGuid));
                allAudioData[i] = nextAudioData;
                
            }

            return allAudioData;

        }

        public static AudioData GetSampleData(string clipGuid) => AssetDatabase.LoadAssetAtPath<AudioData>(GetPath(clipGuid));

        public static bool MetaDataExists(string clipGuid) => AssetDatabase.LoadAssetAtPath<AudioData>(GetPath(clipGuid)) != null;

        private static string GetPath(string clipGuid) => dataDirectory + FileNamePrefix + clipGuid + ".asset";

        public static string SampleNameToGuid(string audioDataName)
        {
            return audioDataName.Substring(FileNamePrefix.Length - 1, FileNamePrefix.Length - audioDataName.Length);
        }

        public static AudioData CreateMetaData(string clipGuid, string[] tags, AudioClip clip)
        {
            AudioData data = GetSampleData(clipGuid);

            if (data)
            {
                return data;
            }

            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            data = ScriptableObject.CreateInstance<AudioData>();
            data.clip = clip;
            data.tags = new List<string>();
            data.tags.AddRange(tags);
            data.guid = clipGuid;
            AssetDatabase.CreateAsset(data, GetPath(clipGuid));

            return data;
        }
    }
}