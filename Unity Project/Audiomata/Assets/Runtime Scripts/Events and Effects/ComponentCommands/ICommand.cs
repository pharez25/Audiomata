using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Audiomata.ComponentTrackers
{
    public interface IAudioCommandable
    {
        //enum for type?
        object RegisterCommand(int enumeratedProp);
        object DoCommand<T>(T value, int enumeratedProp);//
        void ClearChangeHistory();//
        //Undoes the last command that was just done
        void UndoLast();//
        //Removes all Commands from the stack and undoes them;
        void UndoAll();//
        /// <summary>
        /// Undoes specified command, ignoring stack
        /// </summary>
        /// <typeparam name="T">Target Component</typeparam>
        /// <param name="command">command to be undone </param>
        void UndoCommand(object command);//
    }
    
    public interface ICommandLerpable
    {
        void Step(float t);
    }


    public interface IAudioCommand<T>:ICommandLerpable
    {

        void Do(T newValue);
        void Undo();
        int TargetPropEnum();
        T InitialValue { get; set; }
        T FinalValue { get; set; }
        CommandState CommandState { get; set; }
        new void Step(float t);
     
    }

    /// <summary>
    /// Command state for commands that are done over time, otherwise "NotUsed"
    /// </summary>
    public enum CommandState
    {
        Doing,Undoing,Done,Unapplied,NotUsed
    }

}
