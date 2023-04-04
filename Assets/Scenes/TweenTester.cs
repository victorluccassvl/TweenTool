using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using LeRatDev;

public class TweenTester : MonoBehaviour
{
    private ITween pickedTween = null;

    public PositionTween positionTween;
    public LocalPositionTween localPositionTween;
    public RotationTween rotationTween;
    public LocalRotationTween localRotationTween;
    public ScaleTween scaleTween;

    public PositionLoopTween positionLoopTween;
    public TweenGroup tweenGroup;

    public void Awake()
    {
        pickedTween = positionTween;

        positionTween.SubscribeToStartEvent(() => Debug.LogError("Position Tween : Started"));
        positionTween.SubscribeToDelayEndEvent(() => Debug.LogError("Position Tween : Delay Ended"));
        positionTween.SubscribeToEndEvent(() => Debug.LogError("Position Tween : Ended"));

        localPositionTween.SubscribeToStartEvent(() => Debug.LogError("Local Position Tween : Started"));
        localPositionTween.SubscribeToDelayEndEvent(() => Debug.LogError("Local Position Tween : Delay Ended"));
        localPositionTween.SubscribeToEndEvent(() => Debug.LogError("Local Position : Tween Ended"));

        rotationTween.SubscribeToStartEvent(() => Debug.LogError("Rotation Tween : Started"));
        rotationTween.SubscribeToDelayEndEvent(() => Debug.LogError("Rotation Tween : Delay Ended"));
        rotationTween.SubscribeToEndEvent(() => Debug.LogError("Rotation Tween : Ended"));

        localRotationTween.SubscribeToStartEvent(() => Debug.LogError("Local Rotation Tween : Started"));
        localRotationTween.SubscribeToDelayEndEvent(() => Debug.LogError("Local Rotation Tween : Delay Ended"));
        localRotationTween.SubscribeToEndEvent(() => Debug.LogError("Local Rotation : Tween Ended"));

        scaleTween.SubscribeToStartEvent(() => Debug.LogError("Scale Tween : Started"));
        scaleTween.SubscribeToDelayEndEvent(() => Debug.LogError("Scale Tween : Delay Ended"));
        scaleTween.SubscribeToEndEvent(() => Debug.LogError("Scale Tween : Ended"));

        positionLoopTween.SubscribeToStartEvent(() => Debug.LogError("Position Loop Tween : Started"));
        positionLoopTween.SubscribeToDelayEndEvent(() => Debug.LogError("Position Loop Tween : Delay Ended"));
        positionLoopTween.SubscribeToEachLoopEnd((loop) => Debug.LogError("Position Loop Tween : Loop " + loop + " Ended"));
        positionLoopTween.SubscribeToEndEvent(() => Debug.LogError("Position Loop Tween : Ended"));

        tweenGroup.AddTween(positionTween, 0f);
        tweenGroup.AddTween(rotationTween, 0f);
        tweenGroup.AddTween(scaleTween, 0f);
        tweenGroup.SubscribeToStartEvent(() => Debug.LogError("Group Tween : Started"));
        tweenGroup.SubscribeToDelayEndEvent(() => Debug.LogError("Group Tween : Delay Ended"));
        tweenGroup.SubscribeToEndEvent(() => Debug.LogError("Group Tween : Ended"));
    }

    public void OnGUI()
    {
        GUILayout.BeginVertical("Box");

        GUILayout.BeginVertical("Box");
        if (GUILayout.Toggle(pickedTween == positionTween, "Position")) pickedTween = positionTween;
        if (GUILayout.Toggle(pickedTween == localPositionTween, "Local Position")) pickedTween = localPositionTween;
        if (GUILayout.Toggle(pickedTween == rotationTween, "Rotation")) pickedTween = rotationTween;
        if (GUILayout.Toggle(pickedTween == localRotationTween, "Local Rotation")) pickedTween = localRotationTween;
        if (GUILayout.Toggle(pickedTween == scaleTween, "Scale")) pickedTween = scaleTween;
        if (GUILayout.Toggle(pickedTween == positionLoopTween, "Position Loop")) pickedTween = positionLoopTween;
        if (GUILayout.Toggle(pickedTween == tweenGroup, "Group")) pickedTween = tweenGroup;
        GUILayout.EndVertical();

        GUILayout.Space(20);

        GUILayout.BeginVertical("Box");
        if (GUILayout.Button("Run"))
        {
            pickedTween.Run();
        }
        if (GUILayout.Button("Stop"))
        {
            pickedTween.Stop();
        }
        if (GUILayout.Button("Reset"))
        {
            EditorSceneManager.LoadScene("Main", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        if (Time.timeScale == 0f)
        {
            if (GUILayout.Button("Time Scale = 0f"))
            {
                Time.timeScale = 1f;
            }
        }
        else
        {
            if (GUILayout.Button("Time Scale = 1f"))
            {
                Time.timeScale = 0f;
            }
        }
        GUILayout.EndVertical();

        GUILayout.EndVertical();
    }
}


