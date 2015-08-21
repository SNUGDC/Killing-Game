using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class InputPathPair : ScriptableObject
{
	void OnSave()
	{
		EditorUtility.SetDirty(this);
	}
	public NodeInput input;
	public PathOrder path;
}