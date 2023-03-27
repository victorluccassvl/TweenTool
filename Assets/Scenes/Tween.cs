using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class Tween<T, U>
{
    [SerializeField] protected MonoBehaviour monoBehaviour;
    [SerializeField] protected T entity;
    [SerializeField] protected U initialValue;
    [SerializeField] protected U targetValue;
    [SerializeField] protected float duration;
    [SerializeField] protected float delay;

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

    public Tween(MonoBehaviour monoBehaviour, T entity, U initialValue, U targetValue, float duration)
    {
        this.monoBehaviour = monoBehaviour;
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
    public void UnsubscribeToEndEvent(Action endAction)
    {
        OnEnd -= endAction;
    }
    public void SubscribeToStartEvent(Action startAction)
    {
        OnStart -= startAction;
        OnStart += startAction;
    }
    public void UnsubscribeToStartEvent(Action startAction)
    {
        OnStart -= startAction;
    }
    public void SubscribeToDelayEndEvent(Action delayEndAction)
    {
        OnDelayEnd -= delayEndAction;
        OnDelayEnd += delayEndAction;
    }
    public void UnsubscribeToDelayEndEvent(Action delayEndAction)
    {
        OnDelayEnd -= delayEndAction;
    }

    protected abstract void Evaluate(float? progressOverride = null);

    public void Run()
    {
        if (IsRunning) monoBehaviour.StopCoroutine(routine);

        routine = monoBehaviour.StartCoroutine(RunRoutine());
    }

    public void Stop()
    {
        if (!IsRunning) return;

        monoBehaviour.StopCoroutine(routine);
        routine = null;

        if (runEndEventWhenCancelled) OnEnd?.Invoke();
    }

    private IEnumerator RunRoutine()
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

public struct LoopConfig
{
    public uint loopAmount;
    public bool executeEndEventsEveryLoop;
    public bool executeStartEventsEveryLoop;
    public bool applyDelayEveryLoop;
    public Action OnEnd;
    public Action OnStart;

    public bool RunInLoop()
    {
        return loopAmount > 0;
    }
}
