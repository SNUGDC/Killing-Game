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
	CrimeObject _crime;
	public GameObject baseObject
	{
		get
		{
			if (_object == null)
			{
				_object = GameObject.Find(targetID);
				Reallocate(this);
			}	
			return _object;
		}
		set
		{
			_object = value;
			targetID = value.name;
		}
	}
	public CrimeObject crimeObject
	{
		get
		{
			if (_crime == null)
				_crime = baseObject.GetComponent<CrimeObject>();
			return _crime;
		}
		set
		{
			_crime = value;
		}
	}
	public SerializableDictionary<NodeOutput, Selection> selects;
	
	public class Selection
	{
		ObjectNode node;
		//  SelectManager _selectManager;
		//  public SelectManager selectManager
		//  {
		//  	get
		//  	{
		//  		if (_selectManager == null)
		//  			Reallocate(node);
		//  		return _selectManager;
		//  	}
		//  	set
		//  	{
		//  		_selectManager = value;
		//  	}
		//  }
		public SelectManager selectManager;
		
		public SerializableDictionary<NodeInput, IExecutable> functions = new SerializableDictionary<NodeInput, IExecutable>();
		
		public Selection(GameObject baseObject, ObjectNode node)
		{
			this.node = node;
			GameObject selectionHolder = new GameObject();
			selectionHolder.name = "Selections";
			selectManager = selectionHolder.AddComponent<SelectManager>();
			selectionHolder.transform.parent = baseObject.transform;
		}
		public Selection(SelectManager selectManager, ObjectNode node)
		{
			this.node = node;
			this.selectManager = selectManager;
		}
	}
	
	public SerializableDictionary<int, int> testDic;
	
	public static void Reallocate(Node baseNode)
	{
		ObjectNode node = (ObjectNode)baseNode;
		Debug.Log(node.testDic.MyDictionary.Count);
		//  List<Selection> selectsValues = new List<Selection>(node.selects.Values);
		
		//  Debug.Log(node.selects.Count);
		
		//  for (int i=0; i<selectsValues.Count; i++)
		//  {
		//  	Selection selection = selectsValues[i];
		//  	selection.selectManager = node.baseObject.transform.GetChild(i).GetComponent<SelectManager>();
		//  	Debug.Log(selection.selectManager.gameObject.name);
		//  	IExecutable[] comps;
		//  	comps = node.baseObject.transform.GetChild(i).GetComponents<IExecutable>();
		//  	List<NodeInput> functionsKeys = new List<NodeInput>(selection.functions.Keys);
		//  	for (int j=0; j<functionsKeys.Count; j++)
		//  	{
		//  		selection.functions[functionsKeys[j]] = comps[j];
		//  	}
		//  }
	}
	
	public static ObjectNode Create (Rect NodeRect) 
	{ // This function has to be registered in Node_Editor.ContextCallback
		ObjectNode node = ScriptableObject.CreateInstance <ObjectNode> ();
		node.baseObject = new GameObject();
		node.baseObject.transform.parent = GameObject.Find("CrimeObjects").transform;
 		node.baseObject.name = "새 오브젝트";
		node.baseObject.AddComponent<SpriteRenderer>();
		node.crimeObject = node.baseObject.AddComponent<CrimeObject>();
		node.name = "오브젝트";
		node.rect = NodeRect;
		node.selects = new SerializableDictionary<NodeOutput, Selection>();
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
		baseObject.name = EditorGUILayout.TextField(baseObject.name);
		GUILayout.EndHorizontal();
		crimeObject.isActive = EditorGUILayout.Toggle("활성화", crimeObject.isActive);
		crimeObject.useAsRoute = EditorGUILayout.Toggle("조건 노드로 사용하기", crimeObject.useAsRoute);
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
				testDic = new SerializableDictionary<int, int>();
				testDic.MyDictionary.Add(1,1);
				testDic.MyDictionary.Add(2,2);
				
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
				Selection selection = new Selection(baseObject, this);
				NodeOutput key = NodeOutput.Create(this, "선택지 입력", typeof(float));
				selects.MyDictionary.Add(key, selection);
				DrawNode();
			}
		}		
		GUILayout.Space(20);
		
		//  List<NodeOutput> keyList = new List<NodeOutput>(selects.MyDictionary.Keys);
		List<NodeOutput> keyList = new List<NodeOutput>();
		foreach (var item in keyList)
		{
			DrawSelect(item);
		}
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();
		if(GUI.changed)
		{
			crimeObject.GetComponent<SpriteRenderer>().sprite = crimeObject.baseSprite;
			Apply();			
		}	
		if (GUI.changed)
			targetID = baseObject.name;
	}
	
	private void DrawSelect(NodeOutput outPut)
	{
		if (outPut == null || !selects.MyDictionary.ContainsKey(outPut))
			return;
		outPut.DisplayLayout();
		if (isExpanded)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("이름");
			if (selects.MyDictionary.ContainsKey(outPut))
				selects.MyDictionary[outPut].selectManager.gameObject.name = EditorGUILayout.TextField(selects.MyDictionary[outPut].selectManager.gameObject.name);
			if(GUILayout.Button("선택지 삭제"))
			{
				if (!selects.MyDictionary.ContainsKey(outPut))
					return;
				DestroyImmediate(selects.MyDictionary[outPut].selectManager.gameObject);
				selects.MyDictionary.Remove(outPut);
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
			selects.MyDictionary[outPut].selectManager.isActive = EditorGUILayout.Toggle("활성화", selects.MyDictionary[outPut].selectManager.isActive);
			selects.MyDictionary[outPut].selectManager.isOnce = EditorGUILayout.Toggle("일회용", selects.MyDictionary[outPut].selectManager.isOnce);
			selects.MyDictionary[outPut].selectManager.requireTime = EditorGUILayout.FloatField("소요시간", selects.MyDictionary[outPut].selectManager.requireTime);
			//  selects[outPut].selectManager.dangers.isActive = EditorGUILayout.Toggle("위험 활성화", selects[outPut].selectManager.dangers.isActive);
			selects.MyDictionary[outPut].selectManager.dangers.dangerCount = EditorGUILayout.IntField("위험도", selects.MyDictionary[outPut].selectManager.dangers.dangerCount);
			GUILayout.BeginHorizontal();
			selectOptionIndex = EditorGUILayout.Popup("종류", selectOptionIndex, selectOptions);
			if(GUILayout.Button("기능 추가"))
			{
				NodeInput key = NodeInput.Create(this, "기능 출력", typeof(float));
				IExecutable executor = null;
				if (!selects.MyDictionary.ContainsKey(outPut))
					return;
				switch (selectOptionIndex)
				{
					case 0:
						executor = selects.MyDictionary[outPut].selectManager.gameObject.AddComponent<SpriteChanger>();
					break;
					case 1:
						executor = selects.MyDictionary[outPut].selectManager.gameObject.AddComponent<Enabler>();
					break;
					case 2:
						executor = selects.MyDictionary[outPut].selectManager.gameObject.AddComponent<SoundPlayer>();
					break;
					case 3:
						executor = selects.MyDictionary[outPut].selectManager.gameObject.AddComponent<MessageDisplayer>();
					break;
					case 4:
						executor = selects.MyDictionary[outPut].selectManager.gameObject.AddComponent<ItemGainer>();
					break;
					case 5:
						executor = selects.MyDictionary[outPut].selectManager.gameObject.AddComponent<DangerChanger>();
					break;
					case 6:
						executor = selects.MyDictionary[outPut].selectManager.gameObject.AddComponent<SpriteShower>();
					break;
				}
				selects.MyDictionary[outPut].functions.MyDictionary.Add(key, executor);
				DrawNode();
			}
			GUILayout.EndHorizontal();
		}
		
		
		List<NodeInput> keyList = new List<NodeInput>(selects.MyDictionary[outPut].functions.MyDictionary.Keys);
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
		if (!selects.MyDictionary.ContainsKey(outPut) || !selects.MyDictionary[outPut].functions.MyDictionary.ContainsKey(inPut))
			return;
		inPut.DisplayLayout();
		if (isExpanded)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(selectOptions[selects.MyDictionary[outPut].functions.MyDictionary[inPut].ReturnIndex()]);
			if(GUILayout.Button("기능 삭제"))
			{
				if (!selects.MyDictionary.ContainsKey(outPut) || !selects.MyDictionary[outPut].functions.MyDictionary.ContainsKey(inPut))
					return;
				DestroyImmediate((MonoBehaviour)selects.MyDictionary[outPut].functions.MyDictionary[inPut]);			
				selects.MyDictionary[outPut].functions.MyDictionary.Remove(inPut);
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
			switch (selects.MyDictionary[outPut].functions.MyDictionary[inPut].ReturnIndex())
			{
				case 0:
					GUILayout.BeginHorizontal();
					GUILayout.Label("기본 스프라이트");
					((SpriteChanger)selects.MyDictionary[outPut].functions.MyDictionary[inPut]).baseSprite = EditorGUILayout.ObjectField (((SpriteChanger)selects.MyDictionary[outPut].functions.MyDictionary[inPut]).baseSprite, typeof(Sprite), true) as Sprite;
					GUILayout.Label("선택 스프라이트");
					((SpriteChanger)selects.MyDictionary[outPut].functions.MyDictionary[inPut]).selectedSprite = EditorGUILayout.ObjectField (((SpriteChanger)selects.MyDictionary[outPut].functions.MyDictionary[inPut]).selectedSprite, typeof(Sprite), true) as Sprite;
					GUILayout.EndHorizontal();
				break;
				case 1:
					((Enabler)selects.MyDictionary[outPut].functions.MyDictionary[inPut]).option = (EnableOption)EditorGUILayout.EnumPopup("옵션", ((Enabler)selects.MyDictionary[outPut].functions.MyDictionary[inPut]).option);
				break;
				case 2:
					SoundPlayer player = (SoundPlayer)selects.MyDictionary[outPut].functions.MyDictionary[inPut];
					player.sound = EditorGUILayout.ObjectField("효과음", player.sound, typeof(AudioClip), true) as AudioClip;
				break;
				case 3: 
					GUILayout.BeginHorizontal();
					((MessageDisplayer)selects.MyDictionary[outPut].functions.MyDictionary[inPut]).inputMessage = EditorGUILayout.TextArea(((MessageDisplayer)selects.MyDictionary[outPut].functions.MyDictionary[inPut]).inputMessage);
					GUILayout.EndHorizontal();
				break;
				case 4:
					
				break;
				case 5:
					DangerChanger changer = (DangerChanger)selects.MyDictionary[outPut].functions.MyDictionary[inPut];
					changer.newDanger = EditorGUILayout.IntField("새 위험도", changer.newDanger);
				break;
				case 6:
					SpriteShower shower = (SpriteShower)selects.MyDictionary[outPut].functions.MyDictionary[inPut];
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
		List<NodeOutput> keyList = new List<NodeOutput>(selects.MyDictionary.Keys);
		List<NodeInput> inputList;
		foreach (var item in keyList)
		{
			inputList = new List<NodeInput> (selects.MyDictionary[item].functions.MyDictionary.Keys);
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
					if (((ObjectNode)key.connection.body).selects.MyDictionary.ContainsKey(key.connection))
						target = ((ObjectNode)key.connection.body).selects.MyDictionary[key.connection].selectManager.gameObject;
				}
				selects.MyDictionary[item].functions.MyDictionary[key].SetTarget(target);
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