using System;
using UnityEngine;

/// <summary>
/// Class that manages the creation of Audio Events
/// </summary>
public class AudioEventCreator 
{
    TrackTagger activeTrackTagger;

    public AudioEventCreator(TrackTagger activeTagger)
    {
        activeTrackTagger = activeTagger ?? throw new Exception("TrackTagger cannot be null");
    }

   
}
