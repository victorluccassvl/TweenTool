using System;
using UnityEngine;

namespace LeRatDev
{
    [Serializable]
    public class RotationLoopTween : LoopTween<Transform, Vector3>
    {
        public RotationLoopTween(uint? loopAmount, MonoBehaviour monoBehaviour, Transform entity, Quaternion initialValue, Quaternion targetValue, float duration) : base(loopAmount, monoBehaviour, entity, initialValue.eulerAngles, targetValue.eulerAngles, duration) { }

        public RotationLoopTween(uint? loopAmount, MonoBehaviour monoBehaviour, Transform entity, Vector3 initialValue, Vector3 targetValue, float duration) : base(loopAmount, monoBehaviour, entity, initialValue, targetValue, duration) { }

        protected override void Evaluate(float? progressOverride = null)
        {
            float t = progressOverride != null ? progressOverride.Value : progressWithCurveApplied;
            entity.rotation = Quaternion.SlerpUnclamped(Quaternion.Euler(initialValue), Quaternion.Euler(targetValue), t);
        }
    }

    [Serializable]
    public class LocalRotationLoopTween : LoopTween<Transform, Vector3>
    {
        public LocalRotationLoopTween(uint? loopAmount, MonoBehaviour monoBehaviour, Transform entity, Quaternion initialValue, Quaternion targetValue, float duration) : base(loopAmount, monoBehaviour, entity, initialValue.eulerAngles, targetValue.eulerAngles, duration) { }

        public LocalRotationLoopTween(uint? loopAmount, MonoBehaviour monoBehaviour, Transform entity, Vector3 initialValue, Vector3 targetValue, float duration) : base(loopAmount, monoBehaviour, entity, initialValue, targetValue, duration) { }

        protected override void Evaluate(float? progressOverride = null)
        {
            float t = progressOverride != null ? progressOverride.Value : progressWithCurveApplied;
            entity.localRotation = Quaternion.SlerpUnclamped(Quaternion.Euler(initialValue), Quaternion.Euler(targetValue), t);
        }
    }
}