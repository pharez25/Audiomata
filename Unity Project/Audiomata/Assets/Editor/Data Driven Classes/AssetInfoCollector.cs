
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Audio;

public static class AssetInfoCollector
{
    public static string[] GetSnapshotNames(string mixerPath)
    {
        var mixerAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(mixerPath);

        List<string> snapshotNames = new List<string>();
        for (int i = 0; i < mixerAssets.Length; i++)
        {
            AudioMixerSnapshot snapshot = mixerAssets[i] as AudioMixerSnapshot;

            if (snapshot)
            {
                snapshotNames.Add(snapshot.name);
            }
        }

        return snapshotNames.ToArray();
    }

    public static Track[] GetAudioAssets()
    {
        string[] foundGUIDs = AssetDatabase.FindAssets("t:AudioClip");

        Track[] outTracks = new Track[foundGUIDs.Length];

        for (int i = 0; i < outTracks.Length; i++)
        {
            Track next;
            next.guid = foundGUIDs[i];
            next.path = AssetDatabase.GUIDToAssetPath(next.guid);
            next.name = Path.GetFileNameWithoutExtension(next.path);

        }
        return outTracks;}

    // copy GetAudioAssets and Track struct to this
}
