using UnityEngine;
using System.Collections.Generic;

namespace KillingGame.CrimeScene
{
	public class CrimeManager : MonoBehaviour
	{
		public static CrimeManager Instance;
		public bool isGUI = false;
		List<GameObject> items = new List<GameObject>();
		public ItemButton[] itemHolders;
		public GameObject messageHolder;
		public GameObject spriteShower;
		public GameObject soundPlayer;
		List<string> eventFlags;
		
		public float maxTime = 15;
		float currentTime;
		public GameObject needle;
		public int dangerCount;
		List<CrimeObject> itemList = new List<CrimeObject>();
		GameObject[] selectButtons;
		GameObject[] tempSpritors;
		public GameObject canceler;
		
		void Awake()
		{
			currentTime = 0;
			Instance = this;
			
			selectButtons = new GameObject[10];
			for (int i=0; i<10; i++)
			{
				selectButtons[i] = Instantiate(Resources.Load("Prefabs/UI/Select")) as GameObject;
				selectButtons[i].SetActive(false);
			}
			
			tempSpritors = new GameObject[10];
			for (int i=0; i<10; i++)
			{
				tempSpritors[i] = Instantiate(Resources.Load("Prefabs/Effects/TempSpritor")) as GameObject;
				tempSpritors[i].SetActive(false);
			}
		}
		
		public GameObject GetButton()
		{
			for (int i=0; i<10; i++)
			{
				if (!selectButtons[i].activeSelf)
				{
					selectButtons[i].SetActive(true);
					return selectButtons[i];
				}
			}	
			return null;
		}
		
		public GameObject GetCanceler()
		{
			canceler.SetActive(true);
			return canceler;
		}
		
		public GameObject GetTempSpritor()
		{
			for (int i=0; i<10; i++)
			{
				if (!tempSpritors[i].activeSelf)
				{
					tempSpritors[i].SetActive(true);
					return tempSpritors[i];
				}
			}
			return null;
		}
		
		public void ObtainItem(GameObject item)
		{
			CrimeObject itemScript = item.GetComponent<CrimeObject>();
			if (itemScript == null)
				return;
			if (itemList.Contains(itemScript))
				return;
			itemScript.isActive = true;
			itemList.Add(itemScript);
			DisplayItem();
		}
		
		public void DisplayItem()
		{
			int holderCnt = 0;
			for (int cnt=0; cnt<itemList.Count; cnt++)
			{
				if (itemList[cnt] == null || !itemList[cnt].isActive || itemList[cnt].isDestroyed)
					continue;
				itemHolders[holderCnt].SetTarget(itemList[cnt]);
				holderCnt++;
			}
		}

		public void ShowMessage(string[] texts)
		{
			isGUI = true;
			messageHolder.SetActive(true);
			messageHolder.GetComponent<MessageShow>().getMessages(texts);
		}
		
		public void ShowSprite(List<Sprite> sprites)
		{
			isGUI = true;
			spriteShower.SetActive(true);
			spriteShower.GetComponent<SpriteShow>().GetSprites(sprites);
		}
		
		public void RegisterEvent(string newEventFlag)
		{
			if (newEventFlag == null || newEventFlag == "")
				return;
			if (eventFlags.Contains(newEventFlag))
				return;
			eventFlags.Add(newEventFlag);
		}
		
		public void SpendTime(float timeSpent)
		{
			currentTime += timeSpent;
			if (currentTime > maxTime)
				Application.LoadLevel("GameOver");
			ShowTime();
			Debug.Log(currentTime);
		}
		
		public void PlaySound(AudioClip sound)
		{
			soundPlayer.GetComponent<AudioSource>().PlayOneShot(sound);
		}
		
		void ShowTime()
		{
			needle.transform.eulerAngles = new Vector3(0,0,-360 * currentTime/maxTime);
		}
		
		public void ExitCrimeScene()
		{
			GameManager.ChapterClear(dangerCount, eventFlags);
		}
	}	
}

