

namespace Audiomata.ComponentTrackers
{
    using Audiomata;
    //-------------------------------------------------------------------------------------------------------------------------------
    //					THIS CODE IS AUTOMATICALLY GENERATED, DO NOT MODIFY
    //-------------------------------------------------------------------------------------------------------------------------------
    using UnityEngine;

    // Auto generated props, can use reflections but don't because it is REALLY slow
    #region EnumeratedProps
    public enum AudioListenerMembers
    {
        enabled
        , volume
            , pause
            , velocityUpdateMode

    }
    #endregion

    #region CommmandManager
    //Class to manage state of components
    public class AudioListenerCommander : IAudioCommandable
    {
        private LimitedStack<AudioCommand<AudioListener>> audioCommands;
        public AudioListener Target { get; private set; }

        public AudioListenerCommander()
        {
            audioCommands = new LimitedStack<AudioCommand<AudioListener>>();
        }

        public object DoCommand<T>(T value, int enumeratedProp)
        {
            AudioCommand<AudioListener> newCommand = CommandFactory(enumeratedProp);
            newCommand.Do(value);
            audioCommands.Push(newCommand);
            return newCommand;
        }

        public object RegisterCommand(int enumeratedProp)
        {
            AudioCommand<AudioListener> newCommand = CommandFactory(enumeratedProp);
            audioCommands.Push(newCommand);
            return newCommand;
        }

        public void UndoCommand(object cmd)
        {
            AudioCommand<AudioListener> command = (AudioCommand<AudioListener>)cmd;
            command.Undo();
            audioCommands.Remove(command);
        }


        public void UndoLast()
        {
            if (audioCommands.Count < 1)
            {
                return;
            }

            AudioCommand<AudioListener> popped = audioCommands.Pop();
            popped.Undo();
        }

        public void UndoAll()
        {
            while (audioCommands.Count > 0)
            {
                AudioCommand<AudioListener> popped = audioCommands.Pop();
                popped.Undo();
            }
        }

        public void ClearChangeHistory()
        {
            while (audioCommands.Count > 0)
            {
                AudioCommand<AudioListener> popped = audioCommands.Pop();
            }
        }

        private AudioCommand<AudioListener> CommandFactory(int enumeratedProp)
        {
            AudioListenerMembers commandPropTarget = (AudioListenerMembers)enumeratedProp;
            AudioCommand<AudioListener> nextCommand;
            switch (commandPropTarget)
            {
                case AudioListenerMembers.volume:
                    nextCommand = new AudioListenerCmdVolume(Target);
                    return nextCommand;
                case AudioListenerMembers.pause:
                    nextCommand = new AudioListenerCmdPause(Target);
                    return nextCommand;
                case AudioListenerMembers.velocityUpdateMode:
                    nextCommand = new AudioListenerCmdVelocityUpdateMode(Target);
                    return nextCommand;
                case AudioListenerMembers.enabled:
                    nextCommand = new AudioListenerCmdEnabled(Target);
                    return nextCommand;
                default:
                    return null;
            }
        }
    }

    #endregion

    #region AutoGeneratedCommands
    public class AudioListenerCmdVolume : AudioCommand<AudioListener>, IAudioCommand<float>
    {

        public float InitialValue { get; private set; }
        public float FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioListenerCmdVolume(AudioListener target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = AudioListener.volume;
            FinalValue = (float)newValue;
            AudioListener.volume = FinalValue;
        }

        public void Do(float newValue)
        {
            InitialValue = AudioListener.volume;
            FinalValue = newValue;
            AudioListener.volume = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioListenerMembers.volume;
        }

        public override void Undo()
        {
            AudioListener.volume = InitialValue;
        }

        public override void Step(float t)
        {
            float diff = FinalValue - InitialValue;
            AudioListener.volume = InitialValue + (t * diff);
        }

    }

    public class AudioListenerCmdPause : AudioCommand<AudioListener>, IAudioCommand<bool>
    {

        public bool InitialValue { get; private set; }
        public bool FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioListenerCmdPause(AudioListener target)
        {
            Target = target;
        }

        public override void Do(object newValue)
        {
            InitialValue = AudioListener.pause;
            FinalValue = (bool)newValue;
            AudioListener.pause = FinalValue;
        }

        public void Do(bool newValue)
        {
            InitialValue = AudioListener.pause;
            FinalValue = newValue;
            AudioListener.pause = FinalValue;
        }

        public int TargetPropEnum()
        {
            return (int)AudioListenerMembers.pause;
        }

        public override void Undo()
        {
            AudioListener.pause = InitialValue;
        }

    }

    public class AudioListenerCmdVelocityUpdateMode : AudioCommand<AudioListener>, IAudioCommand<AudioVelocityUpdateMode>
    {

        public AudioVelocityUpdateMode InitialValue { get; private set; }
        public AudioVelocityUpdateMode FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioListenerCmdVelocityUpdateMode(AudioListener target)
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
            return (int)AudioListenerMembers.velocityUpdateMode;
        }

        public override void Undo()
        {
            Target.velocityUpdateMode = InitialValue;
        }

    }

    public class AudioListenerCmdEnabled : AudioCommand<AudioListener>, IAudioCommand<bool>
    {

        public bool InitialValue { get; private set; }
        public bool FinalValue { get; private set; }
        public CommandState CommandState { get; set; }


        public AudioListenerCmdEnabled(AudioListener target)
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
            return (int)AudioListenerMembers.enabled;
        }

        public override void Undo()
        {
            Target.enabled = InitialValue;
        }

    }

    #endregion
}


