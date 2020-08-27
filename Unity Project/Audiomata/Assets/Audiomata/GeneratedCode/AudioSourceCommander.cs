

namespace Audiomata.ComponentTrackers
{
    using Audiomata;
    //-------------------------------------------------------------------------------------------------------------------------------
    //					THIS CODE IS AUTOMATICALLY GENERATED, DO NOT MODIFY
    //-------------------------------------------------------------------------------------------------------------------------------
    using UnityEngine;
    using UnityEngine.Audio;

    // Auto generated props, can use reflections but don't because it is REALLY slow
    #region EnumeratedProps
    public enum AudioSourceMembers
    {
        enabled
        , volume
            , pitch
            , time
            , timeSamples
            , clip
            , outputAudioMixerGroup
            , loop
            , ignoreListenerVolume
            , playOnAwake
            , ignoreListenerPause
            , velocityUpdateMode
            , panStereo
            , spatialBlend
            , spatialize
            , spatializePostEffects
            , reverbZoneMix
            , bypassEffects
            , bypassListenerEffects
            , bypassReverbZones
            , dopplerLevel
            , spread
            , priority
            , mute
            , minDistance
            , maxDistance
            , rolloffMode

    }
    #endregion

    #region CommmandManager
    //Class to manage state of components
    public class AudioSourceCommander : IAudioCommandable
    {
        private LimitedStack<AudioCommand<AudioSource>> audioCommands;
        public AudioSource Target { get; private set; }

        public AudioSourceCommander(AudioSource target)
        {
            audioCommands = new LimitedStack<AudioCommand<AudioSource>>();
            Target = target;
        }

        public AudioCommand<AudioSource> CreateCommand(int enumeratedProp)
        {
            AudioCommand<AudioSource> newCommand = CommandFactory(enumeratedProp);
            audioCommands.Push(newCommand);
            return newCommand;
        }


        public object DoCommand<T>(T value, int enumeratedProp)
        {
            AudioCommand<AudioSource> newCommand = CommandFactory(enumeratedProp);
            newCommand.Do(value);
            audioCommands.Push(newCommand);
            return newCommand;
        }

        public object RegisterCommand(int enumeratedProp)
        {
            AudioCommand<AudioSource> newCommand = CommandFactory(enumeratedProp);
            audioCommands.Push(newCommand);
            return newCommand;
        }

        public void UndoCommand(object cmd)
        {
            AudioCommand<AudioSource> command = (AudioCommand<AudioSource>)cmd;
            command.Undo();
            audioCommands.Remove(command);
        }


        public void UndoLast()
        {
            if (audioCommands.Count < 1)
            {
                return;
            }

            AudioCommand<AudioSource> popped = audioCommands.Pop();
            popped.Undo();
        }

        public void UndoAll()
        {
            while (audioCommands.Count > 0)
            {
                AudioCommand<AudioSource> popped = audioCommands.Pop();
                popped.Undo();
            }
        }

        public void ClearChangeHistory()
        {
            while (audioCommands.Count > 0)
            {
                AudioCommand<AudioSource> popped = audioCommands.Pop();
            }
        }

        private AudioCommand<AudioSource> CommandFactory(int enumeratedProp)
        {
            AudioSourceMembers commandPropTarget = (AudioSourceMembers)enumeratedProp;
            AudioCommand<AudioSource> nextCommand;
            switch (commandPropTarget)
            {
                case AudioSourceMembers.volume:
                    nextCommand = new AudioSourceCmdVolume(Target);
                    return nextCommand;
                case AudioSourceMembers.pitch:
                    nextCommand = new AudioSourceCmdPitch(Target);
                    return nextCommand;
                case AudioSourceMembers.time:
                    nextCommand = new AudioSourceCmdTime(Target);
                    return nextCommand;
                case AudioSourceMembers.timeSamples:
                    nextCommand = new AudioSourceCmdTimeSamples(Target);
                    return nextCommand;
                case AudioSourceMembers.clip:
                    nextCommand = new AudioSourceCmdClip(Target);
                    return nextCommand;
                case AudioSourceMembers.outputAudioMixerGroup:
                    nextCommand = new AudioSourceCmdOutputAudioMixerGroup(Target);
                    return nextCommand;
                case AudioSourceMembers.loop:
                    nextCommand = new AudioSourceCmdLoop(Target);
                    return nextCommand;
                case AudioSourceMembers.ignoreListenerVolume:
                    nextCommand = new AudioSourceCmdIgnoreListenerVolume(Target);
                    return nextCommand;
                case AudioSourceMembers.playOnAwake:
                    nextCommand = new AudioSourceCmdPlayOnAwake(Target);
                    return nextCommand;
                case AudioSourceMembers.ignoreListenerPause:
                    nextCommand = new AudioSourceCmdIgnoreListenerPause(Target);
                    return nextCommand;
                case AudioSourceMembers.velocityUpdateMode:
                    nextCommand = new AudioSourceCmdVelocityUpdateMode(Target);
                    return nextCommand;
                case AudioSourceMembers.panStereo:
                    nextCommand = new AudioSourceCmdPanStereo(Target);
                    return nextCommand;
                case AudioSourceMembers.spatialBlend:
                    nextCommand = new AudioSourceCmdSpatialBlend(Target);
                    return nextCommand;
                case AudioSourceMembers.spatialize:
                    nextCommand = new AudioSourceCmdSpatialize(Target);
                    return nextCommand;
                case AudioSourceMembers.spatializePostEffects:
                    nextCommand = new AudioSourceCmdSpatializePostEffects(Target);
                    return nextCommand;
                case AudioSourceMembers.reverbZoneMix:
                    nextCommand = new AudioSourceCmdReverbZoneMix(Target);
                    return nextCommand;
                case AudioSourceMembers.bypassEffects:
                    nextCommand = new AudioSourceCmdBypassEffects(Target);
                    return nextCommand;
                case AudioSourceMembers.bypassListenerEffects:
                    nextCommand = new AudioSourceCmdBypassListenerEffects(Target);
                    return nextCommand;
                case AudioSourceMembers.bypassReverbZones:
                    nextCommand = new AudioSourceCmdBypassReverbZones(Target);
                    return nextCommand;
                case AudioSourceMembers.dopplerLevel:
                    nextCommand = new AudioSourceCmdDopplerLevel(Target);
                    return nextCommand;
                case AudioSourceMembers.spread:
                    nextCommand = new AudioSourceCmdSpread(Target);
                    return nextCommand;
                case AudioSourceMembers.priority:
                    nextCommand = new AudioSourceCmdPriority(Target);
                    return nextCommand;
                case AudioSourceMembers.mute:
                    nextCommand = new AudioSourceCmdMute(Target);
                    return nextCommand;
                case AudioSourceMembers.minDistance:
                    nextCommand = new AudioSourceCmdMinDistance(Target);
                    return nextCommand;
                case AudioSourceMembers.maxDistance:
                    nextCommand = new AudioSourceCmdMaxDistance(Target);
                    return nextCommand;
                case AudioSourceMembers.rolloffMode:
                    nextCommand = new AudioSourceCmdRolloffMode(Target);
                    return nextCommand;
                case AudioSourceMembers.enabled:
                    nextCommand = new AudioSourceCmdEnabled(Target);
                    return nextCommand;
                default:
                    return null;
            }
        }
    }

    #endregion

    #region AutoGeneratedCommands
    public class AudioSourceCmdVolume : AudioCommand<AudioSource>, IAudioCommand<float>
    {

        public float InitialValue { get; set; }
        public float FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdVolume(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.volume;
            FinalValue = (float)newValue;
            Target.volume = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.volume;
            FinalValue = newValue;
            Target.volume = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.volume;
        }

        public override void Undo()
        {
            Target.volume = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.volume = InitialValue + (t * diff);
        }

    }

    public class AudioSourceCmdPitch : AudioCommand<AudioSource>, IAudioCommand<float>
    {

        public float InitialValue { get; set; }
        public float FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdPitch(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.pitch;
            FinalValue = (float)newValue;
            Target.pitch = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.pitch;
            FinalValue = newValue;
            Target.pitch = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.pitch;
        }

        public override void Undo()
        {
            Target.pitch = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.pitch = InitialValue + (t * diff);
        }

    }

    public class AudioSourceCmdTime : AudioCommand<AudioSource>, IAudioCommand<float>
    {

        public float InitialValue { get; set; }
        public float FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdTime(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.time;
            FinalValue = (float)newValue;
            Target.time = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.time;
            FinalValue = newValue;
            Target.time = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.time;
        }

        public override void Undo()
        {
            Target.time = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.time = InitialValue + (t * diff);
        }

    }

    public class AudioSourceCmdTimeSamples : AudioCommand<AudioSource>, IAudioCommand<int>
    {

        public int InitialValue { get; set; }
        public int FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdTimeSamples(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.timeSamples;
            FinalValue = (int)newValue;
            Target.timeSamples = FinalValue;
        }

        public void Do(int newValue)
        {
            InitialValue = Target.timeSamples;
            FinalValue = newValue;
            Target.timeSamples = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.timeSamples;
        }

        public override void Undo()
        {
            Target.timeSamples = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.timeSamples = InitialValue + (int)(t * diff);
        }

    }

    public class AudioSourceCmdClip : AudioCommand<AudioSource>, IAudioCommand<AudioClip>
    {

        public AudioClip InitialValue { get;  set; }
        public AudioClip FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdClip(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.clip;
            FinalValue = (AudioClip)newValue;
            Target.clip = FinalValue;
        }

        public void Do(AudioClip newValue)
        {
            InitialValue = Target.clip;
            FinalValue = newValue;
            Target.clip = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.clip;
        }

        public override void Undo()
        {
            Target.clip = InitialValue;
        }

    }

    public class AudioSourceCmdOutputAudioMixerGroup : AudioCommand<AudioSource>, IAudioCommand<AudioMixerGroup>
    {

        public AudioMixerGroup InitialValue { get;  set; }
        public AudioMixerGroup FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdOutputAudioMixerGroup(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.outputAudioMixerGroup;
            FinalValue = (AudioMixerGroup)newValue;
            Target.outputAudioMixerGroup = FinalValue;
        }

        public void Do(AudioMixerGroup newValue)
        {
            InitialValue = Target.outputAudioMixerGroup;
            FinalValue = newValue;
            Target.outputAudioMixerGroup = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.outputAudioMixerGroup;
        }

        public override void Undo()
        {
            Target.outputAudioMixerGroup = InitialValue;
        }

    }

    public class AudioSourceCmdLoop : AudioCommand<AudioSource>, IAudioCommand<bool>
    {

        public bool InitialValue { get;  set; }
        public bool FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdLoop(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.loop;
            FinalValue = (bool)newValue;
            Target.loop = FinalValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.loop;
            FinalValue = newValue;
            Target.loop = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.loop;
        }

        public override void Undo()
        {
            Target.loop = InitialValue;
        }

    }

    public class AudioSourceCmdIgnoreListenerVolume : AudioCommand<AudioSource>, IAudioCommand<bool>
    {

        public bool InitialValue { get;  set; }
        public bool FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdIgnoreListenerVolume(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.ignoreListenerVolume;
            FinalValue = (bool)newValue;
            Target.ignoreListenerVolume = FinalValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.ignoreListenerVolume;
            FinalValue = newValue;
            Target.ignoreListenerVolume = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.ignoreListenerVolume;
        }

        public override void Undo()
        {
            Target.ignoreListenerVolume = InitialValue;
        }

    }

    public class AudioSourceCmdPlayOnAwake : AudioCommand<AudioSource>, IAudioCommand<bool>
    {

        public bool InitialValue { get;  set; }
        public bool FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdPlayOnAwake(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.playOnAwake;
            FinalValue = (bool)newValue;
            Target.playOnAwake = FinalValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.playOnAwake;
            FinalValue = newValue;
            Target.playOnAwake = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.playOnAwake;
        }

        public override void Undo()
        {
            Target.playOnAwake = InitialValue;
        }

    }

    public class AudioSourceCmdIgnoreListenerPause : AudioCommand<AudioSource>, IAudioCommand<bool>
    {

        public bool InitialValue { get;  set; }
        public bool FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdIgnoreListenerPause(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.ignoreListenerPause;
            FinalValue = (bool)newValue;
            Target.ignoreListenerPause = FinalValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.ignoreListenerPause;
            FinalValue = newValue;
            Target.ignoreListenerPause = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.ignoreListenerPause;
        }

        public override void Undo()
        {
            Target.ignoreListenerPause = InitialValue;
        }

    }

    public class AudioSourceCmdVelocityUpdateMode : AudioCommand<AudioSource>, IAudioCommand<AudioVelocityUpdateMode>
    {

        public AudioVelocityUpdateMode InitialValue { get; set; }
        public AudioVelocityUpdateMode FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdVelocityUpdateMode(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.velocityUpdateMode;
            FinalValue = (AudioVelocityUpdateMode)newValue;
            Target.velocityUpdateMode = FinalValue;
        }

        public void Do(AudioVelocityUpdateMode newValue)
        {
            InitialValue = Target.velocityUpdateMode;
            FinalValue = newValue;
            Target.velocityUpdateMode = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.velocityUpdateMode;
        }

        public override void Undo()
        {
            Target.velocityUpdateMode = InitialValue;
        }

    }

    public class AudioSourceCmdPanStereo : AudioCommand<AudioSource>, IAudioCommand<float>
    {

        public float InitialValue { get; set; }
        public float FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdPanStereo(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.panStereo;
            FinalValue = (float)newValue;
            Target.panStereo = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.panStereo;
            FinalValue = newValue;
            Target.panStereo = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.panStereo;
        }

        public override void Undo()
        {
            Target.panStereo = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.panStereo = InitialValue + (t * diff);
        }

    }

    public class AudioSourceCmdSpatialBlend : AudioCommand<AudioSource>, IAudioCommand<float>
    {

        public float InitialValue { get; set; }
        public float FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdSpatialBlend(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.spatialBlend;
            FinalValue = (float)newValue;
            Target.spatialBlend = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.spatialBlend;
            FinalValue = newValue;
            Target.spatialBlend = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.spatialBlend;
        }

        public override void Undo()
        {
            Target.spatialBlend = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.spatialBlend = InitialValue + (t * diff);
        }

    }

    public class AudioSourceCmdSpatialize : AudioCommand<AudioSource>, IAudioCommand<bool>
    {

        public bool InitialValue { get;  set; }
        public bool FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdSpatialize(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.spatialize;
            FinalValue = (bool)newValue;
            Target.spatialize = FinalValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.spatialize;
            FinalValue = newValue;
            Target.spatialize = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.spatialize;
        }

        public override void Undo()
        {
            Target.spatialize = InitialValue;
        }

    }

    public class AudioSourceCmdSpatializePostEffects : AudioCommand<AudioSource>, IAudioCommand<bool>
    {

        public bool InitialValue { get;  set; }
        public bool FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdSpatializePostEffects(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.spatializePostEffects;
            FinalValue = (bool)newValue;
            Target.spatializePostEffects = FinalValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.spatializePostEffects;
            FinalValue = newValue;
            Target.spatializePostEffects = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.spatializePostEffects;
        }

        public override void Undo()
        {
            Target.spatializePostEffects = InitialValue;
        }

    }

    public class AudioSourceCmdReverbZoneMix : AudioCommand<AudioSource>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdReverbZoneMix(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.reverbZoneMix;
            FinalValue = (float)newValue;
            Target.reverbZoneMix = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.reverbZoneMix;
            FinalValue = newValue;
            Target.reverbZoneMix = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.reverbZoneMix;
        }

        public override void Undo()
        {
            Target.reverbZoneMix = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.reverbZoneMix = InitialValue + (t * diff);
        }

    }

    public class AudioSourceCmdBypassEffects : AudioCommand<AudioSource>, IAudioCommand<bool>
    {

        public bool InitialValue { get;  set; }
        public bool FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdBypassEffects(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.bypassEffects;
            FinalValue = (bool)newValue;
            Target.bypassEffects = FinalValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.bypassEffects;
            FinalValue = newValue;
            Target.bypassEffects = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.bypassEffects;
        }

        public override void Undo()
        {
            Target.bypassEffects = InitialValue;
        }

    }

    public class AudioSourceCmdBypassListenerEffects : AudioCommand<AudioSource>, IAudioCommand<bool>
    {

        public bool InitialValue { get;  set; }
        public bool FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdBypassListenerEffects(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.bypassListenerEffects;
            FinalValue = (bool)newValue;
            Target.bypassListenerEffects = FinalValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.bypassListenerEffects;
            FinalValue = newValue;
            Target.bypassListenerEffects = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.bypassListenerEffects;
        }

        public override void Undo()
        {
            Target.bypassListenerEffects = InitialValue;
        }

    }

    public class AudioSourceCmdBypassReverbZones : AudioCommand<AudioSource>, IAudioCommand<bool>
    {

        public bool InitialValue { get;  set; }
        public bool FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdBypassReverbZones(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.bypassReverbZones;
            FinalValue = (bool)newValue;
            Target.bypassReverbZones = FinalValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.bypassReverbZones;
            FinalValue = newValue;
            Target.bypassReverbZones = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.bypassReverbZones;
        }

        public override void Undo()
        {
            Target.bypassReverbZones = InitialValue;
        }

    }

    public class AudioSourceCmdDopplerLevel : AudioCommand<AudioSource>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdDopplerLevel(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.dopplerLevel;
            FinalValue = (float)newValue;
            Target.dopplerLevel = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.dopplerLevel;
            FinalValue = newValue;
            Target.dopplerLevel = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.dopplerLevel;
        }

        public override void Undo()
        {
            Target.dopplerLevel = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.dopplerLevel = InitialValue + (t * diff);
        }

    }

    public class AudioSourceCmdSpread : AudioCommand<AudioSource>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdSpread(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.spread;
            FinalValue = (float)newValue;
            Target.spread = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.spread;
            FinalValue = newValue;
            Target.spread = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.spread;
        }

        public override void Undo()
        {
            Target.spread = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.spread = InitialValue + (t * diff);
        }

    }

    public class AudioSourceCmdPriority : AudioCommand<AudioSource>, IAudioCommand<int>
    {

        public int InitialValue { get;  set; }
        public int FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdPriority(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.priority;
            FinalValue = (int)newValue;
            Target.priority = FinalValue;
        }

        public void Do(int newValue)
        {
            InitialValue = Target.priority;
            FinalValue = newValue;
            Target.priority = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.priority;
        }

        public override void Undo()
        {
            Target.priority = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.priority = InitialValue + (int)(t * diff);
        }

    }

    public class AudioSourceCmdMute : AudioCommand<AudioSource>, IAudioCommand<bool>
    {

        public bool InitialValue { get;  set; }
        public bool FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdMute(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.mute;
            FinalValue = (bool)newValue;
            Target.mute = FinalValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.mute;
            FinalValue = newValue;
            Target.mute = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.mute;
        }

        public override void Undo()
        {
            Target.mute = InitialValue;
        }

    }

    public class AudioSourceCmdMinDistance : AudioCommand<AudioSource>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdMinDistance(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.minDistance;
            FinalValue = (float)newValue;
            Target.minDistance = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.minDistance;
            FinalValue = newValue;
            Target.minDistance = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.minDistance;
        }

        public override void Undo()
        {
            Target.minDistance = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.minDistance = InitialValue + (t * diff);
        }

    }

    public class AudioSourceCmdMaxDistance : AudioCommand<AudioSource>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdMaxDistance(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.maxDistance;
            FinalValue = (float)newValue;
            Target.maxDistance = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.maxDistance;
            FinalValue = newValue;
            Target.maxDistance = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.maxDistance;
        }

        public override void Undo()
        {
            Target.maxDistance = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.maxDistance = InitialValue + (t * diff);
        }

    }

    public class AudioSourceCmdRolloffMode : AudioCommand<AudioSource>, IAudioCommand<AudioRolloffMode>
    {

        public AudioRolloffMode InitialValue { get;  set; }
        public AudioRolloffMode FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdRolloffMode(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.rolloffMode;
            FinalValue = (AudioRolloffMode)newValue;
            Target.rolloffMode = FinalValue;
        }

        public void Do(AudioRolloffMode newValue)
        {
            InitialValue = Target.rolloffMode;
            FinalValue = newValue;
            Target.rolloffMode = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.rolloffMode;
        }

        public override void Undo()
        {
            Target.rolloffMode = InitialValue;
        }

    }

    public class AudioSourceCmdEnabled : AudioCommand<AudioSource>, IAudioCommand<bool>
    {

        public bool InitialValue { get;  set; }
        public bool FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioSourceCmdEnabled(AudioSource target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.enabled;
            FinalValue = (bool)newValue;
            Target.enabled = FinalValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = Target.enabled;
            FinalValue = newValue;
            Target.enabled = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioSourceMembers.enabled;
        }

        public override void Undo()
        {
            Target.enabled = InitialValue;
        }

    }

    #endregion
}


