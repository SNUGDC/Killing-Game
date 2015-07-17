using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using KillingGame.CrimeScene;
using System;

[System.Serializable]
public class ObjectNode : Node 
{
	public bool isDraw = false;
	
	public string[] selectOptions = new string[] {"스프라이트 변경", "활성화 상태 변경", "사운드 재생", "메시지 출력", "아이템 획득", "위험도 증가"};
	public int selectOptionIndex = 0;
	public CrimeObject crimeObject;
	public Dictionary<NodeOutput, Selection> selects = new Dictionary<NodeOutput, Selection>();
	
	public class Selection
	{
		public SelectManager selectManager;
		public Dictionary<NodeInput, IExecutable> functions = new Dictionary<NodeInput, IExecutable>();
		
		public Selection(GameObject baseObject)
		{
			GameObject selectionHolder = new GameObject();
			selectManager = selectionHolder.AddComponent<SelectManager>();
			selectionHolder.transform.parent = baseObject.transform;
		}
	}
	
	public static ObjectNode Create (Rect NodeRect) 
	{ // This function has to be registered in Node_Editor.ContextCallback
		ObjectNode node = ScriptableObject.CreateInstance <ObjectNode> ();
		node.baseObject = new GameObject();
 		node.baseObject.name = "새 오브젝트";
		node.baseObject.AddComponent<BoxCollider2D>();
		node.baseObject.AddComponent<SpriteRenderer>();
		node.crimeObject = node.baseObject.AddComponent<CrimeObject>();
		node.name = "오브젝트";
		node.rect = NodeRect;

		NodeOutput.Create (node, "오브젝트 입력", typeof (float));

		node.Init ();
		return node;
	}

	public override void DrawNode () 
	{
		GUILayout.BeginHorizontal();
		
		GUILayout.BeginVertical();
		Outputs[0].DisplayLayout();
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("이름");
		baseObject.name = EditorGUILayout.TextField(baseObject.name);	
		GUILayout.EndHorizontal();
		crimeObject.isActive = EditorGUILayout.Toggle("활성화", crimeObject.isActive);
		GUILayout.Label("기본 스프라이트");
		crimeObject.baseSprite = EditorGUILayout.ObjectField (crimeObject.baseSprite, typeof(Sprite), true) as Sprite;
		GUILayout.Label("선택 스프라이트");
		crimeObject.selectedSprite = EditorGUILayout.ObjectField (crimeObject.selectedSprite, typeof(Sprite), true) as Sprite;
		if(GUILayout.Button("선택지 추가"))
		{
			Selection selection = new Selection(baseObject);
			NodeOutput key = NodeOutput.Create(this, "선택지 입력", typeof(float));
			selects.Add(key, selection);
			DrawNode();
		}

		GUILayout.Space(20);
		
		List<NodeOutput> keyList = new List<NodeOutput>(selects.Keys);
		foreach (var item in keyList)
		{
			DrawSelect(item);
		}
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();
		if(GUI.changed)
			baseObject.GetComponent<SpriteRenderer>().sprite = crimeObject.baseSprite;
	}
	
	private void DrawSelect(NodeOutput outPut)
	{
		outPut.DisplayLayout();
		GUILayout.BeginHorizontal();
		GUILayout.Label("이름");
		selects[outPut].selectManager.label = EditorGUILayout.TextField(selects[outPut].selectManager.label);
		if(GUILayout.Button("선택지 삭제"))
		{
			DestroyImmediate(selects[outPut].selectManager.gameObject);
			selects.Remove(outPut);
			Outputs.Remove(outPut);
			DrawNode();
		}
		GUILayout.EndHorizontal();
		selects[outPut].selectManager.isActive = EditorGUILayout.Toggle("활성화", selects[outPut].selectManager.isActive);
		selects[outPut].selectManager.requireTime = EditorGUILayout.FloatField("소요시간", selects[outPut].selectManager.requireTime);
		selects[outPut].selectManager.dangers.isActive = EditorGUILayout.Toggle("위험 활성화", selects[outPut].selectManager.dangers.isActive);
		selects[outPut].selectManager.dangers.dangerCount = EditorGUILayout.IntField("위험도", selects[outPut].selectManager.dangers.dangerCount);
		GUILayout.BeginHorizontal();
		selectOptionIndex = EditorGUILayout.Popup("종류", selectOptionIndex, selectOptions);
		if(GUILayout.Button("기능 추가"))
		{
			NodeInput key = NodeInput.Create(this, "기능 출력", typeof(float));
			IExecutable executor = null;
			switch (selectOptionIndex)
			{
				case 0:
					executor = selects[outPut].selectManager.gameObject.AddComponent<SpriteChanger>();
				break;
				case 1:
					executor = selects[outPut].selectManager.gameObject.AddComponent<Enabler>();
				break;
				case 2:
					executor = selects[outPut].selectManager.gameObject.AddComponent<SpriteChanger>();
				break;
				case 3:
					executor = selects[outPut].selectManager.gameObject.AddComponent<MessageDisplayer>();
				break;
				case 4:
					executor = selects[outPut].selectManager.gameObject.AddComponent<ItemGainer>();
				break;
				case 5:
					executor = selects[outPut].selectManager.gameObject.AddComponent<SpriteChanger>();
				break;
			}
			selects[outPut].functions.Add(key, executor);
			DrawNode();
		}
		GUILayout.EndHorizontal();
		
		List<NodeInput> keyList = new List<NodeInput>(selects[outPut].functions.Keys);
		foreach (var item in keyList)
		{
			DrawSelectFunction(outPut, item);
		}
		
		
		GUILayout.Space(20);
	}
	
	private void DrawSelectFunction(NodeOutput outPut, NodeInput inPut)
	{
		inPut.DisplayLayout();
		GUILayout.BeginHorizontal();
		GUILayout.Label(selectOptions[selects[outPut].functions[inPut].ReturnIndex()]);
		if(GUILayout.Button("기능 삭제"))
		{
			DestroyImmediate((MonoBehaviour)selects[outPut].functions[inPut]);
			selects[outPut].functions.Remove(inPut);
			Inputs.Remove(inPut);
		}
		GUILayout.EndHorizontal();
		switch (selects[outPut].functions[inPut].ReturnIndex())
		{
			case 0:
				GUILayout.BeginHorizontal();
				GUILayout.Label("기본 스프라이트");
				((SpriteChanger)selects[outPut].functions[inPut]).baseSprite = EditorGUILayout.ObjectField (crimeObject.baseSprite, typeof(Sprite), true) as Sprite;
				GUILayout.Label("선택 스프라이트");
				((SpriteChanger)selects[outPut].functions[inPut]).selectedSprite = EditorGUILayout.ObjectField (crimeObject.selectedSprite, typeof(Sprite), true) as Sprite;
				GUILayout.EndHorizontal();
			break;
			case 1:
				((Enabler)selects[outPut].functions[inPut]).option = (EnableOption)EditorGUILayout.EnumPopup("옵션", ((Enabler)selects[outPut].functions[inPut]).option);
			break;
			case 2:
			
			break;
			case 3:
				GUILayout.BeginHorizontal();
				((MessageDisplayer)selects[outPut].functions[inPut]).inputMessage = EditorGUILayout.TextArea(((MessageDisplayer)selects[outPut].functions[inPut]).inputMessage);
				GUILayout.EndHorizontal();
			break;
			case 4:
			
			break;
			case 5:
				
			break;
		}
		GUILayout.Space(10);
	}
	
	public override void Apply()
	{
		List<NodeOutput> keyList = new List<NodeOutput>(selects.Keys);
		List<NodeInput> inputList;
		foreach (var item in keyList)
		{
			inputList = new List<NodeInput> (selects[item].functions.Keys);
			foreach (var key in inputList)
			{
				GameObject target;

				if (key.connection == null || key.connection.body.Inputs.Count < 1 || key.connection == key.connection.body.Inputs[0])
				{
					target = key.connection.body.baseObject;
				}
				else
				{
					target = ((ObjectNode)key.connection.body).selects[key.connection].selectManager.gameObject;
				}
				Debug.Log(target);
				selects[item].functions[key].SetTarget(target);
			}
		}
	}
	
	public override void OnDelete () 
	{
		DestroyImmediate(baseObject);
		base.OnDelete ();
		// Always call this if we want our custom OnDelete operations!
		// Else you can leave this out
	}
}
