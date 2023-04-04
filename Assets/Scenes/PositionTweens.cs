using System;
using UnityEngine;

namespace LeRatDev
{
    [Serializable]
    public class PositionTween : Tween<Transform, Vector3>
    {
        public PositionTween(MonoBehaviour monoBehaviour, Transform entity, Vector3 initialValue, Vector3 targetValue, float duration) : base(monoBehaviour, entity, initialValue, targetValue, duration) { }

        protected override void Evaluate(float? progressOverride = null)
        {
            float t = progressOverride != null ? progressOverride.Value : progressWithCurveApplied;
            entity.position = (1f - t) * initialValue + (t) * targetValue;
        }
    }

    [Serializable]
    public class LocalPositionTween : Tween<Transform, Vector3>
    {
        public LocalPositionTween(MonoBehaviour monoBehaviour, Transform entity, Vector3 initialValue, Vector3 targetValue, float duration) : base(monoBehaviour, entity, initialValue, targetValue, duration) { }

        protected override void Evaluate(float? progressOverride = null)
        {
            float t = progressOverride != null ? progressOverride.Value : progressWithCurveApplied;
            entity.localPosition = (1f - t) * initialValue + (t) * targetValue;
        }
    }
}