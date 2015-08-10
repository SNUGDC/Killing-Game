using UnityEngine;
using System.Collections.Generic;

namespace KillingGame.CrimeScene
{
	public class CrimeManager : MonoBehaviour
	{
		public static CrimeManager Instance;
		public bool isGUI = false;
		public GameObject messageHolder;
		public GameObject spriteShower;
		public GameObject soundPlayer;
		public float maxTime = 15;
		float currentTime;
		public GameObject needle;
		public int dangerCount;
		public List<GameObject> itemList;
		GameObject[] selectButtons;
		GameObject[] tempSpritors;
		
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
		
		public void obtainItem(GameObject item)
		{
			
		}
		
		public void showMessage(string[] texts)
		{
			isGUI = true;
			messageHolder.SetActive(true);
			messageHolder.GetComponent<MessageShow>().getMessages(texts);
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
		
		public void spendTime(float timeSpent)
		{
			currentTime += timeSpent;
			if (currentTime > maxTime)
				Application.LoadLevel("GameOver");
			showTime();
			Debug.Log(currentTime);
		}
		
		public void SpendTime(float timeSpent)
		{
			currentTime += timeSpent;
			if (currentTime > maxTime)
				Application.LoadLevel("GameOver");
			showTime();
			Debug.Log(currentTime);
		}
		
		public void PlaySound(AudioClip sound)
		{
			soundPlayer.GetComponent<AudioSource>().PlayOneShot(sound);
		}
		
		void showTime()
		{
			needle.transform.eulerAngles = new Vector3(0,0,-360 * currentTime/maxTime);
		}
		
		void ExitCrimeScene()
		{
			GameManager.ChapterClear(dangerCount);
		}
	}	
}

