using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using KillingGame.CrimeScene;
using System;

[System.Serializable]
public class ObjectNode : Node 
{
	public bool isExpanded = true;
	public string[] selectOptions = new string[] {"스프라이트 변경", "활성화 상태 변경", "사운드 재생", "메시지 출력", "아이템 획득", "위험도 변경", "이미지 출력"};
	public int selectOptionIndex = 0;
	public CrimeObject crimeObject;
	
	[System.NonSerialized]
	public Dictionary<NodeOutput, Selection> selects;
	
	public OutputSelectionPair[] selectList;
	
	public void OnSave()
	{
		Apply();
		crimeObject.Apply();
		objectPath = GetObjectPath(baseObject);
		
		selectList = new OutputSelectionPair[selects.Count];
		int cnt = 0;
		foreach (var item in selects)
		{
			item.Value.OnSave();
			selectList[cnt] = ScriptableObject.CreateInstance<OutputSelectionPair>();
			selectList[cnt].output = Outputs.IndexOf(item.Key);
			selectList[cnt].selection = item.Value;
			cnt++;
		}
	}
	public void OnLoad()
	{	
		Node_Editor.editor.OnSave += OnSave;
		Node_Editor.editor.OnLoad += OnLoad;
		baseObject = GameObject.Find(objectPath);
		crimeObject = baseObject.GetComponent<CrimeObject>();
		
		selects = new Dictionary<NodeOutput, Selection>();
		foreach (OutputSelectionPair item in selectList)
		{
			selects.Add(Outputs[item.output], item.selection);
			item.selection.OnLoad();
		}
	}
	
	public static ObjectNode Create(Rect NodeRect, NodeType nodeType = NodeType.Object) 
	{ // This function has to be registered in Node_Editor.ContextCallback
		ObjectNode node = ScriptableObject.CreateInstance<ObjectNode>();
		node.nodeType = nodeType;
		node.baseObject = new GameObject();
		node.crimeObject = node.baseObject.AddComponent<CrimeObject>();
		node.rect = NodeRect;
		node.selects = new Dictionary<NodeOutput, Selection>();
		
		if (nodeType == NodeType.Object)
		{
			node.crimeObject.isItem = false;
			node.baseObject.transform.localPosition = Vector3.zero;
			node.baseObject.transform.parent = GameObject.Find("CrimeObjects").transform;	
			node.baseObject.name = "새 오브젝트";
			node.baseObject.AddComponent<SpriteRenderer>();
			node.name = "오브젝트";
			NodeOutput.Create (node, "오브젝트 입력", IOtype.ObjectOnly);
			GameObject transHolder = new GameObject();
			transHolder.name = "버튼 생성위치";
			transHolder.transform.parent = node.baseObject.transform;
			node.crimeObject.buttonTrans = transHolder.transform;
		}
		else if (nodeType == NodeType.Item)
		{
			node.crimeObject.isItem = true;
			node.baseObject.transform.parent = GameObject.Find("CrimeItems").transform;	
			node.baseObject.name = "새 아이템";
			node.name = "아이템";
			NodeOutput.Create (node, "아이템 입력", IOtype.ItemOnly);			
		}
		node.Init ();
		Node_Editor.editor.OnSave += node.OnSave;
		Node_Editor.editor.OnLoad += node.OnLoad;
		return node;
	}

	public override void DrawNode() 
	{
		if (baseObject == null)
			OnLoad();
		GUILayout.BeginHorizontal();
		
		GUILayout.BeginVertical();
		Outputs[0].DisplayLayout();
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("이름");
		baseObject.name = EditorGUILayout.TextField(baseObject.name);
		GUILayout.EndHorizontal();
		crimeObject.isActive = EditorGUILayout.Toggle("활성화", crimeObject.isActive);
		if (!isExpanded)
		{
			if(GUILayout.Button("자세히"))
			{
				isExpanded = true;
				DrawNode();
			}
		}
		if (isExpanded)
		{
			if(GUILayout.Button("간략히"))
			{
				isExpanded = false;
				Vector2 topLeft = rect.position;
				rect = new Rect (topLeft.x, topLeft.y, 200, 100);
				DrawNode();
			}
			GUILayout.Label("기본 스프라이트");
			crimeObject.baseSprite = EditorGUILayout.ObjectField (crimeObject.baseSprite, typeof(Sprite), true) as Sprite;
			GUILayout.Label("선택 스프라이트");
			crimeObject.selectedSprite = EditorGUILayout.ObjectField (crimeObject.selectedSprite, typeof(Sprite), true) as Sprite;
			if(GUILayout.Button("선택지 추가"))
			{
				Selection selection = ScriptableObject.CreateInstance<Selection>();
				GameObject managerHolder = new GameObject();
				managerHolder.transform.parent = baseObject.transform;
				managerHolder.name = "새 선택지";
				selection.selectManager = managerHolder.AddComponent<SelectManager>();
				NodeOutput key = NodeOutput.Create(this, "선택지 입력", IOtype.SelectionOnly);
				selects.Add(key, selection);
				DrawNode();
			}
		}		
		GUILayout.Space(20);
		
		List<NodeOutput> keyList = new List<NodeOutput>(selects.Keys);

		foreach (var item in keyList)
		{
			DrawSelect(item);
		}
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();
	}
	
	private void DrawSelect(NodeOutput outPut)
	{
		if (outPut == null || !selects.ContainsKey(outPut))
			return;
		outPut.DisplayLayout();
		SelectManager selectManager = selects[outPut].selectManager;
		var functions = selects[outPut].functions;
		if (isExpanded)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("이름");
			if (selects.ContainsKey(outPut))
				selects[outPut].selectManager.gameObject.name = EditorGUILayout.TextField(selects[outPut].selectManager.gameObject.name);
			if(GUILayout.Button("선택지 삭제"))
			{
				if (!selects.ContainsKey(outPut))
					return;
				DestroyImmediate(selects[outPut].selectManager.gameObject);
				selects.Remove(outPut);
				Outputs.Remove(outPut);
				Vector2 topLeft = rect.position;
				rect = new Rect (topLeft.x, topLeft.y, 200, 100);
				foreach (NodeInput inPut in outPut.connections)
				{
					inPut.connection = null;
				}
				Outputs.Remove(outPut);
				DrawConnectors();
				return;
			}
			GUILayout.EndHorizontal();
			selectManager.isActive = EditorGUILayout.Toggle("활성화", selectManager.isActive);
			selectManager.isOnce = EditorGUILayout.Toggle("일회용", selects[outPut].selectManager.isOnce);
			selectManager.requireTime = EditorGUILayout.FloatField("소요시간", selectManager.requireTime);
			//  selects[outPut].selectManager.dangers.isActive = EditorGUILayout.Toggle("위험 활성화", selects[outPut].selectManager.dangers.isActive);
			selectManager.dangers.dangerCount = EditorGUILayout.IntField("위험도", selectManager.dangers.dangerCount);
			GUILayout.BeginHorizontal();
			selectOptionIndex = EditorGUILayout.Popup("종류", selectOptionIndex, selectOptions);
			if(GUILayout.Button("기능 추가"))
			{
				NodeInput key;
				Function executor = null;
				if (!selects.ContainsKey(outPut))
					return;
				switch (selectOptionIndex)
				{
					case 0:
						executor = selectManager.gameObject.AddComponent<SpriteChanger>();
						key = NodeInput.Create(this, "기능 출력", IOtype.ObjectOnly);
						break;
					case 1:
						executor = selectManager.gameObject.AddComponent<Enabler>();
						key = NodeInput.Create(this, "기능 출력", IOtype.General);
						break;
					case 2:
						executor = selectManager.gameObject.AddComponent<SoundPlayer>();
						key = NodeInput.Create(this, "기능 출력", IOtype.Closed);
						break;
					case 3:
						executor = selectManager.gameObject.AddComponent<MessageDisplayer>();
						key = NodeInput.Create(this, "기능 출력", IOtype.Closed);
						break;
					case 4:
						executor = selectManager.gameObject.AddComponent<ItemGainer>();
						key = NodeInput.Create(this, "기능 출력", IOtype.ItemOnly);
						break;
					case 5:
						executor = selectManager.gameObject.AddComponent<DangerChanger>();
						key = NodeInput.Create(this, "기능 출력", IOtype.SelectionOnly);
						break;
					case 6:
						executor = selectManager.gameObject.AddComponent<SpriteShower>();
						key = NodeInput.Create(this, "기능 출력", IOtype.Closed);
						break;
					default:
						executor = null;
						key = null;
						break;
				}
				functions.Add(key, executor);
				DrawNode();
			}
			GUILayout.EndHorizontal();
		}
		
		
		List<NodeInput> keyList = new List<NodeInput>(functions.Keys);
		foreach (var item in keyList)
		{
			DrawSelectFunction(outPut, item);
		}
		
		DrawConnectors();
		if (isExpanded)
			GUILayout.Space(20);
	}
	
	private void DrawSelectFunction(NodeOutput outPut, NodeInput inPut)
	{
		var functions = selects[outPut].functions;
		var target = functions[inPut];
		if (!selects.ContainsKey(outPut) || !functions.ContainsKey(inPut))
			return;
		inPut.DisplayLayout();
		if (isExpanded)
		{
			if (target is SpriteChanger)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(selectOptions[0]);
				if(GUILayout.Button("기능 삭제"))
				{
					if (!selects.ContainsKey(outPut) || !functions.ContainsKey(inPut))
						return;
					DestroyImmediate((MonoBehaviour)functions[inPut]);			
					functions.Remove(inPut);
					try
					{
						inPut.connection.connections.Remove(inPut);
					}
					catch (NullReferenceException e)
					{
						
					}
					Inputs.Remove(inPut);
					Vector2 topLeft = rect.position;
					rect = new Rect (topLeft.x, topLeft.y, 200, 100);
					DrawConnectors();
					return;
				}
				GUILayout.EndHorizontal();
				target.delay = EditorGUILayout.FloatField("발동 딜레이(초)", target.delay);
				
				SpriteChanger changer = (SpriteChanger)target;
				GUILayout.BeginHorizontal();
				GUILayout.Label("기본 스프라이트");
				changer.baseSprite = EditorGUILayout.ObjectField (changer.baseSprite, typeof(Sprite), true) as Sprite;
				GUILayout.Label("선택 스프라이트");
				changer.selectedSprite = EditorGUILayout.ObjectField (changer.selectedSprite, typeof(Sprite), true) as Sprite;
				GUILayout.EndHorizontal();
			}
			else if (target is Enabler)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(selectOptions[1]);
				if(GUILayout.Button("기능 삭제"))
				{
					if (!selects.ContainsKey(outPut) || !functions.ContainsKey(inPut))
						return;
					DestroyImmediate((MonoBehaviour)functions[inPut]);			
					functions.Remove(inPut);
					try
					{
						inPut.connection.connections.Remove(inPut);
					}
					catch (NullReferenceException e)
					{
						
					}
					Inputs.Remove(inPut);
					Vector2 topLeft = rect.position;
					rect = new Rect (topLeft.x, topLeft.y, 200, 100);
					DrawConnectors();
					return;
				}
				GUILayout.EndHorizontal();
				target.delay = EditorGUILayout.FloatField("발동 딜레이(초)", target.delay);
				
				Enabler enabler = (Enabler)target;
				enabler.option = (EnableOption)EditorGUILayout.EnumPopup("옵션", enabler.option);
			}
			else if (target is SoundPlayer)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(selectOptions[2]);
				if(GUILayout.Button("기능 삭제"))
				{
					if (!selects.ContainsKey(outPut) || !functions.ContainsKey(inPut))
						return;
					DestroyImmediate((MonoBehaviour)functions[inPut]);			
					functions.Remove(inPut);
					try
					{
						inPut.connection.connections.Remove(inPut);
					}
					catch (NullReferenceException e)
					{
						
					}
					Inputs.Remove(inPut);
					Vector2 topLeft = rect.position;
					rect = new Rect (topLeft.x, topLeft.y, 200, 100);
					DrawConnectors();
					return;
				}
				GUILayout.EndHorizontal();
				target.delay = EditorGUILayout.FloatField("발동 딜레이(초)", target.delay);
				
				SoundPlayer player = (SoundPlayer)target;
				player.sound = EditorGUILayout.ObjectField("효과음", player.sound, typeof(AudioClip), true) as AudioClip;
			}
			else if (target is MessageDisplayer)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(selectOptions[3]);
				if(GUILayout.Button("기능 삭제"))
				{
					if (!selects.ContainsKey(outPut) || !functions.ContainsKey(inPut))
						return;
					DestroyImmediate((MonoBehaviour)functions[inPut]);			
					functions.Remove(inPut);
					try
					{
						inPut.connection.connections.Remove(inPut);
					}
					catch (NullReferenceException e)
					{
						
					}
					Inputs.Remove(inPut);
					Vector2 topLeft = rect.position;
					rect = new Rect (topLeft.x, topLeft.y, 200, 100);
					DrawConnectors();
					return;
				}
				GUILayout.EndHorizontal();
				target.delay = EditorGUILayout.FloatField("발동 딜레이(초)", target.delay);
				
				GUILayout.BeginHorizontal();
				MessageDisplayer displayer = (MessageDisplayer)target;
				displayer.inputMessage = EditorGUILayout.TextArea(displayer.inputMessage);
				GUILayout.EndHorizontal();
			}
			else if (target is ItemGainer)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(selectOptions[4]);
				if(GUILayout.Button("기능 삭제"))
				{
					if (!selects.ContainsKey(outPut) || !functions.ContainsKey(inPut))
						return;
					DestroyImmediate((MonoBehaviour)functions[inPut]);			
					functions.Remove(inPut);
					try
					{
						inPut.connection.connections.Remove(inPut);
					}
					catch (NullReferenceException e)
					{
						
					}
					Inputs.Remove(inPut);
					Vector2 topLeft = rect.position;
					rect = new Rect (topLeft.x, topLeft.y, 200, 100);
					DrawConnectors();
					return;
				}
				GUILayout.EndHorizontal();
				target.delay = EditorGUILayout.FloatField("발동 딜레이(초)", target.delay);
				
				
			}
			else if (target is DangerChanger)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(selectOptions[5]);
				if(GUILayout.Button("기능 삭제"))
				{
					if (!selects.ContainsKey(outPut) || !functions.ContainsKey(inPut))
						return;
					DestroyImmediate((MonoBehaviour)functions[inPut]);			
					functions.Remove(inPut);
					try
					{
						inPut.connection.connections.Remove(inPut);
					}
					catch (NullReferenceException e)
					{
						
					}
					Inputs.Remove(inPut);
					Vector2 topLeft = rect.position;
					rect = new Rect (topLeft.x, topLeft.y, 200, 100);
					DrawConnectors();
					return;
				}
				GUILayout.EndHorizontal();
				target.delay = EditorGUILayout.FloatField("발동 딜레이(초)", target.delay);
				
				DangerChanger dChanger = (DangerChanger)target;
				dChanger.newDanger = EditorGUILayout.IntField("새 위험도", dChanger.newDanger);
			}
			else if (target is SpriteShower)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(selectOptions[6]);
				if(GUILayout.Button("기능 삭제"))
				{
					if (!selects.ContainsKey(outPut) || !functions.ContainsKey(inPut))
						return;
					DestroyImmediate((MonoBehaviour)functions[inPut]);			
					functions.Remove(inPut);
					try
					{
						inPut.connection.connections.Remove(inPut);
					}
					catch (NullReferenceException e)
					{
						
					}
					Inputs.Remove(inPut);
					Vector2 topLeft = rect.position;
					rect = new Rect (topLeft.x, topLeft.y, 200, 100);
					DrawConnectors();
					return;
				}
				GUILayout.EndHorizontal();
				target.delay = EditorGUILayout.FloatField("발동 딜레이(초)", target.delay);
				
				SpriteShower shower = (SpriteShower)target;
				for (int i=0; i<shower.sprites.Count; i++)
				{
					shower.sprites[i] = EditorGUILayout.ObjectField(shower.sprites[i], typeof(Sprite), true) as Sprite;
				}
				Sprite newSprite = null;
				newSprite = EditorGUILayout.ObjectField(newSprite, typeof(Sprite), true) as Sprite;
				if(newSprite != null)
				{
					shower.sprites.Add(newSprite);
				}
			}
			GUILayout.Space(10);
		}
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
				GameObject target = null;
				
				if (key.connection == null)
					continue;
				
				if (key.connection.body.Inputs.Count < 1 || key.connection == key.connection.body.Outputs[0])
				{
					target = key.connection.body.baseObject;
				}
				else
				{
					if (((ObjectNode)key.connection.body).selects.ContainsKey(key.connection))
						target = ((ObjectNode)key.connection.body).selects[key.connection].selectManager.gameObject;
				}
				selects[item].functions[key].SetTarget(target);
			}
		}
	}
	
	public override void OnDelete () 
	{
		Node_Editor.editor.OnSave -= OnSave;
		Node_Editor.editor.OnLoad -= OnLoad;
		DestroyImmediate(baseObject);
		base.OnDelete ();
		// Always call this if we want our custom OnDelete operations!
		// Else you can leave this out
	}
}