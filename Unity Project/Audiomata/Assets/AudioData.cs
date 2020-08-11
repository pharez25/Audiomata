using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Audiomata
{
    [System.Serializable]
    public class AudioData : ScriptableObject
    {
        public string guid;
        public AudioClip clip;
        public List<string> tags;
    }

}