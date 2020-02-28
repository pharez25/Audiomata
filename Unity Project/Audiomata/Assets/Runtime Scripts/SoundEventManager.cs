using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoundEventManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] allAudioClips;

    Dictionary<string, SoundEvent> eventDictionary;

    

    //TEST ME
    public void GetAllAudioClips()
    {
        if (!Application.isEditor)
        {
            Debug.LogError("Audiomata: Cannot Get Audio Clips in Game Build");
            return;
        }

        string[] audioAssets = AssetDatabase.FindAssets("t:AudioClip");
        allAudioClips = new AudioClip[audioAssets.Length];

        for (int i = 0; i < audioAssets.Length; i++)
        {
            string current = audioAssets[i];
            string assetPath = AssetDatabase.GUIDToAssetPath(current);
            AudioClip currentClip =(AudioClip) AssetDatabase.LoadAssetAtPath(assetPath, typeof(AudioClip));
            allAudioClips[i] = currentClip;
        }
    }
    
}

public delegate void SoundEvent();