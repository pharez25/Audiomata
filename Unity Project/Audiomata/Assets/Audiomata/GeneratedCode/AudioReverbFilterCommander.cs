

namespace Audiomata.ComponentTrackers
{
    using Audiomata;
    //-------------------------------------------------------------------------------------------------------------------------------
    //					THIS CODE IS AUTOMATICALLY GENERATED, DO NOT MODIFY
    //-------------------------------------------------------------------------------------------------------------------------------
    using UnityEngine;

    // Auto generated props, can use reflections but don't because it is REALLY slow
    #region EnumeratedProps
    public enum AudioReverbFilterMembers
    {
        enabled
        , reverbPreset
            , dryLevel
            , room
            , roomHF
            , decayTime
            , decayHFRatio
            , reflectionsLevel
            , reflectionsDelay
            , reverbLevel
            , reverbDelay
            , diffusion
            , density
            , hfReference
            , roomLF
            , lfReference

    }
    #endregion

    #region CommmandManager
    //Class to manage state of components
    public class AudioReverbFilterCommander : IAudioCommandable
    {
        private LimitedStack<AudioCommand<AudioReverbFilter>> audioCommands;
        public AudioReverbFilter Target { get; private set; }

        public AudioReverbFilterCommander(AudioReverbFilter target)
        {
            audioCommands = new LimitedStack<AudioCommand<AudioReverbFilter>>();
            Target = target;
        }

        public object DoCommand<T>(T value, int enumeratedProp)
        {
            AudioCommand<AudioReverbFilter> newCommand = CommandFactory(enumeratedProp);
            newCommand.Do(value);
            audioCommands.Push(newCommand);
            return newCommand;
        }

        public AudioCommand<AudioReverbFilter> CreateCommand(int enumeratedProp)
        {
            AudioCommand<AudioReverbFilter> newCommand = CommandFactory(enumeratedProp);
            audioCommands.Push(newCommand);
            return newCommand;
        }

        public object RegisterCommand(int enumeratedProp)
        {
            AudioCommand<AudioReverbFilter> newCommand = CommandFactory(enumeratedProp);
            audioCommands.Push(newCommand);
            return newCommand;
        }

        public void UndoCommand(object cmd)
        {
            AudioCommand<AudioReverbFilter> command = (AudioCommand<AudioReverbFilter>)cmd;
            command.Undo();
            audioCommands.Remove(command);
        }


        public void UndoLast()
        {
            if (audioCommands.Count < 1)
            {
                return;
            }

            AudioCommand<AudioReverbFilter> popped = audioCommands.Pop();
            popped.Undo();
        }

        public void UndoAll()
        {
            while (audioCommands.Count > 0)
            {
                AudioCommand<AudioReverbFilter> popped = audioCommands.Pop();
                popped.Undo();
            }
        }

        public void ClearChangeHistory()
        {
            while (audioCommands.Count > 0)
            {
                AudioCommand<AudioReverbFilter> popped = audioCommands.Pop();
            }
        }

        private AudioCommand<AudioReverbFilter> CommandFactory(int enumeratedProp)
        {
            AudioReverbFilterMembers commandPropTarget = (AudioReverbFilterMembers)enumeratedProp;
            AudioCommand<AudioReverbFilter> nextCommand;
            switch (commandPropTarget)
            {
                case AudioReverbFilterMembers.reverbPreset:
                    nextCommand = new AudioReverbFilterCmdReverbPreset(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.dryLevel:
                    nextCommand = new AudioReverbFilterCmdDryLevel(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.room:
                    nextCommand = new AudioReverbFilterCmdRoom(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.roomHF:
                    nextCommand = new AudioReverbFilterCmdRoomHF(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.decayTime:
                    nextCommand = new AudioReverbFilterCmdDecayTime(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.decayHFRatio:
                    nextCommand = new AudioReverbFilterCmdDecayHFRatio(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.reflectionsLevel:
                    nextCommand = new AudioReverbFilterCmdReflectionsLevel(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.reflectionsDelay:
                    nextCommand = new AudioReverbFilterCmdReflectionsDelay(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.reverbLevel:
                    nextCommand = new AudioReverbFilterCmdReverbLevel(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.reverbDelay:
                    nextCommand = new AudioReverbFilterCmdReverbDelay(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.diffusion:
                    nextCommand = new AudioReverbFilterCmdDiffusion(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.density:
                    nextCommand = new AudioReverbFilterCmdDensity(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.hfReference:
                    nextCommand = new AudioReverbFilterCmdHfReference(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.roomLF:
                    nextCommand = new AudioReverbFilterCmdRoomLF(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.lfReference:
                    nextCommand = new AudioReverbFilterCmdLfReference(Target);
                    return nextCommand;
                case AudioReverbFilterMembers.enabled:
                    nextCommand = new AudioReverbFilterCmdEnabled(Target);
                    return nextCommand;
                default:
                    return null;
            }
        }
    }

    #endregion

    #region AutoGeneratedCommands
    public class AudioReverbFilterCmdReverbPreset : AudioCommand<AudioReverbFilter>, IAudioCommand<AudioReverbPreset>
    {

        public AudioReverbPreset InitialValue { get;  set; }
        public AudioReverbPreset FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdReverbPreset(AudioReverbFilter target)
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
            return (int)AudioReverbFilterMembers.reverbPreset;
        }

        public override void Undo()
        {
            Target.reverbPreset = InitialValue;
        }

    }

    public class AudioReverbFilterCmdDryLevel : AudioCommand<AudioReverbFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdDryLevel(AudioReverbFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.dryLevel;
            FinalValue = (float)newValue;
            Target.dryLevel = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.dryLevel;
            FinalValue = newValue;
            Target.dryLevel = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbFilterMembers.dryLevel;
        }

        public override void Undo()
        {
            Target.dryLevel = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.dryLevel = InitialValue + (t * diff);
        }

    }

    public class AudioReverbFilterCmdRoom : AudioCommand<AudioReverbFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdRoom(AudioReverbFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.room;
            FinalValue = (float)newValue;
            Target.room = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.room;
            FinalValue = newValue;
            Target.room = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbFilterMembers.room;
        }

        public override void Undo()
        {
            Target.room = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.room = InitialValue + (t * diff);
        }

    }

    public class AudioReverbFilterCmdRoomHF : AudioCommand<AudioReverbFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdRoomHF(AudioReverbFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.roomHF;
            FinalValue = (float)newValue;
            Target.roomHF = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.roomHF;
            FinalValue = newValue;
            Target.roomHF = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbFilterMembers.roomHF;
        }

        public override void Undo()
        {
            Target.roomHF = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.roomHF = InitialValue + (t * diff);
        }

    }

    public class AudioReverbFilterCmdDecayTime : AudioCommand<AudioReverbFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdDecayTime(AudioReverbFilter target)
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
            return (int)AudioReverbFilterMembers.decayTime;
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

    public class AudioReverbFilterCmdDecayHFRatio : AudioCommand<AudioReverbFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdDecayHFRatio(AudioReverbFilter target)
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
            return (int)AudioReverbFilterMembers.decayHFRatio;
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

    public class AudioReverbFilterCmdReflectionsLevel : AudioCommand<AudioReverbFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdReflectionsLevel(AudioReverbFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.reflectionsLevel;
            FinalValue = (float)newValue;
            Target.reflectionsLevel = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.reflectionsLevel;
            FinalValue = newValue;
            Target.reflectionsLevel = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbFilterMembers.reflectionsLevel;
        }

        public override void Undo()
        {
            Target.reflectionsLevel = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.reflectionsLevel = InitialValue + (t * diff);
        }

    }

    public class AudioReverbFilterCmdReflectionsDelay : AudioCommand<AudioReverbFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdReflectionsDelay(AudioReverbFilter target)
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
            return (int)AudioReverbFilterMembers.reflectionsDelay;
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

    public class AudioReverbFilterCmdReverbLevel : AudioCommand<AudioReverbFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdReverbLevel(AudioReverbFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.reverbLevel;
            FinalValue = (float)newValue;
            Target.reverbLevel = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.reverbLevel;
            FinalValue = newValue;
            Target.reverbLevel = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbFilterMembers.reverbLevel;
        }

        public override void Undo()
        {
            Target.reverbLevel = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.reverbLevel = InitialValue + (t * diff);
        }

    }

    public class AudioReverbFilterCmdReverbDelay : AudioCommand<AudioReverbFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdReverbDelay(AudioReverbFilter target)
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
            return (int)AudioReverbFilterMembers.reverbDelay;
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

    public class AudioReverbFilterCmdDiffusion : AudioCommand<AudioReverbFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdDiffusion(AudioReverbFilter target)
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
            return (int)AudioReverbFilterMembers.diffusion;
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

    public class AudioReverbFilterCmdDensity : AudioCommand<AudioReverbFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdDensity(AudioReverbFilter target)
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
            return (int)AudioReverbFilterMembers.density;
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

    public class AudioReverbFilterCmdHfReference : AudioCommand<AudioReverbFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdHfReference(AudioReverbFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.hfReference;
            FinalValue = (float)newValue;
            Target.hfReference = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.hfReference;
            FinalValue = newValue;
            Target.hfReference = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbFilterMembers.hfReference;
        }

        public override void Undo()
        {
            Target.hfReference = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.hfReference = InitialValue + (t * diff);
        }

    }

    public class AudioReverbFilterCmdRoomLF : AudioCommand<AudioReverbFilter>, IAudioCommand<float>
    {

        public float InitialValue { get; set; }
        public float FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdRoomLF(AudioReverbFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.roomLF;
            FinalValue = (float)newValue;
            Target.roomLF = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.roomLF;
            FinalValue = newValue;
            Target.roomLF = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbFilterMembers.roomLF;
        }

        public override void Undo()
        {
            Target.roomLF = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.roomLF = InitialValue + (t * diff);
        }

    }

    public class AudioReverbFilterCmdLfReference : AudioCommand<AudioReverbFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdLfReference(AudioReverbFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.lfReference;
            FinalValue = (float)newValue;
            Target.lfReference = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.lfReference;
            FinalValue = newValue;
            Target.lfReference = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioReverbFilterMembers.lfReference;
        }

        public override void Undo()
        {
            Target.lfReference = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.lfReference = InitialValue + (t * diff);
        }

    }

    public class AudioReverbFilterCmdEnabled : AudioCommand<AudioReverbFilter>, IAudioCommand<bool>
    {

        public bool InitialValue { get; set; }
        public bool FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioReverbFilterCmdEnabled(AudioReverbFilter target)
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
            return (int)AudioReverbFilterMembers.enabled;
        }

        public override void Undo()
        {
            Target.enabled = InitialValue;
        }

    }

    #endregion
}


