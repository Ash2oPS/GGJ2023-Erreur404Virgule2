using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CS_GameViewManager))]
public class CSE_GameViewManager : Editor
{
    private CS_GameViewManager _gvm;

    private GUIStyle _centeredStyle;

    private GUIStyle _singleStyle;
    private GUIStyle _averageStyle;

    private void OnEnable()
    {
        _gvm = (CS_GameViewManager)target;

        _centeredStyle = new GUIStyle()
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
        };
        _centeredStyle.normal.textColor = Color.white;

        _singleStyle = new GUIStyle(_centeredStyle);
        _singleStyle.normal.textColor = Color.yellow;

        _averageStyle = new GUIStyle(_centeredStyle);
        _averageStyle.normal.textColor = Color.cyan;

        _gvm.ApplyGameViewMode();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Current Game View Mode:", _centeredStyle);

        if (_gvm.CurrentGameView == E_GameViewMode.averageScreen)
            EditorGUILayout.LabelField("Average Screen", _averageStyle);
        else
            EditorGUILayout.LabelField("Single Screen", _singleStyle);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Switch Game View Mode", GUILayout.Width(200)))
            _gvm.SwitchGameViewMode();

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}