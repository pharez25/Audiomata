﻿using System;

namespace Audiomata.ComponentTrackers
{
    public abstract class AudioCommand<T>
    {
        public virtual T Target { get; protected set; }
        CommandState CommandState { get; }
        public virtual IAudioCommand<T> GetInterface()
        {
            return (IAudioCommand<T>)this;
        }
        public abstract void Do(object newValue);
        public abstract void Undo();
        public virtual void Step(float t = 1)
        {
            throw new System.NotImplementedException();
        }
    }
}