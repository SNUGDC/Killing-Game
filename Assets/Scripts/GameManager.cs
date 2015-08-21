using UnityEngine;
using System.IO;
using System.Collections.Generic;
using LitJson;

namespace KillingGame
{
	public class GameData
	{
		public int dangerCount;
		public int chapter;
		public string routeID;
		public string lastPlay;
		
		public GameData()
		{
			
		}
	}

		
	public static class GameManager
	{
		private static GameData _gameData;
		private static int _currentSlot;
		public static int Danger
		{
			get
			{
				return _gameData.dangerCount;
			}
		}
		public static int Chapter
		{
			get
			{
				return _gameData.chapter;
			}
		}
		
		public static void StartStage()
		{
			Application.LoadLevel("Stage"+_gameData.chapter);
		}
		public static void ChapterClear(int dangerChange, List<string> eventFlagLists)
		{
			_gameData.chapter ++;
			_gameData.dangerCount += dangerChange;
		}
		public static void LoadChapter()
		{
			Application.LoadLevel("Chapter"+_gameData.chapter);
		}
		
		public static void NewGame(int slotNumber)
		{
			_gameData = new GameData();
			SaveGame(slotNumber);
		}
		
		public static void SaveGame(int slotNumber)
		{
			string jsonData = JsonMapper.ToJson(_gameData);
			PlayerPrefs.SetString("SaveData"+slotNumber, jsonData);
			PlayerPrefs.Save();
		}
		
		public static void LoadGame(int slotNumber)
		{
			string jsonData = PlayerPrefs.GetString("SaveData"+slotNumber);
			if (string.IsNullOrEmpty(jsonData))
				return;
			_gameData = JsonMapper.ToObject<GameData>(jsonData);
			LoadChapter();
		}
	}
}