using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(CubeRotator))]
public class CubeRotatorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Custom Inspector for", "CubeRotator"); 
         
        base.OnInspectorGUI();
         
        if(GUILayout.Button("Change Direction"))
        {
            CubeRotator cube = target as CubeRotator;
            if (cube)
            {
                cube.rotationSpeed = -cube.rotationSpeed;
            }
        }
    }
}