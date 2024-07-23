using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TMPTextGroup))]
public class TMPTextGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck())
            ((TMPTextGroup)target).SetLanguageText();
    }
}
