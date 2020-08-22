using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Audiomata.ComponentTrackers
{
    public interface IAudioCommandable
    {
        void SetPropValue<T>(T newValue, int enumeratedProp);
        void ClearChangeHistory();
        void UndoLast();
        void UndoAll();
    }
    

    public interface IAudioCommand<T>
    {

        void Do(T newValue);
        void Undo();
        int TargetPropEnum();
        T InitialValue { get; }
        T FinalValue { get; }
        CommandState CommandState { get; set; }
        void Step(float t);
     
    }

    public enum CommandState
    {
        Doing,Undoing,Done,Unapplied
    }

}
