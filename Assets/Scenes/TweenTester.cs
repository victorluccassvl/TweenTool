using UnityEngine;
using UnityEditor.SceneManagement;
using LeRatDev;
using TMPro;

public class TweenTester : MonoBehaviour
{
    public TextMeshProUGUI debugText;

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

        positionTween.SubscribeToStartEvent(() => debugText.text += "<color=\"blue\">" + "Start : Position" + "</color>\n");
        positionTween.SubscribeToDelayEndEvent(() => debugText.text += "<color=#00AA00>" + "Delay End : Position" + "</color>\n");
        positionTween.SubscribeToEndEvent(() => debugText.text += "<color=\"red\">" + "End : Position" + "</color>\n");

        localPositionTween.SubscribeToStartEvent(() => debugText.text += "<color=\"blue\">" + "Start : Local Position" + "</color>\n");
        localPositionTween.SubscribeToDelayEndEvent(() => debugText.text += "<color=#00AA00>" + "Delay End : Local Position" + "</color>\n");
        localPositionTween.SubscribeToEndEvent(() => debugText.text += "<color=\"red\">" + "End : Local Position" + "</color>\n");

        rotationTween.SubscribeToStartEvent(() => debugText.text += "<color=\"blue\">" + "Start : Rotation" + "</color>\n");
        rotationTween.SubscribeToDelayEndEvent(() => debugText.text += "<color=#00AA00>" + "Delay End : Rotation" + "</color>\n");
        rotationTween.SubscribeToEndEvent(() => debugText.text += "<color=\"red\">" + "End : Rotation" + "</color>\n");

        localRotationTween.SubscribeToStartEvent(() => debugText.text += "<color=\"blue\">" + "Start : Local Rotation" + "</color>\n");
        localRotationTween.SubscribeToDelayEndEvent(() => debugText.text += "<color=#00AA00>" + "Delay End : Local Rotation" + "</color>\n");
        localRotationTween.SubscribeToEndEvent(() => debugText.text += "<color=\"red\">" + "End : Local Rotation" + "</color>\n");

        scaleTween.SubscribeToStartEvent(() => debugText.text += "<color=\"blue\">" + "Start : Scale\n</color>");
        scaleTween.SubscribeToDelayEndEvent(() => debugText.text += "<color=#00AA00>" + "Delay End : Scale" + "</color>\n");
        scaleTween.SubscribeToEndEvent(() => debugText.text += "<color=\"red\">" + "End : Scale" + "</color>\n");

        positionLoopTween.SubscribeToStartEvent(() => debugText.text += "<color=\"blue\">" + "Start : Loop Position" + "</color>\n");
        positionLoopTween.SubscribeToDelayEndEvent(() => debugText.text += "<color=#00AA00>" + "Delay End : Loop Position" + "</color>\n");
        positionLoopTween.SubscribeToEachLoopEnd((i) => debugText.text += "<color=\"purple\">" + "Loop [" + i + "] End : Loop Position" + "</color>\n");
        positionLoopTween.SubscribeToEndEvent(() => debugText.text += "<color=\"red\">" + "End : Loop Position" + "</color>\n");

        tweenGroup.AddTween(positionTween, 0f);
        tweenGroup.AddTween(rotationTween, 0f);
        tweenGroup.AddTween(scaleTween, 0f);
        tweenGroup.AddTween(positionLoopTween, 0f);
        tweenGroup.SubscribeToStartEvent(() => debugText.text += "<color=\"black\">" + "Start : Group" + "</color>\n");
        tweenGroup.SubscribeToDelayEndEvent(() => debugText.text += "<color=\"black\">" + "Delay End : Group" + "</color>\n");
        tweenGroup.SubscribeToEndEvent(() => debugText.text += "<color=\"black\">" + "End : Group" + "</color>\n");
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
            debugText.text = "";
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