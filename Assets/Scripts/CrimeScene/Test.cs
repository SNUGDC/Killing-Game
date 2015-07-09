using UnityEngine;
using LitJson;

public class TClass
{
	public int b = 7;
}

public class TestClass : TClass
{
	public int a = 3;
	
	public struct pos
	{
		public float xPos;
		public float yPos;
		public pos(float x, float y)
		{
			xPos = x;
			yPos = y;
		}
	}
	public pos poss = new pos(1,2);
}

public class Test : MonoBehaviour
{
	TestClass testClass;
	void Start()
	{
		testClass = new TestClass();
		//string json = System.IO.File.ReadAllText(Application.dataPath+"/TestJson.json");
		string json1 = JsonMapper.ToJson(testClass);
		Debug.Log(json1);
		//  testClass = JsonMapper.ToObject<TestClass>(json);
		//  Debug.Log(testClass.a);
	}
}