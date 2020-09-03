using UnityEngine;
using UnityEngine.Audio;
using Audiomata;
using Audiomata.ComponentTrackers;

public class FilterToggler : MonoBehaviour
{
    private AudioEchoFilter echoFilter;
    private AudioEchoFilterCommander echolFilterCmder;

    private AudioLowPassFilter lowPassFilter;
    private AudioLowPassFilterCommander lowPassFilterCmder;

    private AudioEffectManager afxManager;

    // Start is called before the first frame update
    void Start()
    {
        afxManager = AudioManager.Instance.EffectManager;
        echoFilter = GetComponent<AudioEchoFilter>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();
        

       echolFilterCmder = afxManager.StartTrackingEchoFilter(echoFilter);
        lowPassFilterCmder =  afxManager.StartTrackingLowPass(lowPassFilter);
     

        ///afxManager is only meant to store active commanders, it is not intended for management as such as this is normally done in a localised fashion anyway
        //afxManager.DoCommand<>
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            echolFilterCmder.DoCommand<bool>(!echoFilter.enabled, (int)AudioEchoFilterMembers.enabled);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            lowPassFilterCmder.DoCommand<bool>(!lowPassFilter.enabled, (int)AudioLowPassFilterMembers.enabled);
        }



    }
}
