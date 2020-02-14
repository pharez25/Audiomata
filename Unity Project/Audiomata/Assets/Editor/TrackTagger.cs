using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class manages the tagging of Audio Clips
/// </summary>
class TrackTagger
{

    /// <summary>
    /// Gets all Audio Clips from within the project
    /// </summary>
    /// <returns>an array of all audio clip names from within the project </returns>
    public  string[] GetAllAudio()
    {
        string[] foundAssets = AssetDatabase.FindAssets("t:AudioClip");

        string[] trackNames = new string[foundAssets.Length];

        for (int i = 0; i < trackNames.Length; i++)
        {
            string nextClip = AssetDatabase.GUIDToAssetPath(foundAssets[i]);
            int lastSlash = nextClip.LastIndexOf('/');
            nextClip = nextClip.Remove(0, lastSlash + 1);
            int dotIdx = nextClip.LastIndexOf('.');
            nextClip = nextClip.Remove(dotIdx, nextClip.Length-dotIdx);

            trackNames[i] = nextClip;
        }


        return trackNames;
    }

 }

