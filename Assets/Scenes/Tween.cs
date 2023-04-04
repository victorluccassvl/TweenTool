using System;
using System.Collections;
using UnityEngine;

namespace LeRatDev
{
    [Serializable]
    public abstract class Tween<T, U> : ITween where T : UnityEngine.Object
    {
        public MonoBehaviour Owner { get => owner; }
        public UnityEngine.Object Entity { get => entity; }

        [Header("Basic Settings")]
        [SerializeField] protected MonoBehaviour owner;
        [SerializeField] protected T entity;
        [SerializeField] protected U initialValue;
        [SerializeField] protected U targetValue;
        [SerializeField] protected float duration;
        [SerializeField] protected float delay;

        [Space(10)]

        [Header("Advanced Settings")]
        [SerializeField] protected AnimationCurve interpolationCurve;
        [SerializeField] protected bool ignoreTimeScale;
        [SerializeField] protected bool runEndEventWhenCancelled;

        protected Action OnStart;
        protected Action OnDelayEnd;
        protected Action OnEnd;

        protected float progress;
        protected float progressWithCurveApplied;
        protected Coroutine routine;
        public bool IsRunning { get => routine != null; }

        public Tween(MonoBehaviour owner, T entity, U initialValue, U targetValue, float duration)
        {
            this.owner = owner;
            this.entity = entity;
            this.initialValue = initialValue;
            this.targetValue = targetValue;
            this.duration = Mathf.Max(0f, duration);
            this.delay = 0f;

            this.interpolationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            this.ignoreTimeScale = false;
            this.runEndEventWhenCancelled = false;

            this.OnStart = null;
            this.OnDelayEnd = null;
            this.OnEnd = null;

            this.progress = 0f;
            this.routine = null;
        }

        public void SetInterpolationCurve(AnimationCurve interpolationCurve) { this.interpolationCurve = interpolationCurve; }
        public void SetDelay(float delay) { this.delay = delay; }
        public void SetIgnoreTimeScale(bool ignoreTimeScale) { this.ignoreTimeScale = ignoreTimeScale; }
        public void SetRunEndEventWhenCancelled(bool runEndEventWhenCancelled) { this.runEndEventWhenCancelled = runEndEventWhenCancelled; }

        public void SubscribeToEndEvent(Action endAction)
        {
            OnEnd -= endAction;
            OnEnd += endAction;
        }
        public void UnsubscribeFromEndEvent(Action endAction)
        {
            OnEnd -= endAction;
        }
        public void SubscribeToStartEvent(Action startAction)
        {
            OnStart -= startAction;
            OnStart += startAction;
        }
        public void UnsubscribeFromStartEvent(Action startAction)
        {
            OnStart -= startAction;
        }
        public void SubscribeToDelayEndEvent(Action delayEndAction)
        {
            OnDelayEnd -= delayEndAction;
            OnDelayEnd += delayEndAction;
        }
        public void UnsubscribeFromDelayEndEvent(Action delayEndAction)
        {
            OnDelayEnd -= delayEndAction;
        }

        protected abstract void Evaluate(float? progressOverride = null);

        public void Run()
        {
            if (IsRunning) owner.StopCoroutine(routine);

            NormalizeInterpolationCurve();

            routine = owner.StartCoroutine(RunRoutine());
        }

        public void Stop()
        {
            if (!IsRunning) return;

            owner.StopCoroutine(routine);
            routine = null;

            if (runEndEventWhenCancelled) OnEnd?.Invoke();
        }

        protected void NormalizeInterpolationCurve()
        {
            float smallestTime = interpolationCurve.keys[0].time;
            float biggestTime = interpolationCurve.keys[interpolationCurve.keys.Length - 1].time;
            float difference = biggestTime - smallestTime;
            for (int i = 0; i < interpolationCurve.keys.Length; i++)
            {
                interpolationCurve.keys[i].time = (interpolationCurve.keys[i].time + smallestTime) / difference;
            }
        }

        protected virtual IEnumerator RunRoutine()
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

            // Evaluate interpolation over time
            progress = 0f;
            progressWithCurveApplied = interpolationCurve.Evaluate(progress);
            while (currentTime < endTime)
            {
                Evaluate();

                yield return null;

                currentTime = ignoreTimeScale ? Time.unscaledTime : Time.time;

                progress = Mathf.Clamp01((currentTime - startTime - delay) / duration);
                progressWithCurveApplied = interpolationCurve.Evaluate(progress);
            }
            Evaluate();

            OnEnd?.Invoke();

            routine = null;
        }
    }
}
