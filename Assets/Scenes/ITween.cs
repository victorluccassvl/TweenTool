using System;
using UnityEngine;

namespace LeRatDev
{
    public interface ITween
    {
        public MonoBehaviour Owner { get; }

        public void Run();
        public void Stop();

        public void SetDelay(float delay);
        public void SetIgnoreTimeScale(bool ignoreTimeScale);
        public void SetRunEndEventWhenCancelled(bool runEndEventWhenCancelled);

        public void SubscribeToEndEvent(Action endAction);
        public void UnsubscribeFromEndEvent(Action endAction);
        public void SubscribeToStartEvent(Action startAction);
        public void UnsubscribeFromStartEvent(Action startAction);
        public void SubscribeToDelayEndEvent(Action delayEndAction);
        public void UnsubscribeFromDelayEndEvent(Action delayEndAction);

        public bool IsRunning { get; }
    }
}
