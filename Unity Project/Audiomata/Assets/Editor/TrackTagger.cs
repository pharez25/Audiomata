using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Class manages the tagging of Audio Clips
/// </summary>
class TrackTagger
{
    /// <summary>
    /// Dictionary of all tags against tracks
    /// </summary>
    Dictionary<string, List<string>> taggedTracks;

    /// <summary>
    /// clip name to asset ID
    /// </summary>
    Dictionary<string, string> cliptoGUID;

    public TrackTagger()
    {
        taggedTracks = new Dictionary<string, List<string>>();
        cliptoGUID = new Dictionary<string, string>();
        GetAudioAssets();
    }

    /// <summary>
    /// Gets all Audio Clips from within the project updates dictionary of tracks where appropriate
    /// </summary>
    /// <returns>an array of all audio clip names from within the project </returns>
    public  string[] GetAudioAssets()
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
            if (!cliptoGUID.ContainsKey(nextClip))
            {
                cliptoGUID.Add(nextClip, foundAssets[i]);
            }
            
        }
        return trackNames;
    }

    public string[] GetActiveTrackNames()
    {
        string[] tracks = new string[cliptoGUID.Count];
        cliptoGUID.Keys.CopyTo(tracks, 0);
        return tracks;
    }

    public void TagTrack(string tag, string trackName)
    {
        if (taggedTracks.TryGetValue(tag, out var trackList))
        {
            if (!trackList.Contains(trackName))
            {
                trackList.Add(trackName);
                trackList.Sort();
            }
        }
        else
        {
            trackList = new List<string>();
            trackList.Add(trackName);
            taggedTracks.Add(tag, trackList);
        }
    }

    public bool UntagTrack(string tag, string trackName)
    {
        if(taggedTracks.TryGetValue(tag, out var trackList))
        {
           return trackList.Remove(trackName);
        }

        return false;
    }

   public bool GetTrack(string tag, out string trackName)
    {
        if(!taggedTracks.TryGetValue(tag, out var tracks))
        {
            trackName = null;
            return false;
        }
        if (tracks.Count > 0)
        {
            trackName = tracks[Random.Range(0, tracks.Count - 1)];
            return true;
        }
        else
        {
            trackName = null;
            return false;
        }
        
        
    }

    public string[] GetTags(string trackName)
    {
        List<string> trackTags = new List<string>();

        foreach(var entry in taggedTracks)
        {
            if (entry.Value.Contains(trackName))
            {
                trackTags.Add(entry.Key);
            }
        }
        trackTags.Sort();
        return trackTags.ToArray();
    }

    public void AddTag(string tag)
    {
        if (!taggedTracks.ContainsKey(tag))
        {
            taggedTracks.Add(tag, new List<string>());
        }
    }

    public void RemoveTag(string tag)
    {
        taggedTracks.Remove(tag);
    }

   public string[] GetAllTags()
    {
        string[] tags = new string[taggedTracks.Count];
        taggedTracks.Keys.CopyTo(tags, 0);

        return tags;
    }

    public void SaveAll(string path)
    {
        List<TaggedTrack> trackData  = new List<TaggedTrack>();

        foreach (var clip in cliptoGUID)
        {
            trackData.Add(new TaggedTrack() { name = clip.Key,tags= new List<string>() });
        }

        foreach(var entry in taggedTracks)
        {
            for (int i = 0; i < trackData.Count; i++)
            {
                if (entry.Value.Contains(trackData[i].name))
                {
                    trackData[i].tags.Add(entry.Key);
                }
            }
        }

        try
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(trackData));
        }
        catch (IOException e)
        {
            Debug.Log("Audiomata: Failed To Save Tags: "+e.Message);
        }
    }

    public void Load(string path, bool clearAll = true)
    {
        if (clearAll)
        {
            taggedTracks.Clear();
        }
        string data;
        try
        {
             data = File.ReadAllText(path);
        }
        catch(IOException e)
        {
            Debug.Log("Audiomata: Failed to load Tag file: " + e.Message);
            return;
        }
        

       JArray arr = JArray.Parse(data);
        for (int i = 0; i < arr.Count; i++)
        {
            TaggedTrack next = JsonConvert.DeserializeObject<TaggedTrack>(arr[i].ToString());


            for (int j = 0; j < next.tags.Count; j++)
            {
                string nextTag = next.tags[j];
                if (!taggedTracks.ContainsKey(nextTag))
                {
                    List<string> trackNames = new List<string>();
                    trackNames.Add(next.name);
                    taggedTracks.Add(nextTag, trackNames);
                }
                else
                {
                    taggedTracks[nextTag].Add(next.name);
                }
                
            }
        }
    }
 }

public struct TaggedTrack
{
   public string name;
    public List<string> tags;
}

