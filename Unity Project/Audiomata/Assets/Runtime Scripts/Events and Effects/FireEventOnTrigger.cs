using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audiomata;


public class FireEventOnTrigger : MonoBehaviour
{
    EventManager eventManager;

    [SerializeField]
    private string eventName = "fourSeasonsDemo";

    [SerializeField]
    private AudioBehaviour[] relevantComps;

    void Start()
    {
        eventManager = AudioManager.Instance.EventManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player")
        {
            return;
        }

        eventManager.FireEvent(eventName, this,  this);
    }

    public void EnableRelevantComps()
    {
        for (int i = 0; i < relevantComps.Length; i++)
        {
            relevantComps[i].enabled = true;
        }
    }
}
