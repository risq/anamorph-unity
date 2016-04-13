using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SocialDataManager))]
public class SocialDataManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
        SocialDataManager socialDataManager = (SocialDataManager)target;
        socialDataManager.activityVolumePublic = EditorGUILayout.Slider("Public Volume Activity", socialDataManager.activityVolumePublic, 0, 100);
        socialDataManager.activityVolumePrivate = EditorGUILayout.Slider("Private Volume Activity", socialDataManager.activityVolumePrivate, 0, 100);
        socialDataManager.activityVolumePro = EditorGUILayout.Slider("Pro Volume Activity", socialDataManager.activityVolumePro, 0, 100);
        socialDataManager.activityFrequencePublic = EditorGUILayout.Slider("Public Frequence Activity", socialDataManager.activityFrequencePublic, 0, 100);
        socialDataManager.activityFrequencePrivate = EditorGUILayout.Slider("Private Frequence Activity", socialDataManager.activityFrequencePrivate, 0, 100);
        socialDataManager.activityFreqencePro = EditorGUILayout.Slider("Pro Frequence Activity", socialDataManager.activityFreqencePro, 0, 100);
    }
}
