

namespace Audiomata.ComponentTrackers
{
    using Audiomata;
    //-------------------------------------------------------------------------------------------------------------------------------
    //					THIS CODE IS AUTOMATICALLY GENERATED, DO NOT MODIFY
    //-------------------------------------------------------------------------------------------------------------------------------
    using UnityEngine;

    // Auto generated props, can use reflections but don't because it is REALLY slow
    #region EnumeratedProps
    public enum AudioLowPassFilterMembers
    {
        enabled
        , customCutoffCurve
            , cutoffFrequency
            , lowpassResonanceQ

    }
    #endregion

    #region CommmandManager
    //Class to manage state of components
    public class AudioLowPassFilterCommander : IAudioCommandable
    {
        private LimitedStack<AudioCommand<AudioLowPassFilter>> audioCommands;
        public AudioLowPassFilter Target { get; private set; }

        public AudioLowPassFilterCommander(AudioLowPassFilter lowPassFilter)
        {
            Target = lowPassFilter;
            audioCommands = new LimitedStack<AudioCommand<AudioLowPassFilter>>();
        }

        public object DoCommand<T>(T value, int enumeratedProp)
        {
            AudioCommand<AudioLowPassFilter> newCommand = CommandFactory(enumeratedProp);
            newCommand.Do(value);
            audioCommands.Push(newCommand);
            return newCommand;
        }

        public AudioCommand<AudioLowPassFilter> CreateCommand(int enumeratedProp)
        {
            AudioCommand<AudioLowPassFilter> newCommand = CommandFactory(enumeratedProp);
            audioCommands.Push(newCommand);
            return newCommand;
        }

        public object RegisterCommand(int enumeratedProp)
        {
            AudioCommand<AudioLowPassFilter> newCommand = CommandFactory(enumeratedProp);
            audioCommands.Push(newCommand);
            return newCommand;
        }

        public void UndoCommand(object cmd)
        {
            AudioCommand<AudioLowPassFilter> command = (AudioCommand<AudioLowPassFilter>)cmd;
            command.Undo();
            audioCommands.Remove(command);
        }


        public void UndoLast()
        {
            if (audioCommands.Count < 1)
            {
                return;
            }

            AudioCommand<AudioLowPassFilter> popped = audioCommands.Pop();
            popped.Undo();
        }

        public void UndoAll()
        {
            while (audioCommands.Count > 0)
            {
                AudioCommand<AudioLowPassFilter> popped = audioCommands.Pop();
                popped.Undo();
            }
        }

        public void ClearChangeHistory()
        {
            while (audioCommands.Count > 0)
            {
                AudioCommand<AudioLowPassFilter> popped = audioCommands.Pop();
            }
        }

        private AudioCommand<AudioLowPassFilter> CommandFactory(int enumeratedProp)
        {
            AudioLowPassFilterMembers commandPropTarget = (AudioLowPassFilterMembers)enumeratedProp;
            AudioCommand<AudioLowPassFilter> nextCommand;
            switch (commandPropTarget)
            {
                case AudioLowPassFilterMembers.customCutoffCurve:
                    nextCommand = new AudioLowPassFilterCmdCustomCutoffCurve(Target);
                    return nextCommand;
                case AudioLowPassFilterMembers.cutoffFrequency:
                    nextCommand = new AudioLowPassFilterCmdCutoffFrequency(Target);
                    return nextCommand;
                case AudioLowPassFilterMembers.lowpassResonanceQ:
                    nextCommand = new AudioLowPassFilterCmdLowpassResonanceQ(Target);
                    return nextCommand;
                case AudioLowPassFilterMembers.enabled:
                    nextCommand = new AudioLowPassFilterCmdEnabled(Target);
                    return nextCommand;
                default:
                    return null;
            }
        }
    }

    #endregion

    #region AutoGeneratedCommands
    public class AudioLowPassFilterCmdCustomCutoffCurve : AudioCommand<AudioLowPassFilter>, IAudioCommand<AnimationCurve>
    {

        public AnimationCurve InitialValue { get;  set; }
        public AnimationCurve FinalValue { get;  set; }
        public CommandState CommandState { get; set; }


        public AudioLowPassFilterCmdCustomCutoffCurve(AudioLowPassFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.customCutoffCurve;
            FinalValue = (AnimationCurve)newValue;
            Target.customCutoffCurve = FinalValue;
        }

        public void Do(AnimationCurve newValue)
        {
            InitialValue = Target.customCutoffCurve;
            FinalValue = newValue;
            Target.customCutoffCurve = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioLowPassFilterMembers.customCutoffCurve;
        }

        public override void Undo()
        {
            Target.customCutoffCurve = InitialValue;
        }

    }

    public class AudioLowPassFilterCmdCutoffFrequency : AudioCommand<AudioLowPassFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioLowPassFilterCmdCutoffFrequency(AudioLowPassFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.cutoffFrequency;
            FinalValue = (float)newValue;
            Target.cutoffFrequency = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.cutoffFrequency;
            FinalValue = newValue;
            Target.cutoffFrequency = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioLowPassFilterMembers.cutoffFrequency;
        }

        public override void Undo()
        {
            Target.cutoffFrequency = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.cutoffFrequency = InitialValue + (t * diff);
        }

    }

    public class AudioLowPassFilterCmdLowpassResonanceQ : AudioCommand<AudioLowPassFilter>, IAudioCommand<float>
    {

        public float InitialValue { get;  set; }
        public float FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioLowPassFilterCmdLowpassResonanceQ(AudioLowPassFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.lowpassResonanceQ;
            FinalValue = (float)newValue;
            Target.lowpassResonanceQ = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.lowpassResonanceQ;
            FinalValue = newValue;
            Target.lowpassResonanceQ = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioLowPassFilterMembers.lowpassResonanceQ;
        }

        public override void Undo()
        {
            Target.lowpassResonanceQ = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.lowpassResonanceQ = InitialValue + (t * diff);
        }

    }

    public class AudioLowPassFilterCmdEnabled : AudioCommand<AudioLowPassFilter>, IAudioCommand<bool>
    {

        public bool InitialValue { get;  set; }
        public bool FinalValue { get; set; }
        public CommandState CommandState { get; set; }


        public AudioLowPassFilterCmdEnabled(AudioLowPassFilter target)
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
            return (int)AudioLowPassFilterMembers.enabled;
        }

        public override void Undo()
        {
            Target.enabled = InitialValue;
        }

    }

    #endregion
}


