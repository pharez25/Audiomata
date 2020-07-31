using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Class manages the tagging of Audio Clips
/// </summary>
public class TrackTagger
{
    Dictionary<string, AudioData> trackDataDict;
    

   public TrackTagger()
    {
        trackDataDict = new Dictionary<string, AudioData>();
        AudioData[] tracks = AssetImporter.GenerateAndLoadAllAudioData();

        for (int i = 0; i < tracks.Length; i++)
        {
            
        }
    }

    public void RefreshTrackList()
    {

    }

    public AudioData GetTrack(string guid)
    {
        return null;
    }

    public AudioData[] GetTracksByTag(string tag)
    {
        return null;
    }
}

