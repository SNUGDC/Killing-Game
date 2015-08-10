using UnityEngine;
using System.Collections;

[System.Serializable]
public class InputPathPair : ScriptableObject
{
	public NodeInput input;
	public PathOrder path;
}