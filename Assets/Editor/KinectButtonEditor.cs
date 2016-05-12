using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KinectButton))]
class KinectButtonEditor : Editor
{
    void OnSceneGUI()
    {
        KinectButton kinectButton = target as KinectButton;
        Handles.color = Color.red;
        Handles.DrawWireDisc(kinectButton.transform.position, Vector3.forward, kinectButton.radius);

        EditorGUI.BeginChangeCheck();
        kinectButton.radius = Handles.ScaleValueHandle(kinectButton.radius, kinectButton.transform.position + new Vector3(-kinectButton.radius, 0, 0), Quaternion.identity, 2, Handles.CylinderCap, 2);
    }
}