using System;
using UnityEngine;

[Serializable]
public class RotationTween : Tween<Transform, Quaternion>
{
    public RotationTween(MonoBehaviour monoBehaviour, Transform entity, Quaternion initialValue, Quaternion targetValue, float duration) : base(monoBehaviour, entity, initialValue, targetValue, duration) { }
    public RotationTween(MonoBehaviour monoBehaviour, Transform entity, Vector3 initialValue, Vector3 targetValue, float duration) : base(monoBehaviour, entity, Quaternion.Euler(initialValue), Quaternion.Euler(targetValue), duration) { }

    protected override void Evaluate(float? progressOverride = null)
    {
        float t = progressOverride != null ? progressOverride.Value : progressWithCurveApplied;
        entity.rotation = Quaternion.SlerpUnclamped(initialValue, targetValue, t);
    }
}

[Serializable]
public class LocalRotationTween : Tween<Transform, Quaternion>
{
    public LocalRotationTween(MonoBehaviour monoBehaviour, Transform entity, Quaternion initialValue, Quaternion targetValue, float duration) : base(monoBehaviour, entity, initialValue, targetValue, duration) { }
    public LocalRotationTween(MonoBehaviour monoBehaviour, Transform entity, Vector3 initialValue, Vector3 targetValue, float duration) : base(monoBehaviour, entity, Quaternion.Euler(initialValue), Quaternion.Euler(targetValue), duration) { }

    protected override void Evaluate(float? progressOverride = null)
    {
        float t = progressOverride != null ? progressOverride.Value : progressWithCurveApplied;
        entity.localRotation = Quaternion.SlerpUnclamped(initialValue, targetValue, t);
    }
}