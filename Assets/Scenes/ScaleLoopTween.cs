using System;
using UnityEngine;

namespace LeRatDev
{
    [Serializable]
    public class ScaleLoopTween : LoopTween<Transform, Vector3>
    {
        public ScaleLoopTween(uint? loopAmount, MonoBehaviour monoBehaviour, Transform entity, Vector3 initialValue, Vector3 targetValue, float duration) : base(loopAmount, monoBehaviour, entity, initialValue, targetValue, duration) { }

        protected override void Evaluate(float? progressOverride = null)
        {
            float t = progressOverride != null ? progressOverride.Value : progressWithCurveApplied;
            entity.localScale = (1f - t) * initialValue + (t) * targetValue;
        }
    }
}