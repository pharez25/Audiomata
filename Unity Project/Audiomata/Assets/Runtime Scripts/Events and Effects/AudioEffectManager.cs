using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Audiomata.ComponentTrackers;
using System;

namespace Audiomata
{
    [RequireComponent(typeof(AudioManager))]
    public class AudioEffectManager : MonoBehaviour
    {
        Dictionary<int, IAudioCommandable> curentTrackedComps;

        List<IAudioCommand<float>> activeFloatCommands;
        List<IAudioCommand<int>> activeIntCommands;
        List<Duration> activeIntDurations;
        List<Duration> activeFloatDurations;

        public static AudioEffectManager Instace { get; private set; }

        private WaitForEndOfFrame frameDelayObj;

        public bool HasActiveCommands { get; private set; } = false;

        private void Awake()
        {
            if (!Instace)
            {
                Instace = this;
            }
            else
            {
                Debug.LogError("2 Effect Managers are present on this scene");
                Destroy(gameObject);
                return;
            }
            curentTrackedComps = new Dictionary<int, IAudioCommandable>();
            frameDelayObj = new WaitForEndOfFrame();
            activeFloatCommands = new List<IAudioCommand<float>>();
            activeIntCommands = new List<IAudioCommand<int>>();
            activeIntDurations = new List<Duration>();
            activeFloatDurations = new List<Duration>();
        }

        public bool RemoveAndClearTracking(Component trackedTarget, bool revert = false)
        {
            int instanceId = trackedTarget.GetInstanceID();
            if (curentTrackedComps.TryGetValue(instanceId, out IAudioCommandable tracker))
            {
                if (revert)
                {
                    tracker.UndoAll();
                }
                else
                {
                    tracker.ClearChangeHistory();
                }
                curentTrackedComps.Remove(instanceId);
                return true;
            }
             return false;
        }
       
        IEnumerator IncrementActiveCommands()
        {
            HasActiveCommands = true;

            while ((activeFloatCommands.Count >0 || activeIntCommands.Count > 0 ) &enabled)
            {
                yield return frameDelayObj;

                for (int i = activeIntCommands.Count; i >-1; i--)
                {
                    IAudioCommand<int> nextCmd =  activeIntCommands[i];
                    Duration currentDuration = activeIntDurations[i];
                    currentDuration.current +=  Time.time;

                    if (currentDuration.current>= currentDuration.total)
                    {
                        currentDuration.current = currentDuration.total;
                        nextCmd.CommandState = CommandState.Done;
                        activeIntCommands.RemoveAt(i);
                        activeIntCommands.RemoveAt(i);
                    }
                    nextCmd.Step(currentDuration.current / currentDuration.total);
                }

                for (int i = activeFloatCommands.Count; i > -1; i--)
                {
                    IAudioCommand<float> nextCmd = activeFloatCommands[i];
                    Duration currentDuration = activeIntDurations[i];
                    currentDuration.current += Time.time;

                    if (currentDuration.current >= currentDuration.total)
                    {
                        currentDuration.current = currentDuration.total;
                        nextCmd.CommandState = CommandState.Done;
                        activeIntCommands.RemoveAt(i);
                        activeIntCommands.RemoveAt(i);
                    }
                    nextCmd.Step(currentDuration.current / currentDuration.total);
                }
            }

            HasActiveCommands = false;
            
        }

        public IAudioCommandable GetTrackedComponent(Component target) => curentTrackedComps[target.GetInstanceID()];

        
        public object DoCommand<T>(Component target, int enumeratedProp, T value)
        {
            IAudioCommandable commandable = GetTrackedComponent(target);
            
            return commandable.DoCommand<T>(value, enumeratedProp);
        }

        public void UndoLast(Component target) => GetTrackedComponent(target).UndoLast();
        public void UndoAll(Component target) => GetTrackedComponent(target).UndoAll();

        private void OnEnable()
        {
            if (HasActiveCommands)
            {
                StartCoroutine(IncrementActiveCommands());
            }
        }


        private void OnDisable()
        {
            StopAllCoroutines();
        }


        private struct Duration
        {
            public float total;
            public float current;

        }

        

        #region CommandOverTimeCreation

        public void CommandOverTime(AudioChorusFilter target, int enumeratedProp, float targetValue, float duration)
        {
            AudioChorusFilterCommander commandable = (AudioChorusFilterCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<float> command = (IAudioCommand<float>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeFloatDurations.Add(new Duration() { current = 0, total = duration });
            activeFloatCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }

        public void CommandOverTime(AudioChorusFilter target, int enumeratedProp, int targetValue, float duration)
        {
            AudioChorusFilterCommander commandable = (AudioChorusFilterCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<int> command = (IAudioCommand<int>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeIntDurations.Add(new Duration() { current = 0, total = duration });
            activeIntCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }


        public void CommandOverTime(AudioListener target, int enumeratedProp, int targetValue, float duration)
        {
            AudioListenerCommander commandable = (AudioListenerCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<int> command = (IAudioCommand<int>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeIntDurations.Add(new Duration() { current = 0, total = duration });
            activeIntCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }

        public void CommandOverTime(AudioListener target, int enumeratedProp, float targetValue, float duration)
        {
            AudioListenerCommander commandable = (AudioListenerCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<float> command = (IAudioCommand<float>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeFloatDurations.Add(new Duration() { current = 0, total = duration });
            activeFloatCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }


        public void CommandOverTime(AudioSource target, int enumeratedProp, int targetValue, float duration)
        {
            AudioSourceCommander commandable = (AudioSourceCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<int> command = (IAudioCommand<int>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeIntDurations.Add(new Duration() { current = 0, total = duration });
            activeIntCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }

        public void CommandOverTime(AudioSource target, int enumeratedProp, float targetValue, float duration)
        {
            AudioSourceCommander commandable = (AudioSourceCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<float> command = (IAudioCommand<float>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeFloatDurations.Add(new Duration() { current = 0, total = duration });
            activeFloatCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }


        public void CommandOverTime(AudioLowPassFilter target, int enumeratedProp, int targetValue, float duration)
        {
            AudioLowPassFilterCommander commandable = (AudioLowPassFilterCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<int> command = (IAudioCommand<int>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeIntDurations.Add(new Duration() { current = 0, total = duration });
            activeIntCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }

        public void CommandOverTime(AudioLowPassFilter target, int enumeratedProp, float targetValue, float duration)
        {
            AudioLowPassFilterCommander commandable = (AudioLowPassFilterCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<float> command = (IAudioCommand<float>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeFloatDurations.Add(new Duration() { current = 0, total = duration });
            activeFloatCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }


        public void CommandOverTime(AudioDistortionFilter target, int enumeratedProp, int targetValue, float duration)
        {
            AudioDistortionFilterCommander commandable = (AudioDistortionFilterCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<int> command = (IAudioCommand<int>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeIntDurations.Add(new Duration() { current = 0, total = duration });
            activeIntCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }

        public void CommandOverTime(AudioDistortionFilter target, int enumeratedProp, float targetValue, float duration)
        {
            AudioDistortionFilterCommander commandable = (AudioDistortionFilterCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<float> command = (IAudioCommand<float>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeFloatDurations.Add(new Duration() { current = 0, total = duration });
            activeFloatCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }


        public void CommandOverTime(AudioEchoFilter target, int enumeratedProp, int targetValue, float duration)
        {
            AudioEchoFilterCommander commandable = (AudioEchoFilterCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<int> command = (IAudioCommand<int>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeIntDurations.Add(new Duration() { current = 0, total = duration });
            activeIntCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }

        public void CommandOverTime(AudioEchoFilter target, int enumeratedProp, float targetValue, float duration)
        {
            AudioEchoFilterCommander commandable = (AudioEchoFilterCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<float> command = (IAudioCommand<float>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeFloatDurations.Add(new Duration() { current = 0, total = duration });
            activeFloatCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }


        public void CommandOverTime(AudioHighPassFilter target, int enumeratedProp, int targetValue, float duration)
        {
            AudioHighPassFilterCommander commandable = (AudioHighPassFilterCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<int> command = (IAudioCommand<int>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeIntDurations.Add(new Duration() { current = 0, total = duration });
            activeIntCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }

        public void CommandOverTime(AudioHighPassFilter target, int enumeratedProp, float targetValue, float duration)
        {
            AudioHighPassFilterCommander commandable = (AudioHighPassFilterCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<float> command = (IAudioCommand<float>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeFloatDurations.Add(new Duration() { current = 0, total = duration });
            activeFloatCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }


        public void CommandOverTime(AudioReverbZone target, int enumeratedProp, int targetValue, float duration)
        {
            AudioReverbZoneCommander commandable = (AudioReverbZoneCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<int> command = (IAudioCommand<int>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeIntDurations.Add(new Duration() { current = 0, total = duration });
            activeIntCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }

        public void CommandOverTime(AudioReverbZone target, int enumeratedProp, float targetValue, float duration)
        {
            AudioReverbZoneCommander commandable = (AudioReverbZoneCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<float> command = (IAudioCommand<float>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeFloatDurations.Add(new Duration() { current = 0, total = duration });
            activeFloatCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }

        public void CommandOverTime(AudioReverbFilter target, int enumeratedProp, int targetValue, float duration)
        {
            AudioReverbFilterCommander commandable = (AudioReverbFilterCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<int> command = (IAudioCommand<int>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeIntDurations.Add(new Duration() { current = 0, total = duration });
            activeIntCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }

        public void CommandOverTime(AudioReverbFilter target, int enumeratedProp, float targetValue, float duration)
        {
            AudioReverbFilterCommander commandable = (AudioReverbFilterCommander)curentTrackedComps[target.GetInstanceID()];

            IAudioCommand<float> command = (IAudioCommand<float>)commandable.CreateCommand(enumeratedProp);

            command.CommandState = CommandState.Doing;

            command.FinalValue = targetValue;
            activeFloatDurations.Add(new Duration() { current = 0, total = duration });
            activeFloatCommands.Add(command);

            if (!HasActiveCommands)
            {
                HasActiveCommands = true;
                StartCoroutine(IncrementActiveCommands());
            }

        }

        #endregion

        //decided to take this approach as the type will always need to be known and it avoids boxing. 
        #region CommanderFactory
        public AudioChorusFilterCommander StartTrackingChorusFilter(AudioChorusFilter target)
        {
            AudioChorusFilterCommander filterCommander = new AudioChorusFilterCommander(target);
            curentTrackedComps[target.GetInstanceID()] = filterCommander;
            return filterCommander;
        }

        public AudioDistortionFilterCommander StartTrackingDistortionFilter(AudioDistortionFilter target)
        {
            AudioDistortionFilterCommander filterCommander = new AudioDistortionFilterCommander(target);
            curentTrackedComps[target.GetInstanceID()] = filterCommander;
            return filterCommander;
        }

        public AudioEchoFilterCommander StartTrackingEchoFilter(AudioEchoFilter target)
        {
            AudioEchoFilterCommander filterCommander = new AudioEchoFilterCommander(target);
            curentTrackedComps[target.GetInstanceID()] = filterCommander;
            return filterCommander;
        }

        public AudioHighPassFilterCommander StartTrackingHighPassFilter(AudioHighPassFilter target)
        {
            AudioHighPassFilterCommander filterCommander = new AudioHighPassFilterCommander(target);
            curentTrackedComps[target.GetInstanceID()] = filterCommander;
            return filterCommander;
        }

        public AudioLowPassFilterCommander StartTrackingLowPass (AudioLowPassFilter target)
        {
            AudioLowPassFilterCommander filterCommander = new AudioLowPassFilterCommander(target);
            curentTrackedComps[target.GetInstanceID()] = filterCommander;
            return filterCommander;
        }

        public AudioReverbFilterCommander StartTrackingReverbFilter (AudioReverbFilter target)
        {
            AudioReverbFilterCommander filterCommander = new AudioReverbFilterCommander(target);
            curentTrackedComps[target.GetInstanceID()] = filterCommander;
            return filterCommander;
        }

        public AudioSourceCommander StartTrackingSource(AudioSource target)
        {
            AudioSourceCommander commander = new AudioSourceCommander(target);
            curentTrackedComps[target.GetInstanceID()] = commander;
            return commander;
        }

        public AudioListenerCommander StartTrackingListener(AudioListener target)
        {
            AudioListenerCommander commander = new AudioListenerCommander(target);
            curentTrackedComps[target.GetInstanceID()] = commander;
            return commander;
        }

        public AudioReverbZoneCommander StartTrackingReverbZone(AudioReverbZone target)
        {
            AudioReverbZoneCommander commander = new AudioReverbZoneCommander(target);
            curentTrackedComps[target.GetInstanceID()] = commander;
            return commander;
        }
        #endregion

    }

}
