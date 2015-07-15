using UnityEngine;
using UnityEditor;
using KillingGame.CrimeScene;
[CustomEditor(typeof(CrimeObject))]
public class CrimeObjectInspector : Editor
{
	public override void OnInspectorGUI()
	{	
		base.OnInspectorGUI(); // delete this line when finished
        CrimeObject crimeObject = target as CrimeObject;	
		crimeObject.isActive = EditorGUILayout.Toggle("활성화", crimeObject.isActive);
		//crimeObject.baseSprite = 
		if(GUILayout.Button("선택지 추가"))
        {
            if (crimeObject)
            {
                GameObject newSelection = Instantiate(Resources.Load("Prefabs/UI/Select")) as GameObject;
				newSelection.transform.parent = crimeObject.transform;		
            }
        }
		EditorGUILayout.HelpBox("도움말은 여기에", MessageType.Info);				
	}
}
