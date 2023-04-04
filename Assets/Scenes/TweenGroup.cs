using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeRatDev
{
    [Serializable]
    public class TweenGroup : ITween
    {
        private struct TweenGroupElement
        {
            public ITween tween;
            public float startGroupDelay;
        }
        private List<TweenGroupElement> tweens;
        private uint runningTweens = 0;

        public MonoBehaviour Owner { get => owner; }

        [Header("Basic Settings")]
        [SerializeField] protected MonoBehaviour owner;
        [SerializeField] protected float delay;

        [Space(10)]

        [Header("Advanced Settings")]
        [SerializeField] protected bool ignoreTimeScale;
        [SerializeField] protected bool runEndEventWhenCancelled;

        protected Action OnStart;
        protected Action OnDelayEnd;
        protected Action OnEnd;

        protected Coroutine routine;
        public bool IsRunning { get => routine != null; }

        public TweenGroup(MonoBehaviour owner)
        {
            this.tweens = new List<TweenGroupElement>();

            this.owner = owner;
            this.delay = 0f;

            this.ignoreTimeScale = false;
            this.runEndEventWhenCancelled = false;

            this.OnStart = null;
            this.OnDelayEnd = null;
            this.OnEnd = null;

            this.routine = null;
        }

        public void AddTween(ITween tween, float startGroupDelay)
        {
            if (tween.Owner != this.owner) return;

            if (this.tweens == null) this.tweens = new List<TweenGroupElement>();

            int index = tweens.FindIndex(tweenElement => tweenElement.tween == tween);
            if (index != -1) tweens.RemoveAt(index);

            TweenGroupElement tweenGroupElement;
            tweenGroupElement.tween = tween;
            tweenGroupElement.startGroupDelay = startGroupDelay;

            for (index = 0; index < tweens.Count; index++)
            {
                if (startGroupDelay > tweens[index].startGroupDelay) continue;
                break;
            }
            tweens.Insert(index, tweenGroupElement);
        }

        public void RemoveTween(ITween tween)
        {
            int index = tweens.FindIndex(tweenElement => tweenElement.tween == tween);
            if (index != -1) tweens.RemoveAt(index);
        }

        public void SetDelay(float delay)
        {
            this.delay = delay;
        }

        public void SetIgnoreTimeScale(bool ignoreTimeScale)
        {
            foreach (TweenGroupElement tweenGroupElement in tweens)
            {
                tweenGroupElement.tween.SetIgnoreTimeScale(ignoreTimeScale);
            }
            this.ignoreTimeScale = ignoreTimeScale;
        }

        public void SetRunEndEventWhenCancelled(bool runEndEventWhenCancelled)
        {
            foreach (TweenGroupElement tweenGroupElement in tweens)
            {
                tweenGroupElement.tween.SetRunEndEventWhenCancelled(runEndEventWhenCancelled);
            }
            this.runEndEventWhenCancelled = runEndEventWhenCancelled;
        }

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

        public void Run()
        {
            if (IsRunning) owner.StopCoroutine(routine);

            routine = owner.StartCoroutine(RunRoutine());
        }

        public void Stop()
        {
            if (!IsRunning) return;

            owner.StopCoroutine(routine);
            routine = null;

            foreach (TweenGroupElement tweenGroupElement in tweens)
            {
                tweenGroupElement.tween.UnsubscribeFromEndEvent(DecreaseNumberOfRunningTweens);
                tweenGroupElement.tween.Stop();
            }

            if (runEndEventWhenCancelled) OnEnd?.Invoke();
        }

        private void DecreaseNumberOfRunningTweens()
        {
            runningTweens--;
        }

        protected IEnumerator RunRoutine()
        {
            float currentTime = ignoreTimeScale ? Time.unscaledTime : Time.time;
            float startTime = currentTime;

            OnStart?.Invoke();

            // Wait for delay
            while (currentTime < startTime + delay)
            {
                yield return null;

                currentTime = ignoreTimeScale ? Time.unscaledTime : Time.time;
            }

            if (delay > 0f) OnDelayEnd?.Invoke();

            // Evaluate interpolation over time
            runningTweens = 0;
            int i = 0;
            while (i < tweens.Count)
            {
                if (currentTime < tweens[i].startGroupDelay)
                {
                    yield return null;

                    currentTime = ignoreTimeScale ? Time.unscaledTime : Time.time;
                }
                else
                {
                    runningTweens++;
                    tweens[i].tween.SubscribeToEndEvent(DecreaseNumberOfRunningTweens);
                    tweens[i].tween.Run();
                    i++;
                }
            }

            while (runningTweens > 0)
            {
                yield return null;
            }

            OnEnd?.Invoke();

            routine = null;
        }
    }
}
