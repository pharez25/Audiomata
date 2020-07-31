using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;


public static class AssetImporter
{
    private static string dataDirectory = @"Assets\Audiomata\Generated Data\Track Metadata\";
    private static string fileNamePrefix = "SampleData-";

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

    public static AudioData GetSampleData(string clipGuid) => AssetDatabase.LoadAssetAtPath<AudioData>(GetPath(clipGuid));

    public static bool SampleExists(string clipGuid) => AssetDatabase.LoadAssetAtPath<AudioData>(GetPath(clipGuid)) != null;
    
    private static string GetPath(string clipGuid) => dataDirectory + fileNamePrefix + clipGuid + ".asset";

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
