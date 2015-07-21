using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using KillingGame.CrimeScene;
using System;

[System.Serializable]
public class ObjectNode : Node 
{
	public bool isDraw = false;
	bool isExpanded = true;
	public string[] selectOptions = new string[] {"스프라이트 변경", "활성화 상태 변경", "사운드 재생", "메시지 출력", "아이템 획득", "위험도 변경", "이미지 출력"};
	public int selectOptionIndex = 0;
	public Dictionary<NodeOutput, Selection> selects = new Dictionary<NodeOutput, Selection>();

	
	public class Selection
	{
		public SelectManager selectManager;
		public Dictionary<NodeInput, IExecutable> functions = new Dictionary<NodeInput, IExecutable>();
		public Selection(CrimeObjectManager manager)
		{
			selectManager = new SelectManager();
			manager.selectList.Add(selectManager);
		}
	}
	
	public static ObjectNode Create (Rect NodeRect) 
	{ // This function has to be registered in Node_Editor.ContextCallback
		ObjectNode node = ScriptableObject.CreateInstance <ObjectNode> ();
		node.manager = new CrimeObjectManager();
		if (node.baseObject == null)
		{
			node.baseObject = new GameObject();
			node.baseObject.AddComponent<CrimeObject>();
			node.baseObject.AddComponent<SpriteRenderer>();
		}
		node.crimeObject = node.baseObject.GetComponent<CrimeObject>();
		node.manager = node.crimeObject.Manager;
		
		//  node.tempObject.transform.parent = GameObject.Find("TempObjects").transform;
 		//  node.tempObject.AddComponent<CrimeObject>().Manager = new CrimeObjectManager();
		//  node.manager = node.tempObject.GetComponent<CrimeObject>().Manager;
		//  node.tempObject.name = "새 오브젝트";
		//  node.name = "오브젝트";
		node.rect = NodeRect;

		NodeOutput.Create (node, "오브젝트 입력", typeof (float));

		node.Init ();
		return node;
	}

	public override void DrawNode() 
	{
		GUILayout.BeginHorizontal();
		
		GUILayout.BeginVertical();
		Outputs[0].DisplayLayout();
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("이름");
		manager.label = EditorGUILayout.TextField(manager.label);
		GUILayout.EndHorizontal();
		manager.isActive = EditorGUILayout.Toggle("활성화", manager.isActive);
		manager.useAsRoute = EditorGUILayout.Toggle("조건 노드로 사용하기", manager.useAsRoute);
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
			manager.baseSprite = EditorGUILayout.ObjectField (manager.baseSprite, typeof(Sprite), true) as Sprite;
			GUILayout.Label("선택 스프라이트");
			manager.selectedSprite = EditorGUILayout.ObjectField (manager.selectedSprite, typeof(Sprite), true) as Sprite;
			if(GUILayout.Button("선택지 추가"))
			{
				Selection selection = new Selection(manager);
				NodeOutput key = NodeOutput.Create(this, "선택지 입력", typeof(float));
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
		if (GUI.changed)
			Apply();
	}
	
	private void DrawSelect(NodeOutput outPut)
	{
		if (!selects.ContainsKey(outPut))
			return;
		outPut.DisplayLayout();
		if (isExpanded)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("이름");
			if (!selects.ContainsKey(outPut) || selects[outPut].selectManager == null)
				return;
			selects[outPut].selectManager.label = EditorGUILayout.TextField(selects[outPut].selectManager.label);
			if(GUILayout.Button("선택지 삭제"))
			{
				if (!selects.ContainsKey(outPut))
					return;
				manager.selectList.Remove(selects[outPut].selectManager);
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
			selects[outPut].selectManager.isActive = EditorGUILayout.Toggle("활성화", selects[outPut].selectManager.isActive);
			selects[outPut].selectManager.isOnce = EditorGUILayout.Toggle("일회용", selects[outPut].selectManager.isOnce);
			selects[outPut].selectManager.requireTime = EditorGUILayout.FloatField("소요시간", selects[outPut].selectManager.requireTime);
			//  selects[outPut].selectManager.dangers.isActive = EditorGUILayout.Toggle("위험 활성화", selects[outPut].selectManager.dangers.isActive);
			selects[outPut].selectManager.dangers.dangerCount = EditorGUILayout.IntField("위험도", selects[outPut].selectManager.dangers.dangerCount);
			GUILayout.BeginHorizontal();
			selectOptionIndex = EditorGUILayout.Popup("종류", selectOptionIndex, selectOptions);
			if(GUILayout.Button("기능 추가"))
			{
				NodeInput key = NodeInput.Create(this, "기능 출력", typeof(float));
				IExecutable executor = null;
				if (!selects.ContainsKey(outPut))
					return;
				switch (selectOptionIndex)
				{
					case 0:
						executor = new SpriteChanger();
					break;
					case 1:
						executor = new Enabler();
					break;
					case 2:
						executor = new SoundPlayer();
					break;
					case 3:
						executor = new MessageDisplayer();
					break;
					case 4:
						executor = new ItemGainer();
					break;
					case 5:
						executor = new DangerChanger();
					break;
					case 6:
						executor = new SpriteShower();
					break;
				}
				selects[outPut].selectManager.functions.Add(executor);
				selects[outPut].functions.Add(key, executor);
				DrawNode();
			}
			GUILayout.EndHorizontal();
		}
		
		
		List<NodeInput> keyList = new List<NodeInput>(selects[outPut].functions.Keys);
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
		if (!selects.ContainsKey(outPut) || !selects[outPut].functions.ContainsKey(inPut))
			return;
		inPut.DisplayLayout();
		if (isExpanded)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(selectOptions[selects[outPut].functions[inPut].ReturnIndex()]);
			if(GUILayout.Button("기능 삭제"))
			{
				if (!selects.ContainsKey(outPut) || !selects[outPut].functions.ContainsKey(inPut))
					return;
				selects[outPut].selectManager.functions.Remove(selects[outPut].functions[inPut]);		
				selects[outPut].functions.Remove(inPut);
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
			switch (selects[outPut].functions[inPut].ReturnIndex())
			{
				case 0:
					GUILayout.BeginHorizontal();
					GUILayout.Label("기본 스프라이트");
					((SpriteChanger)selects[outPut].functions[inPut]).baseSprite = EditorGUILayout.ObjectField (((SpriteChanger)selects[outPut].functions[inPut]).baseSprite, typeof(Sprite), true) as Sprite;
					GUILayout.Label("선택 스프라이트");
					((SpriteChanger)selects[outPut].functions[inPut]).selectedSprite = EditorGUILayout.ObjectField (((SpriteChanger)selects[outPut].functions[inPut]).selectedSprite, typeof(Sprite), true) as Sprite;
					GUILayout.EndHorizontal();
				break;
				case 1:
					((Enabler)selects[outPut].functions[inPut]).option = (EnableOption)EditorGUILayout.EnumPopup("옵션", ((Enabler)selects[outPut].functions[inPut]).option);
				break;
				case 2:
					SoundPlayer player = (SoundPlayer)selects[outPut].functions[inPut];
					player.sound = EditorGUILayout.ObjectField("효과음", player.sound, typeof(AudioClip), true) as AudioClip;
				break;
				case 3: 
					GUILayout.BeginHorizontal();
					((MessageDisplayer)selects[outPut].functions[inPut]).inputMessage = EditorGUILayout.TextArea(((MessageDisplayer)selects[outPut].functions[inPut]).inputMessage);
					GUILayout.EndHorizontal();
				break;
				case 4:
					
				break;
				case 5:
					DangerChanger changer = (DangerChanger)selects[outPut].functions[inPut];
					changer.newDanger = EditorGUILayout.IntField("새 위험도", changer.newDanger);
				break;
				case 6:
					SpriteShower shower = (SpriteShower)selects[outPut].functions[inPut];
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
				break;
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
				IEnable target = null;
				
				if (key.connection == null)
					continue;
				
				if (key.connection.body.Inputs.Count < 1 || key.connection == key.connection.body.Inputs[0])
				{
					target = key.connection.body.manager;
				}
				else
				{
					if (((ObjectNode)key.connection.body).selects.ContainsKey(key.connection))
						target = ((ObjectNode)key.connection.body).selects[key.connection].selectManager;
				}
				Debug.Log(target);
				selects[item].functions[key].SetTarget(target);
			}
		}
		//  if (baseObject == null)
		//  {
		//  	baseObject = new GameObject();
		//  	baseObject.transform.parent = GameObject.Find("CrimeObjects").transform;
		//  	baseObject.transform.localPosition = 2 * Vector3.back;
		//  	baseObject.AddComponent<CrimeObject>();
		//  }
		//  manager.GetCopy(baseObject.GetComponent<CrimeObject>().Manager);
		//  baseObject.GetComponent<CrimeObject>().Init();
		//  baseObject.GetComponent<SpriteRenderer>().sprite = manager.baseSprite;
		//  baseObject.GetComponent<CrimeObject>().Manager = manager;
		Collider2D coll = baseObject.GetComponent<Collider2D>();
		if (coll != null)
			DestroyImmediate(coll);	
		baseObject.AddComponent<PolygonCollider2D>();
	}
	
	public override void OnDelete () 
	{
		base.OnDelete ();
		// Always call this if we want our custom OnDelete operations!
		// Else you can leave this out
	}
}