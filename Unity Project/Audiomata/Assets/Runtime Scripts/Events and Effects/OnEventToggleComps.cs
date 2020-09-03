
using Audiomata;
using UnityEngine;
using UnityEngine.Audio;

public class OnEventToggleComps : MonoBehaviour
{
    [SerializeField]
    private AudioBehaviour[] targets;
    
    
    private EventManager eventManager;
    [SerializeField]
    string eventId = "fourSeasonsDemo";

    void Start()
    {
        eventManager = AudioManager.Instance.EventManager;
        eventManager.AddEventReference(eventId, new Audiomata.Event(OnEnableEvent));
    }

   void OnEnableEvent(object sender, Object t)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            AudioBehaviour next = targets[i];
            next.enabled = false;
        }
        FireEventOnTrigger targetRegion = (FireEventOnTrigger)t;
        targetRegion.EnableRelevantComps();

    }

}
