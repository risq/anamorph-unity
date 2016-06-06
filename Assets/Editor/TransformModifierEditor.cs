using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TransformModifier))]
public class TransformModifierEditor : Editor {

    public override void OnInspectorGUI()
    {
        TransformModifier transformModifier = (TransformModifier)target;
        
        if (DrawDefaultInspector())
        {
            transformModifier.UpdateTransform();
        }

        EditorGUILayout.Space();
        transformModifier.CurrentValue = EditorGUILayout.IntSlider("Current Value", (int)transformModifier.CurrentValue, 0, 100);
    }
}
