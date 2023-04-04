using System;
using UnityEngine;

namespace LeRatDev
{
    [Serializable]
    public class PositionLoopTween : LoopTween<Transform, Vector3>
    {
        public PositionLoopTween(uint? loopAmount, MonoBehaviour monoBehaviour, Transform entity, Vector3 initialValue, Vector3 targetValue, float duration) : base(loopAmount, monoBehaviour, entity, initialValue, targetValue, duration) { }

        protected override void Evaluate(float? progressOverride = null)
        {
            float t = progressOverride != null ? progressOverride.Value : progressWithCurveApplied;
            entity.position = (1f - t) * initialValue + (t) * targetValue;
        }
    }

    [Serializable]
    public class LocalPositionLoopTween : LoopTween<Transform, Vector3>
    {
        public LocalPositionLoopTween(uint? loopAmount, MonoBehaviour monoBehaviour, Transform entity, Vector3 initialValue, Vector3 targetValue, float duration) : base(loopAmount, monoBehaviour, entity, initialValue, targetValue, duration) { }

        protected override void Evaluate(float? progressOverride = null)
        {
            float t = progressOverride != null ? progressOverride.Value : progressWithCurveApplied;
            entity.localPosition = (1f - t) * initialValue + (t) * targetValue;
        }
    }
}