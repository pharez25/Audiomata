

namespace Audiomata.ComponentTrackers
{
    using Audiomata;
    //-------------------------------------------------------------------------------------------------------------------------------
    //					THIS CODE IS AUTOMATICALLY GENERATED, DO NOT MODIFY
    //-------------------------------------------------------------------------------------------------------------------------------
    using UnityEngine;

    // Auto generated props, can use reflections but don't because it is REALLY slow
    #region EnumeratedProps
    public enum AudioReverbZoneMembers
    {
        enabled
        , minDistance
            , maxDistance
            , reverbPreset
            , room
            , roomHF
            , roomLF
            , decayTime
            , decayHFRatio
            , reflections
            , reflectionsDelay
            , reverb
            , reverbDelay
            , HFReference
            , LFReference
            , diffusion
            , density

    }
    #endregion

    #region CommmandManager
    //Class to manage state of components
    public class AudioReverbZoneCommander : IAudioCommandable
    {
        private LimitedStack<AudioCommand<AudioReverbZone>> audioCommands;
        public AudioReverbZone Target { get; private set; }

        public AudioReverbZoneCommander()
        {
            audioCommands = new LimitedStack<AudioCommand<AudioReverbZone>>();
        }

        public object DoCommand<T>(T value, int enumeratedProp)
        {
            AudioCommand<AudioReverbZone> newCommand = CommandFactory(enumeratedProp);
            newCommand.Do(value);
            audioCommands.Push(newCommand);
            return newCommand;
        }

        public object RegisterCommand(int enumeratedProp)
        {
            AudioCommand<AudioReverbZone> newCommand = CommandFactory(enumeratedProp);
            audioCommands.Push(newCommand);
            return newCommand;
        }

        public void UndoCommand(object cmd)
        {
            AudioCommand<AudioReverbZone> command = (AudioCommand<AudioReverbZone>)cmd;
            command.Undo();
            audioCommands.Remove(command);
        }


        public void UndoLast()
        {
            if (audioCommands.Count < 1)
            {
                return;
            }

            AudioCommand<AudioReverbZone> popped = audioCommands.Pop();
            popped.Undo();
        }

        public void UndoAll()
        {
            while (audioCommands.Count > 0)
            {
                AudioCommand<AudioReverbZone> popped = audioCommands.Pop();
                popped.Undo();
            }
        }

        public void ClearChangeHistory()
        {
            while (audioCommands.Count > 0)
            {
                AudioCommand<AudioReverbZone> popped = audioCommands.Pop();
            }
        }

        private AudioCommand<AudioReverbZone> CommandFactory(int enumeratedProp)
        {
            AudioReverbZoneMembers commandPropTarget = (AudioReverbZoneMembers)enumeratedProp;
            AudioCommand<AudioReverbZone> nextCommand;
            switch (commandPropTarget)
            {
                case AudioReverbZoneMembers.minDistance:
                    nextCommand = new AudioReverbZoneCmdMinDistance(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.maxDistance:
                    nextCommand = new AudioReverbZoneCmdMaxDistance(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.reverbPreset:
                    nextCommand = new AudioReverbZoneCmdReverbPreset(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.room:
                    nextCommand = new AudioReverbZoneCmdRoom(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.roomHF:
                    nextCommand = new AudioReverbZoneCmdRoomHF(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.roomLF:
                    nextCommand = new AudioReverbZoneCmdRoomLF(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.decayTime:
                    nextCommand = new AudioReverbZoneCmdDecayTime(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.decayHFRatio:
                    nextCommand = new AudioReverbZoneCmdDecayHFRatio(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.reflections:
                    nextCommand = new AudioReverbZoneCmdReflections(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.reflectionsDelay:
                    nextCommand = new AudioReverbZoneCmdReflectionsDelay(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.reverb:
                    nextCommand = new AudioReverbZoneCmdReverb(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.reverbDelay:
                    nextCommand = new AudioReverbZoneCmdReverbDelay(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.HFReference:
                    nextCommand = new AudioReverbZoneCmdHFReference(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.LFReference:
                    nextCommand = new AudioReverbZoneCmdLFReference(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.diffusion:
                    nextCommand = new AudioReverbZoneCmdDiffusion(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.density:
                    nextCommand = new AudioReverbZoneCmdDensity(Target);
                    return nextCommand;
                case AudioReverbZoneMembers.enabled:
                    nextCommand = new AudioReverbZoneCmdEnabled(Target);
                    return nextCommand;
                default:
                    return null;
            }
        }
    }

    #endregion

    #region AutoGeneratedCommands
    public class AudioReverbZoneCmdMinDistance : AudioCommand<AudioReverbZone>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdMinDistance(AudioReverbZone target)
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
            return (int)AudioReverbZoneMembers.minDistance;
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

    public class AudioReverbZoneCmdMaxDistance : AudioCommand<AudioReverbZone>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdMaxDistance(AudioReverbZone target)
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
            return (int)AudioReverbZoneMembers.maxDistance;
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

    public class AudioReverbZoneCmdReverbPreset : AudioCommand<AudioReverbZone>, IAudioCommand<AudioReverbPreset>
    {

        public AudioReverbPreset InitialValue { get; private set; }
        public AudioReverbPreset FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdReverbPreset(AudioReverbZone target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.reverbPreset;
            FinalValue = (AudioReverbPreset)newValue;
            Target.reverbPreset = FinalValue;
        }

        public void Do(AudioReverbPreset newValue)
        {
            InitialValue = Target.reverbPreset;
            FinalValue = newValue;
            Target.reverbPreset = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbZoneMembers.reverbPreset;
        }

        public override void Undo()
        {
            Target.reverbPreset = InitialValue;
        }

    }

    public class AudioReverbZoneCmdRoom : AudioCommand<AudioReverbZone>, IAudioCommand<int>
    {

        public int InitialValue { get; private set; }
        public int FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdRoom(AudioReverbZone target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.room;
            FinalValue = (int)newValue;
            Target.room = FinalValue;
        }

        public void Do(int newValue)
        {
            InitialValue = Target.room;
            FinalValue = newValue;
            Target.room = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbZoneMembers.room;
        }

        public override void Undo()
        {
            Target.room = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.room = InitialValue + (int)(t * diff);
        }

    }

    public class AudioReverbZoneCmdRoomHF : AudioCommand<AudioReverbZone>, IAudioCommand<int>
    {

        public int InitialValue { get; private set; }
        public int FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdRoomHF(AudioReverbZone target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.roomHF;
            FinalValue = (int)newValue;
            Target.roomHF = FinalValue;
        }

        public void Do(int newValue)
        {
            InitialValue = Target.roomHF;
            FinalValue = newValue;
            Target.roomHF = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbZoneMembers.roomHF;
        }

        public override void Undo()
        {
            Target.roomHF = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.roomHF = InitialValue + (int)(t * diff);
        }

    }

    public class AudioReverbZoneCmdRoomLF : AudioCommand<AudioReverbZone>, IAudioCommand<int>
    {

        public int InitialValue { get; private set; }
        public int FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdRoomLF(AudioReverbZone target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.roomLF;
            FinalValue = (int)newValue;
            Target.roomLF = FinalValue;
        }

        public void Do(int newValue)
        {
            InitialValue = Target.roomLF;
            FinalValue = newValue;
            Target.roomLF = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbZoneMembers.roomLF;
        }

        public override void Undo()
        {
            Target.roomLF = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.roomLF = InitialValue + (int)(t * diff);
        }

    }

    public class AudioReverbZoneCmdDecayTime : AudioCommand<AudioReverbZone>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdDecayTime(AudioReverbZone target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.decayTime;
            FinalValue = (float)newValue;
            Target.decayTime = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.decayTime;
            FinalValue = newValue;
            Target.decayTime = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbZoneMembers.decayTime;
        }

        public override void Undo()
        {
            Target.decayTime = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.decayTime = InitialValue + (t * diff);
        }

    }

    public class AudioReverbZoneCmdDecayHFRatio : AudioCommand<AudioReverbZone>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdDecayHFRatio(AudioReverbZone target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.decayHFRatio;
            FinalValue = (float)newValue;
            Target.decayHFRatio = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.decayHFRatio;
            FinalValue = newValue;
            Target.decayHFRatio = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbZoneMembers.decayHFRatio;
        }

        public override void Undo()
        {
            Target.decayHFRatio = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.decayHFRatio = InitialValue + (t * diff);
        }

    }

    public class AudioReverbZoneCmdReflections : AudioCommand<AudioReverbZone>, IAudioCommand<int>
    {

        public int InitialValue { get; private set; }
        public int FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdReflections(AudioReverbZone target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.reflections;
            FinalValue = (int)newValue;
            Target.reflections = FinalValue;
        }

        public void Do(int newValue)
        {
            InitialValue = Target.reflections;
            FinalValue = newValue;
            Target.reflections = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbZoneMembers.reflections;
        }

        public override void Undo()
        {
            Target.reflections = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.reflections = InitialValue + (int)(t * diff);
        }

    }

    public class AudioReverbZoneCmdReflectionsDelay : AudioCommand<AudioReverbZone>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdReflectionsDelay(AudioReverbZone target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.reflectionsDelay;
            FinalValue = (float)newValue;
            Target.reflectionsDelay = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.reflectionsDelay;
            FinalValue = newValue;
            Target.reflectionsDelay = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbZoneMembers.reflectionsDelay;
        }

        public override void Undo()
        {
            Target.reflectionsDelay = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.reflectionsDelay = InitialValue + (t * diff);
        }

    }

    public class AudioReverbZoneCmdReverb : AudioCommand<AudioReverbZone>, IAudioCommand<int>
    {

        public int InitialValue { get; private set; }
        public int FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdReverb(AudioReverbZone target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.reverb;
            FinalValue = (int)newValue;
            Target.reverb = FinalValue;
        }

        public void Do(int newValue)
        {
            InitialValue = Target.reverb;
            FinalValue = newValue;
            Target.reverb = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbZoneMembers.reverb;
        }

        public override void Undo()
        {
            Target.reverb = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.reverb = InitialValue + (int)(t * diff);
        }

    }

    public class AudioReverbZoneCmdReverbDelay : AudioCommand<AudioReverbZone>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdReverbDelay(AudioReverbZone target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.reverbDelay;
            FinalValue = (float)newValue;
            Target.reverbDelay = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.reverbDelay;
            FinalValue = newValue;
            Target.reverbDelay = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbZoneMembers.reverbDelay;
        }

        public override void Undo()
        {
            Target.reverbDelay = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.reverbDelay = InitialValue + (t * diff);
        }

    }

    public class AudioReverbZoneCmdHFReference : AudioCommand<AudioReverbZone>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdHFReference(AudioReverbZone target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.HFReference;
            FinalValue = (float)newValue;
            Target.HFReference = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.HFReference;
            FinalValue = newValue;
            Target.HFReference = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbZoneMembers.HFReference;
        }

        public override void Undo()
        {
            Target.HFReference = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.HFReference = InitialValue + (t * diff);
        }

    }

    public class AudioReverbZoneCmdLFReference : AudioCommand<AudioReverbZone>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdLFReference(AudioReverbZone target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.LFReference;
            FinalValue = (float)newValue;
            Target.LFReference = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.LFReference;
            FinalValue = newValue;
            Target.LFReference = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbZoneMembers.LFReference;
        }

        public override void Undo()
        {
            Target.LFReference = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.LFReference = InitialValue + (t * diff);
        }

    }

    public class AudioReverbZoneCmdDiffusion : AudioCommand<AudioReverbZone>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdDiffusion(AudioReverbZone target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.diffusion;
            FinalValue = (float)newValue;
            Target.diffusion = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.diffusion;
            FinalValue = newValue;
            Target.diffusion = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbZoneMembers.diffusion;
        }

        public override void Undo()
        {
            Target.diffusion = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.diffusion = InitialValue + (t * diff);
        }

    }

    public class AudioReverbZoneCmdDensity : AudioCommand<AudioReverbZone>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdDensity(AudioReverbZone target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.density;
            FinalValue = (float)newValue;
            Target.density = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.density;
            FinalValue = newValue;
            Target.density = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbZoneMembers.density;
        }

        public override void Undo()
        {
            Target.density = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.density = InitialValue + (t * diff);
        }

    }

    public class AudioReverbZoneCmdEnabled : AudioCommand<AudioReverbZone>, IAudioCommand<bool>
    {

        public bool InitialValue { get; private set; }
        public bool FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioReverbZoneCmdEnabled(AudioReverbZone target)
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
            return (int)AudioReverbZoneMembers.enabled;
        }

        public override void Undo()
        {
            Target.enabled = InitialValue;
        }

    }

    #endregion
}


