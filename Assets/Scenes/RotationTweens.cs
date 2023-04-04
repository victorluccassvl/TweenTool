using System;
using UnityEngine;

namespace LeRatDev
{
    [Serializable]
    public class RotationTween : Tween<Transform, Vector3>
    {
        public RotationTween(MonoBehaviour monoBehaviour, Transform entity, Quaternion initialValue, Quaternion targetValue, float duration) : base(monoBehaviour, entity, initialValue.eulerAngles, targetValue.eulerAngles, duration) { }
        public RotationTween(MonoBehaviour monoBehaviour, Transform entity, Vector3 initialValue, Vector3 targetValue, float duration) : base(monoBehaviour, entity, initialValue, targetValue, duration) { }

        protected override void Evaluate(float? progressOverride = null)
        {
            float t = progressOverride != null ? progressOverride.Value : progressWithCurveApplied;
            entity.rotation = Quaternion.SlerpUnclamped(Quaternion.Euler(initialValue), Quaternion.Euler(targetValue), t);
        }
    }

    [Serializable]
    public class LocalRotationTween : Tween<Transform, Vector3>
    {
        public LocalRotationTween(MonoBehaviour monoBehaviour, Transform entity, Quaternion initialValue, Quaternion targetValue, float duration) : base(monoBehaviour, entity, initialValue.eulerAngles, targetValue.eulerAngles, duration) { }
        public LocalRotationTween(MonoBehaviour monoBehaviour, Transform entity, Vector3 initialValue, Vector3 targetValue, float duration) : base(monoBehaviour, entity, initialValue, targetValue, duration) { }

        protected override void Evaluate(float? progressOverride = null)
        {
            float t = progressOverride != null ? progressOverride.Value : progressWithCurveApplied;
            entity.localRotation = Quaternion.SlerpUnclamped(Quaternion.Euler(initialValue), Quaternion.Euler(targetValue), t);
        }
    }

}