using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SourceEvent : IAudioEvent
{
    private AudioSource targetSource;
    public string Id { get; private set; }
    private object startValue;

    private ModifySourceProp modifierFunction;


    private delegate void ModifySourceProp(object newValue,float t);

   
    private void Test(ref float testy)
    {
        testy = 1;
    }

    public SourceEvent(string id, AudioSource target,string propName)
    {
        targetSource = target;
        
        Id = id;

        switch (propName)
        {
            default:
                break;
        }

    }

    public void Apply()
    {
        
    }

    public void Apply(float t)
    {
        
    }

    public void Remove()
    {
        
    }

    public void Remove(float t)
    {
        
    }
    public UnityEngine.Object Target => targetSource;
    public float FadeIn { get; set; }
    public float FadeOut { get; set; }
    public AudioEventStatus Status { get; set; }

    #region Property Delegates

    
    

    #endregion
}



