using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Diagram))]
public class DiagramEditor : Editor {

    public override void OnInspectorGUI()
    {
        Diagram diagram = (Diagram)target;

        if (DrawDefaultInspector())
        {
            diagram.UpdateDiagram();
        }
    }

}
