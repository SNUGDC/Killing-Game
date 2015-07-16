using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using KillingGame.CrimeScene;

[System.Serializable]
public class ObjectNode : Node 
{
	public enum CalcType { Add, Substract, Multiply, Divide }
	public CalcType type = CalcType.Add;
	public float Input1Val = 1f;
	public float Input2Val = 1f;
	public bool isDraw = false;
	
	public string[] selectOptions = new string[] {"스프라이트 변경", "활성화 상태 변경", "사운드 재생", "메시지 출력", "아이템 획득", "위험도 증가"};
	public int selectOptionIndex = 0;
	public GameObject baseObject;
	public CrimeObject crimeObject;
	public List<SelectManager> selections = new List<SelectManager>();
	
	public class Selection
	{
		public NodeInput Input;
		public Selection()
		{
			
		}
	}
	
	public static ObjectNode Create (Rect NodeRect) 
	{ // This function has to be registered in Node_Editor.ContextCallback
		ObjectNode node = ScriptableObject.CreateInstance <ObjectNode> ();
		node.baseObject = new GameObject();
 		node.baseObject.name = "새 오브젝트";
		node.crimeObject = node.baseObject.AddComponent<CrimeObject>();
		node.name = "오브젝트";
		node.rect = NodeRect;
		
		NodeInput.Create (node, "Object Input", typeof (float));
		NodeInput.Create (node, "Input 2", typeof (float));
		
		NodeOutput.Create (node, "Output 1", typeof (float));

		node.Init ();
		return node;
	}

	public override void DrawNode () 
	{
		GUILayout.BeginHorizontal();
		
		GUILayout.BeginVertical();
		
		GUILayout.BeginHorizontal();
		Inputs[0].DisplayLayout();
		GUILayout.Label("이름");
		baseObject.name = GUILayout.TextField(baseObject.name);
		Outputs[0].DisplayLayout();
		GUILayout.EndHorizontal();
		crimeObject.isActive = EditorGUILayout.Toggle("활성화", crimeObject.isActive);
		GUILayout.Label("기본 스프라이트");
		crimeObject.baseSprite = EditorGUILayout.ObjectField (crimeObject.baseSprite, typeof(Sprite), true) as Sprite;
		GUILayout.Label("선택 스프라이트");
		crimeObject.selectedSprite = EditorGUILayout.ObjectField (crimeObject.selectedSprite, typeof(Sprite), true) as Sprite;
		if(GUILayout.Button("선택지 추가"))
		{
			SelectManager newSelect = new GameObject().AddComponent<SelectManager>();
			selections.Add(newSelect);
			DrawNode();
		}

		GUILayout.Space(20);
		
		for (int i=0; i<selections.Count; i++)
		{
			DrawSelect(i);
		}
		GUILayout.EndVertical();
		
		GUILayout.EndHorizontal();
		if (GUI.changed)
			;
	}
	
	private void DrawSelect(int i)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("선택지 이름");
		selections[i].name = GUILayout.TextField(selections[i].name);
		if(GUILayout.Button("선택지 삭제"))
		{
			DestroyImmediate(selections[i].gameObject);
			selections.RemoveAt(i);
			DrawNode();
		}
		GUILayout.EndHorizontal();
		selections[i].isActive = EditorGUILayout.Toggle("활성화", selections[i].isActive);
		GUILayout.BeginHorizontal();
		EditorGUILayout.Popup("종류", selectOptionIndex, selectOptions);
		if(GUILayout.Button("기능 추가"))
		{
			
		}
		GUILayout.EndHorizontal();
		
		
		GUILayout.Space(10);
	}
	
	public override void OnDelete () 
	{
		DestroyImmediate(baseObject);
		base.OnDelete ();
		// Always call this if we want our custom OnDelete operations!
		// Else you can leave this out
	}
}
