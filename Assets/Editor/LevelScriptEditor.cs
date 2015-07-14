using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelScript))]
public class LevelScriptEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        LevelScript myTarget = (LevelScript)target;
        
        myTarget.experience = EditorGUILayout.IntField("경험치", myTarget.experience);
        EditorGUILayout.LabelField("레벨", myTarget.Level.ToString());
		EditorGUILayout.HelpBox("도움말은 여기에", MessageType.Info);
    }
}