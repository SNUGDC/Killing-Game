using UnityEngine;
using System.Collections.Generic;
using LitJson;

namespace KillingGame.CrimeScene
{
	public class CrimeSceneManager : MonoBehaviour 
	{
		public string stageJsonPath;
		StageData stageData;
		
		void Awake()
		{
			string json = System.IO.File.ReadAllText(stageJsonPath);
			stageData = JsonMapper.ToObject<StageData>(json);
			initialize();
		}
		
		void initialize()
		{
			
		}
		
		void gameOver()
		{
			
		}
		
		void stageClear()
		{
			
		}
	}
}