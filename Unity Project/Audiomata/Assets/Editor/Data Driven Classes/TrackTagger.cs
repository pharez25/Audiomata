using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Class manages the tagging of Audio Clips
/// </summary>
public class TrackTagger
{
    /// <summary>
    /// Dictionary of guids against a list of their tags
    /// </summary>
    private Dictionary<string, List<string>> trackTagDict;

    /// <summary>
    /// clipId to info
    /// 
    /// </summary>
    private Dictionary<string, Track> assetDict;

    public TrackTagger()
    {
        trackTagDict = new Dictionary<string, List<string>>();
        assetDict = new Dictionary<string, Track>();
        GetAudioAssets(true);
    }

    /// <summary>
    /// Gets all Audio Clips from within the project updates dictionary of tracks where appropriate
    /// </summary>
    /// <param name="refresh">If false returns cached copy of all tracks within the class</param>
    /// <returns>an array of all audio clip names from within the project </returns>
    public Track[] GetAudioAssets(bool refresh = false)
    {
        if (!refresh)
        {
            Track[] allTracks = new Track[assetDict.Count];
            assetDict.Values.CopyTo(allTracks, 0);
            return allTracks;
        }

        string[] foundAssets = AssetDatabase.FindAssets("t:AudioClip");

        Track[] trackNames = new Track[foundAssets.Length];

        for (int i = 0; i < trackNames.Length; i++)
        {
            Track nextTrack;
            nextTrack.guid = foundAssets[i];
            nextTrack.path = AssetDatabase.GUIDToAssetPath(nextTrack.guid);
            nextTrack.name = Path.GetFileNameWithoutExtension(nextTrack.path);
            trackNames[i] = nextTrack;

            if (!assetDict.ContainsKey(nextTrack.guid))
            {
                assetDict.Add(nextTrack.guid, nextTrack);
            }
        }
        return trackNames;
    }


    /// <summary>
    /// Gets track from internal asset dictionary
    /// </summary>
    /// <param name="id">key identifier for track</param>
    /// <param name="track">the track if found</param>
    /// <returns>true if the track was found</returns>
    public bool GetTrack(string id, out Track track)
    {
        return assetDict.TryGetValue(id, out track);
    }

    /// <summary>
    /// Updates any data stored within a Track strucutre inside this class
    /// </summary>
    /// <param name="track">The new track data to replace the old</param>
    /// <param name="idOld">The previous id, used if the id has been updated</param>
    public bool UpdateTrack(Track track, string idOld = null)
    {
        if (idOld != null)
        {
            if (!assetDict.Remove(idOld))
            {
                return false;
            }
            assetDict.Add(track.guid, track);

            //update keys in trackTagDictionary as well
            List<string>[] trackTagValues = new List<string>[trackTagDict.Values.Count];

            trackTagDict.Values.CopyTo(trackTagValues, 0);

            for (int i = 0; i < trackTagValues.Length; i++)
            {
                List<string> next = trackTagValues[i];

                if (next.Remove(idOld))
                {
                    next.Add(track.guid);
                }
            }
            
        }
        else
        {
            assetDict[track.guid] = track;
        }


        return true;
    }

    public void TagTrack(string guid, string tag)
    { 
       if(trackTagDict.TryGetValue(guid, out var tags))
        {
            if (!tags.Contains(tag))
            {
                tags.Add(tag);
                tags.Sort();
            }
        }
        else
        {
            tags = new List<string>();
            tags.Add(tag);
            trackTagDict.Add(guid, tags);
        }
    }
    public void TagTrack(Track track, string tag) => TagTrack(track.guid, tag);

    public bool UntagTrack(string guid, string tag)
    {
        if (trackTagDict.TryGetValue(guid, out var tagList))
        {
            return tagList.Remove(tag);
        }
        return false;
    }

    public bool GetRandomTrack(string tag, out Track track)
    {
        track = new Track();
        List<string> tracksAsTagged = new List<string>();

        foreach(var keyValuePair in trackTagDict)
        {
            if (keyValuePair.Value.Contains(tag))
            {
                tracksAsTagged.Add(keyValuePair.Key);
            }
        }

        if (tracksAsTagged.Count < 1)
        {
            return false;
        }

        string trackId = tracksAsTagged[Random.Range(0, tracksAsTagged.Count - 1)];
        track = assetDict[trackId];
        return true;
    }

    public string[] GetTags(string trackGuid)
    {
        if (trackTagDict.TryGetValue(trackGuid, out var tags))
        {
            return tags.ToArray();
        }
        else
        {
            return null;
        }
    }

    public string[] GetAllTags()
    {
        List<string> tags = new List<string>();

        foreach (var kvp in trackTagDict)
        {
            List<string> tagSet = kvp.Value;

            for (int i = 0; i < tagSet.Count; i++)
            {
                string next = tagSet[i];
                if (!tags.Contains(next))
                {
                    tags.Add(next);
                }
            }
        }

        return tags.ToArray();
    }

    public void SaveAll(string path)
    {
        TagData fileData;
        fileData.taggedTracks = new TaggedTrack[trackTagDict.Count];

        int i = 0;
        foreach (var kvp in trackTagDict)
        {
            TaggedTrack next;
            next.tags = kvp.Value.ToArray();
            next.track = assetDict[kvp.Key];
            fileData.taggedTracks[i] = next;
            i++;   
        }

        try
        {
            File.WriteAllText(path, JsonUtility.ToJson(fileData));
            Debug.Log("Audiomata:Saved Sucessfully");
        }
        catch (IOException e)
        {
            Debug.Log("Audiomata:Failed to save - "+e.Message);
        }
    }

    public void Load(string path, bool clearAll = true)
    {
        if (clearAll)
        {
            trackTagDict.Clear();
            assetDict.Clear();
            GetAudioAssets(true);
        }

        string fileData;
        try
        {
            fileData = File.ReadAllText(path);
            
        }
        catch (IOException e)
        {
            Debug.Log("Audiomata: Failed to load Tag file: " + e.Message);
            return;
        }

        TaggedTrack[] trackTagArr = JsonUtility.FromJson<TagData>(fileData).taggedTracks;

        for (int i = 0; i < trackTagArr.Length; i++)
        {
            TaggedTrack next = trackTagArr[i];

            if (!assetDict.ContainsKey(next.track.guid))
            {
                Debug.LogError("Audiomata: Asset Not Found: " + next.track.path);
                continue;
            }

            List<string> tags = new List<string>();

            tags.AddRange(next.tags);

            trackTagDict.Add(next.track.guid, tags);
            
        }

        Debug.Log("Audiomata Loaded Sucessfully");
    }

    public Track[] GetTracksTaggedAs(string tag)
    {
        List<Track> tracks = new List<Track>();

        foreach (var kvp in trackTagDict)
        {
            if (kvp.Value.Contains(tag))
            {
                tracks.Add(assetDict[kvp.Key]);
            }
        }
        return tracks.ToArray();
    }
}

[System.Serializable]
public struct TagData
{
    public TaggedTrack[] taggedTracks;
}

//TODO move this to it's own file
[System.Serializable]
public struct Track
{
    public string name;
    public string path;
    public string guid;

    public override bool Equals(object obj)
    {
        if(obj.GetType() != typeof(Track))
        {
            return false;
        }
        Track t = (Track)obj;

        return (t.name == name && t.path == path && guid == t.guid);
    }
}

[System.Serializable]
public struct TaggedTrack
{
    public Track track;
    public string[] tags;
}

