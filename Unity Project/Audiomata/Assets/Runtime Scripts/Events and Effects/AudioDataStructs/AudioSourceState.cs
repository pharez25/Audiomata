using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


namespace Audiomata.ComponentTrackers
{
    public class AudioSourceState : IAudioCommandable
    {
        Stack<AudioCommand<AudioSource>> audioCommands;
        public AudioSource Target { get; private set; }

        //code generations was being looked into but, there was not enough time to risk it

        public void Play() => Target.Play();

        public void PlayDelayed(float delay) => Target.PlayDelayed(delay);

        public void Pause() => Target.Pause();
        public void UnPause() => Target.UnPause();

        public AudioSourceState(AudioSource source) 
        {
            audioCommands = new Stack<AudioCommand<AudioSource>>();
            Target = source;
        }

        public void ExecuteCommand<T>(AudioSourceProps targetProp, T targetValue, float t = -1) 
        {
            Type propType = targetValue.GetType();

            if (propType == typeof(AudioClip))
            {
                
            }
            else if (propType == typeof(bool))
            {
                switch (targetProp)
                {
                    case AudioSourceProps.bypassEffects:
                        AudioCommand<AudioSource> setBypassEffectsCmd = new SetBypassEffects(Target);
                        setBypassEffectsCmd.Do(targetValue);
                        audioCommands.Push(setBypassEffectsCmd);
                        
                        break;

                    case AudioSourceProps.bypassListenerEffects:
                        break;

                    case AudioSourceProps.bypassReverbZones:
                        break;

                    case AudioSourceProps.ignoreListenerPause:
                        break;
                    case AudioSourceProps.ignoreListenerVolume:
                        break;

                    case AudioSourceProps.loop:
                        break;

                    case AudioSourceProps.mute:
                        break;

                    case AudioSourceProps.spatialize:
                        break;

                    case AudioSourceProps.spatializePostEffects:
                        break;
                  
                }
            }
            else if (propType == typeof(float))
            {

            }
            else if (propType == typeof(float))
            {

            }
            else if (propType == typeof(int))
            {

            }

        }

        public void ClearChangeHistory() 
        {
            throw new System.NotImplementedException();
        }

        public void RedoLast() 
        {
            throw new System.NotImplementedException();
        }

        public void SetPropValue<T>(T newValue, int enumeratedProp) 
        {
            AudioSourceProps prop = (AudioSourceProps)enumeratedProp;
            Type propType = newValue.GetType();
        }

        public void UndoAll( )
        {
            while (audioCommands.Count > 0)
            {
                AudioCommand<AudioSource> cmd = audioCommands.Pop();

                cmd.Undo();
            }
        }

        public void UndoLast() 
        {
            if (audioCommands.Count > 0)
            {
                AudioCommand<AudioSource> cmd = audioCommands.Pop();
                cmd.Undo();
               
            }
        }

    }
    /// <summary>
    /// Properties that can be accesed and changed by Audiomata within AudioSource Components
    /// </summary>
    /// <remarks>Matches the properties exactly in case of external use of System.Reflection</remarks>
    public enum AudioSourceProps
    {
        bypassEffects, bypassListenerEffects, bypassReverbZones, ignoreListenerPause, ignoreListenerVolume,                 
        loop, mute, spatialize, clip, dopplerLevel, maxDistance, minDistance, panStereo, pitch, reverbZoneMix, rolloffMode, spatialBlend, spatializePostEffects,
        spread, time, timeSamples, volume, priority, velocityUpdateMode, outputAudioMixerGroup
    }

    //Below is a lot of copy-paste, would not recommend reading all of it.
    //Automatic code generation was considered, however there weas not enough time
    #region Commands
    #region Commands.Bools
    public class SetBypassEffects : AudioCommand<AudioSource>, IAudioCommand<bool>
    {
        public bool InitialValue { get; private set; }
        public bool FinalValue { get; private set; }
        public CommandState CommandState { get; set; }
        public SetBypassEffects(AudioSource target)
        {
            InitialValue = target.bypassEffects;
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.bypassEffects;
            Target.bypassEffects = (bool)newValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.bypassEffects;
            Target.bypassEffects = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.bypassEffects;
        }

        public override void Undo()
        {
            Target.bypassEffects = FinalValue;

        }
    }

    public class SetByPassListenerEffects : AudioCommand<AudioSource>, IAudioCommand<bool>
    {
        public bool InitialValue { get; private set; }
        public bool FinalValue { get; private set; }
        public CommandState CommandState { get; set; }

        public SetByPassListenerEffects(AudioSource target)
        {
            InitialValue = target.bypassListenerEffects;
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.bypassListenerEffects;
            Target.bypassListenerEffects = (bool)newValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.bypassListenerEffects;
            Target.bypassListenerEffects = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.bypassListenerEffects;
        }

        public override void Undo()
        {
            Target.bypassListenerEffects = InitialValue;

        }
    }

    public class SetBypassReverbZones : AudioCommand<AudioSource>, IAudioCommand<bool>
    {
        public bool InitialValue { get; private set; }
        public bool FinalValue { get; private set; }
        public CommandState CommandState { get; set; }

        public SetBypassReverbZones(AudioSource target)
        {
            InitialValue = target.bypassReverbZones;
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.bypassReverbZones;
            Target.bypassReverbZones = (bool)newValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.bypassReverbZones;
            Target.bypassReverbZones = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.bypassReverbZones;
        }

        public override void Undo()
        {
            Target.bypassReverbZones = InitialValue;

        }
    }

    public class SetIgnoreListenerPause : AudioCommand<AudioSource>, IAudioCommand<bool>
    {
        public bool InitialValue { get; private set; }
        public bool FinalValue { get; private set; }
        public CommandState CommandState { get; set; }

        public SetIgnoreListenerPause(AudioSource target)
        {
            InitialValue = target.ignoreListenerPause;
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.ignoreListenerPause;
            Target.ignoreListenerPause = (bool)newValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.ignoreListenerPause;
            Target.ignoreListenerPause = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.ignoreListenerPause;
        }

        public override void Undo()
        {
            Target.ignoreListenerPause = InitialValue;

        }
    }

    public class SetIgnoreListenerVolume: AudioCommand<AudioSource>, IAudioCommand<bool>
    {
        public bool InitialValue { get; private set; }
        public bool FinalValue { get; private set; }
        public CommandState CommandState { get; set; }

        public SetIgnoreListenerVolume(AudioSource target)
        {
            InitialValue = target.ignoreListenerVolume;
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.ignoreListenerVolume;
            Target.ignoreListenerVolume = (bool)newValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.ignoreListenerVolume;
            Target.ignoreListenerVolume = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.ignoreListenerVolume;
        }

        public override void Undo()
        {
            Target.ignoreListenerVolume = InitialValue;

        }
    }

    public class SetLoop: AudioCommand<AudioSource>, IAudioCommand<bool>
    {
        public bool InitialValue { get; private set; }
        public bool FinalValue { get; private set; }
        public CommandState CommandState { get; set; }

        public SetLoop(AudioSource target)
        {
            InitialValue = target.loop;
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.loop;
            Target.loop = (bool)newValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.loop;
            Target.loop = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.loop;
        }

        public override void Undo()
        {
            Target.loop = InitialValue;

        }
    }

    public class SetMute: AudioCommand<AudioSource>, IAudioCommand<bool>
    {
        public bool InitialValue { get; private set; }
        public bool FinalValue { get; private set; }
        public CommandState CommandState { get; set; }

        public SetMute(AudioSource target)
        {
            InitialValue = target.mute;
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.mute;
            Target.mute = (bool)newValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.mute;
            Target.mute = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.loop;
        }

        public override void Undo()
        {
            Target.mute = InitialValue;

        }
    }

    public class SetSpatialize : AudioCommand<AudioSource>, IAudioCommand<bool>
    {
        public bool InitialValue { get; private set; }
        public bool FinalValue { get; private set; }
        public CommandState CommandState { get; set; }

        public SetSpatialize(AudioSource target)
        {
            InitialValue = target.spatialize;
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.spatialize;
            Target.mute = (bool)newValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.spatialize;
            Target.mute = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.spatialize;
        }

        public override void Undo()
        {
            Target.mute = InitialValue;

        }
    }

    public class SetSpatializePostEffects : AudioCommand<AudioSource>, IAudioCommand<bool>
    {
        public bool InitialValue { get; private set; }
        public bool FinalValue { get; private set; }
        public CommandState CommandState { get; set; }

        public SetSpatializePostEffects(AudioSource target)
        {
            InitialValue = target.spatializePostEffects;
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.spatializePostEffects;
            Target.mute = (bool)newValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.spatializePostEffects;
            Target.mute = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.spatializePostEffects;
        }

        public override void Undo()
        {
            Target.mute = InitialValue;

        }
    }

    #endregion

    #region Commands.Floats
    public class SetTime: AudioCommand<AudioSource>, IAudioCommand<float>
    {
        public float InitialValue { get; private set; }

        public float FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public SetTime(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            float volumeValue = (float)newValue;
            InitialValue = Target.time;
            FinalValue = volumeValue;
            Target.time = volumeValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.time;
            FinalValue = newValue;
            Target.time = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.volume;
        }

        public override void Step(float t = 1)
        {
            float diff = FinalValue - InitialValue;
            Target.time = InitialValue + (t * diff);
        }

        public override void Undo()
        {
            Target.time = InitialValue;
        }
    }

    public class SetVolume : AudioCommand<AudioSource>, IAudioCommand<float>
    {
        public float InitialValue { get; private set; }

        public float FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public SetVolume(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            float volumeValue = (float)newValue;
            InitialValue = Target.volume;
            FinalValue = volumeValue;
            Target.volume = volumeValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.volume;
            FinalValue = newValue;
            Target.volume = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.volume;
        }

        public override void Step(float t = 1)
        {
            float diff = FinalValue - InitialValue;
            Target.volume = InitialValue + (t * diff);
        }

        public override void Undo()
        {
            Target.volume = InitialValue;
        }
    }

    public class SetDopplerLevel : AudioCommand<AudioSource>, IAudioCommand<float>
    {
        public float InitialValue { get; private set; }

        public float FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public SetDopplerLevel(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            float volumeValue = (float)newValue;
            InitialValue = Target.dopplerLevel;
            FinalValue = volumeValue;
            Target.dopplerLevel = volumeValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.dopplerLevel;
            FinalValue = newValue;
            Target.dopplerLevel = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.volume;
        }

        public override void Step(float t = 1)
        {
            float diff = FinalValue - InitialValue;
            Target.dopplerLevel = InitialValue + (t * diff);
        }

        public override void Undo()
        {
            Target.dopplerLevel = InitialValue;
        }
    }
    
    public class SetMaxDistance : AudioCommand<AudioSource>, IAudioCommand<float>
    {
        public float InitialValue { get; private set; }

        public float FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public SetMaxDistance(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            float volumeValue = (float)newValue;
            InitialValue = Target.maxDistance;
            FinalValue = volumeValue;
            Target.maxDistance = volumeValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.maxDistance;
            FinalValue = newValue;
            Target.maxDistance = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.volume;
        }

        public override void Step(float t = 1)
        {
            float diff = FinalValue - InitialValue;
            Target.maxDistance = InitialValue + (t * diff);
        }

        public override void Undo()
        {
            Target.maxDistance = InitialValue;
        }
    }

    public class SetMinDistance : AudioCommand<AudioSource>, IAudioCommand<float>
    {
        public float InitialValue { get; private set; }

        public float FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public SetMinDistance(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            float volumeValue = (float)newValue;
            InitialValue = Target.minDistance;
            FinalValue = volumeValue;
            Target.minDistance = volumeValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.minDistance;
            FinalValue = newValue;
            Target.minDistance = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.volume;
        }

        public override void Step(float t = 1)
        {
            float diff = FinalValue - InitialValue;
            Target.minDistance = InitialValue + (t * diff);
        }

        public override void Undo()
        {
            Target.minDistance
 = InitialValue;
        }
    }

    public class SetPanStereo : AudioCommand<AudioSource>, IAudioCommand<float>
    {
        public float InitialValue { get; private set; }

        public float FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public SetPanStereo(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            float volumeValue = (float)newValue;
            InitialValue = Target.panStereo;
            FinalValue = volumeValue;
            Target.panStereo = volumeValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.panStereo;
            FinalValue = newValue;
            Target.panStereo = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.volume;
        }

        public override void Step(float t = 1)
        {
            float diff = FinalValue - InitialValue;
            Target.panStereo = InitialValue + (t * diff);
        }

        public override void Undo()
        {
            Target.panStereo = InitialValue;
        }
    }

    public class SetPitch : AudioCommand<AudioSource>, IAudioCommand<float>
    {
        public float InitialValue { get; private set; }

        public float FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public SetPitch(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            float volumeValue = (float)newValue;
            InitialValue = Target.pitch;
            FinalValue = volumeValue;
            Target.pitch = volumeValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.pitch;
            FinalValue = newValue;
            Target.pitch = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.volume;
        }

        public override void Step(float t = 1)
        {
            float diff = FinalValue - InitialValue;
            Target.pitch = InitialValue + (t * diff);
        }

        public override void Undo()
        {
            Target.pitch = InitialValue;
        }
    }

    public class SetReverbZoneMix : AudioCommand<AudioSource>, IAudioCommand<float>
    {
        public float InitialValue { get; private set; }

        public float FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public SetReverbZoneMix(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            float volumeValue = (float)newValue;
            InitialValue = Target.reverbZoneMix;
            FinalValue = volumeValue;
            Target.reverbZoneMix = volumeValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.reverbZoneMix;
            FinalValue = newValue;
            Target.reverbZoneMix = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.reverbZoneMix;
        }

        public override void Step(float t = 1)
        {
            float diff = FinalValue - InitialValue;
            Target.reverbZoneMix = InitialValue + (t * diff);
        }

        public override void Undo()
        {
            Target.reverbZoneMix = InitialValue;
        }
    }

    public class SetSpatialBlend: AudioCommand<AudioSource>, IAudioCommand<float>
    {
        public float InitialValue { get; private set; }

        public float FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public SetSpatialBlend(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            float spatilBlendVal = (float)newValue;
            InitialValue = Target.spatialBlend;
            FinalValue = spatilBlendVal;
            Target.spatialBlend = spatilBlendVal;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.spatialBlend;
            FinalValue = newValue;
            Target.spatialBlend = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.spatialBlend;
        }

        public override void Step(float t = 1)
        {
            float diff = FinalValue - InitialValue;
            Target.spatialBlend = InitialValue + (t * diff);
        }

        public override void Undo()
        {
            Target.spatialBlend = InitialValue;
        }
    }

    public class SetSpread: AudioCommand<AudioSource>, IAudioCommand<float>
    {
        public float InitialValue { get; private set; }

        public float FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public SetSpread(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            float spatialValue = (float)newValue;
            InitialValue = Target.spread;
            FinalValue = spatialValue;
            Target.spread = spatialValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.spread;
            FinalValue = newValue;
            Target.spread = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.spread;
        }

        public override void Step(float t = 1)
        {
            float diff = FinalValue - InitialValue;
            Target.spread = InitialValue + (t * diff);
        }

        public override void Undo()
        {
            Target.spread = InitialValue;
        }
    }

    #endregion

    #region Commands.Ints

    public class SetTimeSamples : AudioCommand<AudioSource>, IAudioCommand<int>
    {
        public int InitialValue { get; private set; }

        public int FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public SetTimeSamples(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            int timeSamples = (int)newValue;
            InitialValue = Target.timeSamples;
            FinalValue = timeSamples;
            Target.timeSamples = timeSamples;
        }

        public void Do(int newValue)
        {
            InitialValue = Target.timeSamples;
            FinalValue = newValue;
            Target.timeSamples = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.spread;
        }

        public override void Step(float t = 1)
        {
            int diff = FinalValue - InitialValue;
            Target.timeSamples = (int)Math.Round(InitialValue + (t * diff));
        }

        public override void Undo()
        {
            Target.spread = InitialValue;
        }
    }

    public class SetPriority: AudioCommand<AudioSource>, IAudioCommand<int>
    {
        public int InitialValue { get; private set; }

        public int FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public SetPriority(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            int priority = (int)newValue;
            InitialValue = Target.priority;
            FinalValue = priority;
            Target.priority = priority;
        }

        public void Do(int newValue)
        {
            InitialValue = Target.priority;
            FinalValue = newValue;
            Target.priority = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.spread;
        }

        public override void Step(float t = 1)
        {
            int diff = FinalValue - InitialValue;
            Target.priority = (int)Math.Round(InitialValue + (t * diff));
        }

        public override void Undo()
        {
            Target.priority = InitialValue;
        }
    }

    #endregion

    #region Commands.Other

    public class SetClip : AudioCommand<AudioSource>, IAudioCommand<AudioClip>
    {
        public AudioClip InitialValue { get; private set; }

        public AudioClip FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public void Do(AudioClip newValue)
        {
            InitialValue = Target.clip;
            FinalValue = newValue;
            Target.clip= newValue;
        }

        public override void Do(object newValue)
        {
            AudioClip clip = (AudioClip)newValue;
            InitialValue = Target.clip;
            FinalValue = clip;
            Target.clip = clip;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.clip;
        }

        public override void Undo()
        {
            Target.clip = InitialValue;
        }
    }

    public class SetRollOffMode : AudioCommand<AudioSource>, IAudioCommand<AudioRolloffMode>
    {
        public AudioRolloffMode InitialValue { get; private set; }

        public AudioRolloffMode FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public SetRollOffMode(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            AudioRolloffMode rollOfVal = (AudioRolloffMode)newValue;
            InitialValue = Target.rolloffMode;
            FinalValue = rollOfVal;
            Target.rolloffMode = rollOfVal;
        }

        public void Do(AudioRolloffMode newValue)
        {
            InitialValue = Target.rolloffMode;
            FinalValue = newValue;
            Target.rolloffMode = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.reverbZoneMix;
        }

        public override void Undo()
        {
            Target.rolloffMode = InitialValue;
        }
    }

    public class SetVelocityUpdateMode : AudioCommand<AudioSource>, IAudioCommand<AudioVelocityUpdateMode>
    {
        public AudioVelocityUpdateMode InitialValue { get; private set; }

        public AudioVelocityUpdateMode FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public SetVelocityUpdateMode(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            AudioVelocityUpdateMode updateMode = (AudioVelocityUpdateMode)newValue;
            InitialValue = Target.velocityUpdateMode;
            FinalValue = updateMode;
            Target.velocityUpdateMode = updateMode;
        }

        public void Do(AudioVelocityUpdateMode newValue)
        {
            InitialValue = Target.velocityUpdateMode;
            FinalValue = newValue;
            Target.velocityUpdateMode = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.velocityUpdateMode;
        }

        public override void Undo()
        {
            Target.velocityUpdateMode = InitialValue;
        }
    }

    public class SetOuputAudioMixerGroup : AudioCommand<AudioSource>, IAudioCommand<AudioMixerGroup>
    {
        public AudioMixerGroup InitialValue { get; private set; }

        public AudioMixerGroup FinalValue { get; private set; }

        public CommandState CommandState { get; set; }

        public override void Do(object newValue)
        {
            InitialValue = Target.outputAudioMixerGroup;
            Target.outputAudioMixerGroup = (AudioMixerGroup)newValue;
            FinalValue = Target.outputAudioMixerGroup;
        }

        public void Do(AudioMixerGroup newValue)
        {
            InitialValue = Target.outputAudioMixerGroup;
            FinalValue = Target.outputAudioMixerGroup;
            Target.outputAudioMixerGroup = newValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceProps.outputAudioMixerGroup;
        }

        public override void Undo()
        {
            Target.outputAudioMixerGroup = InitialValue;
        }
    }

    #endregion


    #endregion
}
