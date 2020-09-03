using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audiomata;
using UnityStandardAssets.Characters.FirstPerson;

public class GetFootStepSounds : MonoBehaviour
{
    [SerializeField]
    private string query = "";
    private QueryManager qm;

    private void Start()
    {
        qm = AudioManager.Instance.QueryManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        FirstPersonController firstPerson = other.GetComponent<FirstPersonController>();
        qm.QueryAudio(query, out var result);

        firstPerson.UpdateFootStepSounds(result);

    }

    
}
