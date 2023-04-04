using System;
using UnityEngine;

namespace LeRatDev
{
    [Serializable]
    public class ScaleTween : Tween<Transform, Vector3>
    {
        public ScaleTween(MonoBehaviour monoBehaviour, RectTransform entity, Vector3 initialValue, Vector3 targetValue, float duration) : base(monoBehaviour, entity, initialValue, targetValue, duration) { }

        protected override void Evaluate(float? progressOverride = null)
        {
            float t = progressOverride != null ? progressOverride.Value : progressWithCurveApplied;
            entity.localScale = (1f - t) * initialValue + (t) * targetValue;
        }
    }
}