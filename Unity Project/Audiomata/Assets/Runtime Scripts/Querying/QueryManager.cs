using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class QueryManager
{
    private Dictionary<string, string[]> tagsToTracks;
    private Dictionary<string, AudioClip> idToClip;

    public AudioClip GetTrackById(string id) => idToClip[id];
}
