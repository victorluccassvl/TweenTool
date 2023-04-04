using System;
using System.Collections;
using UnityEngine;

namespace LeRatDev
{
    [Serializable]
    public abstract class LoopTween<T, U> : Tween<T, U> where T : UnityEngine.Object
    {
        [Space(10)]

        [Header("Loop Settings")]
        [SerializeField] protected uint loopAmount;

        protected bool LoopForever { get => loopAmount == 0; }

        protected Action<uint> OnEachLoopEnd;

        public LoopTween(uint? loopAmount, MonoBehaviour monoBehaviour, T entity, U initialValue, U targetValue, float duration) : base(monoBehaviour, entity, initialValue, targetValue, duration)
        {
            this.loopAmount = (loopAmount != null) ? loopAmount.Value : 0;

            this.OnEachLoopEnd = null;
        }

        public void SubscribeToEachLoopEnd(Action<uint> onEachLoopEnd)
        {
            OnEachLoopEnd -= onEachLoopEnd;
            OnEachLoopEnd += onEachLoopEnd;
        }
        public void UnsubscribeFromEachLoopEnd(Action<uint> onEachLoopEnd)
        {
            OnEachLoopEnd -= onEachLoopEnd;
        }

        protected override IEnumerator RunRoutine()
        {
            float currentTime = ignoreTimeScale ? Time.unscaledTime : Time.time;
            float startTime = currentTime;
            float endTime = delay + startTime + duration;
            OnStart?.Invoke();

            // Wait for delay
            while (currentTime < startTime + delay)
            {
                yield return null;

                currentTime = ignoreTimeScale ? Time.unscaledTime : Time.time;
            }

            if (delay > 0f) OnDelayEnd?.Invoke();

            // Evaluate interpolation over time for each loop
            for (uint loop = 0; loop < loopAmount || LoopForever; loop++)
            {
                currentTime = ignoreTimeScale ? Time.unscaledTime : Time.time;
                float loopStartTime = currentTime;
                float loopEndTime = loopStartTime + duration;

                progress = 0f;
                progressWithCurveApplied = interpolationCurve.Evaluate(progress);
                while (currentTime < loopEndTime)
                {
                    Evaluate();

                    yield return null;

                    currentTime = ignoreTimeScale ? Time.unscaledTime : Time.time;

                    progress = Mathf.Clamp01((currentTime - loopStartTime) / duration);
                    progressWithCurveApplied = interpolationCurve.Evaluate(progress);
                }
                Evaluate();

                OnEachLoopEnd?.Invoke(loop);
            }

            OnEnd?.Invoke();

            routine = null;
        }
    }
}
