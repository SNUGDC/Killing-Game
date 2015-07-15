using UnityEngine;
using UnityEditor;
using KillingGame.CrimeScene;
[CustomEditor(typeof(SelectManager))]

public class SelectManagerInspector : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI(); // delete this line when finished
		SelectManager selectManager = target as SelectManager;
		selectManager.isActive = EditorGUILayout.Toggle("Active", selectManager.isActive);
		selectManager.requireTime = EditorGUILayout.FloatField("Time", selectManager.requireTime);
		
		if (GUILayout.Button("Enabler"))
		{
			if (selectManager)
			{
				selectManager.gameObject.AddComponent<Enabler>();
			}
		}
		if (GUILayout.Button("SpriteChanger"))
		{
			if (selectManager)
			{
				selectManager.gameObject.AddComponent<SpriteChanger>();
			}
		}
		if (GUILayout.Button("ItemGainer"))
		{
			if (selectManager)
			{
				selectManager.gameObject.AddComponent<ItemGainer>();
			}
		}
		if (GUILayout.Button("MessageDisplayer"))
		{
			if (selectManager)
			{
				selectManager.gameObject.AddComponent<MessageDisplayer>();
			}
		}
		
		if (GUI.changed)
		{
			selectManager.gameObject.name = selectManager.label;
			//  selectManager.GetComponent<TextMesh>().text = selectManager.label;
		}
	}
}
