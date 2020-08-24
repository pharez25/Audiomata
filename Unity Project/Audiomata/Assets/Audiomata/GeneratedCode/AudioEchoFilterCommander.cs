

namespace Audiomata.ComponentTrackers
{
    using Audiomata;
    //-------------------------------------------------------------------------------------------------------------------------------
    //					THIS CODE IS AUTOMATICALLY GENERATED, DO NOT MODIFY
    //-------------------------------------------------------------------------------------------------------------------------------
    using UnityEngine;

    // Auto generated props, can use reflections but don't because it is REALLY slow
    #region EnumeratedProps
    public enum AudioEchoFilterMembers
    {
        enabled
        , delay
            , decayRatio
            , dryMix
            , wetMix

    }
    #endregion

    #region CommmandManager
    //Class to manage state of components
    public class AudioEchoFilterCommander : IAudioCommandable
    {
        private LimitedStack<AudioCommand<AudioEchoFilter>> audioCommands;
        public AudioEchoFilter Target { get; private set; }

        public AudioEchoFilterCommander()
        {
            audioCommands = new LimitedStack<AudioCommand<AudioEchoFilter>>();
        }

        public object DoCommand<T>(T value, int enumeratedProp)
        {
            AudioCommand<AudioEchoFilter> newCommand = CommandFactory(enumeratedProp);
            newCommand.Do(value);
            audioCommands.Push(newCommand);
            return newCommand;
        }

        public object RegisterCommand(int enumeratedProp)
        {
            AudioCommand<AudioEchoFilter> newCommand = CommandFactory(enumeratedProp);
            audioCommands.Push(newCommand);
            return newCommand;
        }

        public void UndoCommand(object cmd)
        {
            AudioCommand<AudioEchoFilter> command = (AudioCommand<AudioEchoFilter>)cmd;
            command.Undo();
            audioCommands.Remove(command);
        }


        public void UndoLast()
        {
            if (audioCommands.Count < 1)
            {
                return;
            }

            AudioCommand<AudioEchoFilter> popped = audioCommands.Pop();
            popped.Undo();
        }

        public void UndoAll()
        {
            while (audioCommands.Count > 0)
            {
                AudioCommand<AudioEchoFilter> popped = audioCommands.Pop();
                popped.Undo();
            }
        }

        public void ClearChangeHistory()
        {
            while (audioCommands.Count > 0)
            {
                AudioCommand<AudioEchoFilter> popped = audioCommands.Pop();
            }
        }

        private AudioCommand<AudioEchoFilter> CommandFactory(int enumeratedProp)
        {
            AudioEchoFilterMembers commandPropTarget = (AudioEchoFilterMembers)enumeratedProp;
            AudioCommand<AudioEchoFilter> nextCommand;
            switch (commandPropTarget)
            {
                case AudioEchoFilterMembers.delay:
                    nextCommand = new AudioEchoFilterCmdDelay(Target);
                    return nextCommand;
                case AudioEchoFilterMembers.decayRatio:
                    nextCommand = new AudioEchoFilterCmdDecayRatio(Target);
                    return nextCommand;
                case AudioEchoFilterMembers.dryMix:
                    nextCommand = new AudioEchoFilterCmdDryMix(Target);
                    return nextCommand;
                case AudioEchoFilterMembers.wetMix:
                    nextCommand = new AudioEchoFilterCmdWetMix(Target);
                    return nextCommand;
                case AudioEchoFilterMembers.enabled:
                    nextCommand = new AudioEchoFilterCmdEnabled(Target);
                    return nextCommand;
                default:
                    return null;
            }
        }
    }

    #endregion

    #region AutoGeneratedCommands
    public class AudioEchoFilterCmdDelay : AudioCommand<AudioEchoFilter>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioEchoFilterCmdDelay(AudioEchoFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.delay;
            FinalValue = (float)newValue;
            Target.delay = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.delay;
            FinalValue = newValue;
            Target.delay = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioEchoFilterMembers.delay;
        }

        public override void Undo()
        {
            Target.delay = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.delay = InitialValue + (t * diff);
        }

    }

    public class AudioEchoFilterCmdDecayRatio : AudioCommand<AudioEchoFilter>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioEchoFilterCmdDecayRatio(AudioEchoFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.decayRatio;
            FinalValue = (float)newValue;
            Target.decayRatio = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.decayRatio;
            FinalValue = newValue;
            Target.decayRatio = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioEchoFilterMembers.decayRatio;
        }

        public override void Undo()
        {
            Target.decayRatio = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.decayRatio = InitialValue + (t * diff);
        }

    }

    public class AudioEchoFilterCmdDryMix : AudioCommand<AudioEchoFilter>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioEchoFilterCmdDryMix(AudioEchoFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.dryMix;
            FinalValue = (float)newValue;
            Target.dryMix = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.dryMix;
            FinalValue = newValue;
            Target.dryMix = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioEchoFilterMembers.dryMix;
        }

        public override void Undo()
        {
            Target.dryMix = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.dryMix = InitialValue + (t * diff);
        }

    }

    public class AudioEchoFilterCmdWetMix : AudioCommand<AudioEchoFilter>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioEchoFilterCmdWetMix(AudioEchoFilter target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = Target.wetMix;
            FinalValue = (float)newValue;
            Target.wetMix = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = Target.wetMix;
            FinalValue = newValue;
            Target.wetMix = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioEchoFilterMembers.wetMix;
        }

        public override void Undo()
        {
            Target.wetMix = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            Target.wetMix = InitialValue + (t * diff);
        }

    }

    public class AudioEchoFilterCmdEnabled : AudioCommand<AudioEchoFilter>, IAudioCommand<bool>
    {

        public bool InitialValue { get; private set; }
        public bool FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioEchoFilterCmdEnabled(AudioEchoFilter target)
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
            return (int)AudioEchoFilterMembers.enabled;
        }

        public override void Undo()
        {
            Target.enabled = InitialValue;
        }

    }

    #endregion
}


