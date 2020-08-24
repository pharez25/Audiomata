

namespace Audiomata.ComponentTrackers
{
//-------------------------------------------------------------------------------------------------------------------------------
//					THIS CODE IS AUTOMATICALLY GENERATED, DO NOT MODIFY
//-------------------------------------------------------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using Audiomata;

 // Auto generated props, can use reflections but don't because it is REALLY slow
#region EnumeratedProps
 public enum AudioMixerMembers
 {
	          updateMode
	
	 }
#endregion

#region CommmandManager
 //Class to manage state of components
 public class AudioMixerCommander :IAudioCommandable
 {
	private LimitedStack<AudioCommand<AudioMixer> > audioCommands;
    public AudioMixer Target{get;private set;}

    public object DoCommand<T>(T value, int enumeratedProp)
    {
        AudioCommand<AudioMixer> newCommand = CommandFactory(enumeratedProp);
        newCommand.Do(value);
        audioCommands.Push(newCommand);
        return newCommand;
    }
    
    public object RegisterCommand(int enumeratedProp)
    {
        AudioCommand<AudioMixer> newCommand = CommandFactory(enumeratedProp);
        audioCommands.Push(newCommand);
        return newCommand;
    }

    public void UndoCommand(object cmd)
    {
        AudioCommand<AudioMixer> command = (AudioCommand<AudioMixer>) cmd;
        command.Undo();
        audioCommands.Remove(command);
    }


    public void UndoLast()
    {
        if (audioCommands.Count<1)
        {
            return;
        }
        
        AudioCommand<AudioMixer> popped = audioCommands.Pop();
        popped.Undo();
    }

    public void UndoAll()
    {
        while (audioCommands.Count>0)
        {
            AudioCommand<AudioMixer> popped = audioCommands.Pop();
            popped.Undo();
        }
    }

    public void ClearChangeHistory()
    {
        while (audioCommands.Count>0)
        {
            AudioCommand<AudioMixer> popped = audioCommands.Pop();
        }
    }

    private AudioCommand<AudioMixer>CommandFactory(int enumeratedProp)
    {
        AudioMixerMembers commandPropTarget = ( AudioMixerMembers ) enumeratedProp;
            AudioCommand<AudioMixer> nextCommand;
        switch(commandPropTarget){
                    case AudioMixerMembers.outputAudioMixerGroup:
                nextCommand = new AudioMixerCmdOutputAudioMixerGroup(Target);
            return nextCommand;
               case AudioMixerMembers.updateMode:
                nextCommand = new AudioMixerCmdUpdateMode(Target);
            return nextCommand;
           default:
            return null; 
      }
    }
 }

#endregion

#region AutoGeneratedCommands
		public class AudioMixerCmdOutputAudioMixerGroup : AudioCommand< AudioMixer >,IAudioCommand< AudioMixerGroup >
{

	public AudioMixerGroup InitialValue { get; private set; }
	public AudioMixerGroup FinalValue { get; private set; }
	public CommandState CommandState {get;set;}
    

	public AudioMixerCmdOutputAudioMixerGroup(AudioMixer target){
		Target = target;
	}

	public override void Do(object newValue){
		InitialValue = Target.outputAudioMixerGroup;
		FinalValue = (AudioMixerGroup) newValue;
		Target.outputAudioMixerGroup = FinalValue;
	}

	public void Do(AudioMixerGroup newValue){
		InitialValue = Target.outputAudioMixerGroup;
		FinalValue = newValue;
		Target.outputAudioMixerGroup = FinalValue;
	}

	public int TargetPropEnum()
	{
		return (int)AudioMixerMembers.outputAudioMixerGroup;
	}

	public override void Undo(){
	Target.outputAudioMixerGroup = InitialValue;
	}
    
}
   
		public class AudioMixerCmdUpdateMode : AudioCommand< AudioMixer >,IAudioCommand< AudioMixerUpdateMode >
{

	public AudioMixerUpdateMode InitialValue { get; private set; }
	public AudioMixerUpdateMode FinalValue { get; private set; }
	public CommandState CommandState {get;set;}
    

	public AudioMixerCmdUpdateMode(AudioMixer target){
		Target = target;
	}

	public override void Do(object newValue){
		InitialValue = Target.updateMode;
		FinalValue = (AudioMixerUpdateMode) newValue;
		Target.updateMode = FinalValue;
	}

	public void Do(AudioMixerUpdateMode newValue){
		InitialValue = Target.updateMode;
		FinalValue = newValue;
		Target.updateMode = FinalValue;
	}

	public int TargetPropEnum()
	{
		return (int)AudioMixerMembers.updateMode;
	}

	public override void Undo(){
	Target.updateMode = InitialValue;
	}
    
}
   
	#endregion
}


