using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using KillingGame.CrimeScene;
using System;

[System.Serializable]
public class ItemNode : Node
{
	
	public override void DrawNode()
	{
		
	}
	
	public override void Apply()
	{
		
	}
	
	public static ItemNode Create(Rect NodeRect)
	{
		ItemNode node = ScriptableObject.CreateInstance<ItemNode>();
		node.baseObject = new GameObject();
		node.baseObject.transform.parent = GameObject.Find("CrimeItems").transform;
		node.baseObject.name = "새 아이템";
		node.name = "아이템";
			
		return node;
	}
}
