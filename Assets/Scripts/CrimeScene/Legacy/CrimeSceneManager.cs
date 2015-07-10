using UnityEngine;
using System;
using System.Collections.Generic;
using LitJson;

namespace KillingGame.CrimeScene
{
	public class TimeSchedule
	{
		float maxTime;
		float currentTime;
		
		public TimeSchedule(float maxTime, float currentTime = 0)
		{
			this.maxTime = maxTime;
			this.currentTime = currentTime;
			showCurrentTime();
		}
		
		public void spendTime(float timeSpent)
		{
			currentTime=Mathf.Clamp(timeSpent+currentTime, 0 , maxTime);
			showCurrentTime();
		}
		void showCurrentTime()
		{
			
		}
	}
	
	public class CrimeSceneManager : MonoBehaviour 
	{
		public string stageJsonPath;
		StageData stageData;
		TimeSchedule timeSchedule;
		Dictionary<string, InteractObject> interactions;
		
		void Awake()
		{
			string json = System.IO.File.ReadAllText(stageJsonPath);
			stageData = JsonMapper.ToObject<StageData>(json);
			initialize();
		}
		
		void initialize()
		{
			interactions = new Dictionary<string, InteractObject>();	
		}
		
		public bool checkActive(string key)
		{
			InteractObject value;
			try
			{
				return (interactions.TryGetValue(key, out value)&&value.isActive);
			}
			catch(ArgumentNullException e)
			{
				Debug.Log(key + " does not exists");
				return false;
			}
		}
		
		void gameOver()
		{
			
		}
		
		void stageClear()
		{
			
		}
	}
}