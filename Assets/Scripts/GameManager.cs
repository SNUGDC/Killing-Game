using UnityEngine;
using System.IO;
using System.Collections;
using LitJson;

namespace KillingGame
{
	public class GameData
	{
		public int dangerCount;
		public int chapter;
		
		public GameData(int dangerCount = 0, int chapter = 0)
		{
			this.dangerCount = dangerCount;
			this.chapter = chapter;
		}
	}
	
	public static class SaveManager
	{
		private static bool checkSlot(int saveSlot, out string filePath)
		{
			filePath = Application.persistentDataPath + "/save" + saveSlot.ToString("00") + ".json";
			if (0 <= saveSlot && saveSlot <= 99)
				return true;
			return false;
		}
		public static bool SaveGame(int saveSlot, ref GameData gameData)
		{
			string filePath;
			if (!checkSlot(saveSlot, out filePath))
				return false;
			
			string textData = JsonMapper.ToJson(gameData);
			File.WriteAllText(filePath, textData);
			
			return true;
		}
		public static bool LoadGame(int saveSlot, ref GameData gameData)
		{	
			string filePath;
			if (!checkSlot(saveSlot, out filePath))
				return false;
			
			FileInfo info = new FileInfo(filePath);
			if (info == null || info.Exists == false)
				return false;
			
			string textData = File.ReadAllText(filePath);
			gameData = JsonMapper.ToObject<GameData>(textData);
			
			return true;
		}
		public static bool NewGame(int saveSlot, ref GameData gameData)
		{
			string filePath;
			if (!checkSlot(saveSlot, out filePath))
				return false;
			
			gameData = new GameData();
			string textData = JsonMapper.ToJson(gameData);
			File.WriteAllText(filePath, textData);
			
			return true;
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
		
		public static void ChapterClear(int dangerChange)
		{
			_gameData.chapter ++;
			_gameData.dangerCount += dangerChange;
		}
		public static void LoadChapter()
		{
			
		}
	}
}