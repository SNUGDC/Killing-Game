using UnityEngine;
using UnityEditor;
using KillingGame.CrimeScene;
[CustomEditor(typeof(Enabler))]

public class EnablerInspector : Editor 
{
	Enabler enabler;
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI(); // delete this line when finished
		enabler = target as Enabler;
		
		if (GUI.changed)
		{
					Handles.Label(enabler.transform.position + Vector3.up*2,
	 				enabler.transform.position.ToString() + "\nShieldArea: ");
		}
		if (enabler.target != null)
		{
			Handles.DrawLine(enabler.transform.position, enabler.target.transform.position);
		}
		HandleUtility.Repaint();
	}
	public void OnScene(SceneView sceneView)
	{
		if (enabler != null && enabler.target != null)
		{
			Handles.DrawLine(enabler.transform.position, enabler.target.transform.position);
		}
		HandleUtility.Repaint();
	}
	 void OnEnable()
     {
         SceneView.onSceneGUIDelegate -= OnScene;
         SceneView.onSceneGUIDelegate += OnScene;
     }
}
