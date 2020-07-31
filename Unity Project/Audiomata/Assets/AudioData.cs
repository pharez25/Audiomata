using System.Collections.Generic;
using UnityEngine;

public class AudioData : ScriptableObject
{
    public string guid;
    public AudioClip clip;
    public List<string> tags;
}
