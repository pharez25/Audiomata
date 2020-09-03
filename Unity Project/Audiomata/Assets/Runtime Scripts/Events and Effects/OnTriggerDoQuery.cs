using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audiomata;



public class OnTriggerDoQuery : MonoBehaviour
{
    [SerializeField]
    string query = "";
    [SerializeField]
    string targetTag = "Player";
    [SerializeField]
    AudioSource targetSrc;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != targetTag)
        {
            return;
        }
        targetSrc.Stop();

        targetSrc.clip = AudioManager.Instance.QueryManager.QueryAudio(query);
        if (!targetSrc.clip)
        {
            Debug.LogWarning("There was no clip found from query: " + query);
            return;
        }
        targetSrc.Play();
    }
}
