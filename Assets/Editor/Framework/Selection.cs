using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using KillingGame.CrimeScene;

[System.Serializable]
public class Selection : ScriptableObject
{
	//  public ObjectNode node;
	[System.NonSerialized]
	public SelectManager selectManager;
	public string selectHolderPath;
	[System.NonSerialized]
	public Dictionary<NodeInput, Function> functions = new Dictionary<NodeInput, Function>();
	
	public InputPathPair[] functionList;
	
	public void OnSave()
	{
		selectHolderPath = Node.GetObjectPath(selectManager.gameObject);
		functionList = new InputPathPair[functions.Count];
		int cnt = 0;
		foreach (var item in functions)
		{
			PathOrder path = Node.GetComponentPath<Function>(item.Value);
			functionList[cnt] = ScriptableObject.CreateInstance<InputPathPair>();
			functionList[cnt].input = item.Key;
			functionList[cnt].path = path;
			cnt ++;
		}
		EditorUtility.SetDirty(this);
	}
	public void OnLoad()
	{
		Debug.Log(selectHolderPath);
		selectManager = Node.GetObject(selectHolderPath).GetComponent<SelectManager>();
		functions = new Dictionary<NodeInput, Function>();
		foreach (InputPathPair item in functionList)
		{
			Function target = Node.GetComponentFromPath<Function>(item.path);
			functions.Add(item.input, target);
		}
	}
}
