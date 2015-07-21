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
		
		void Awake()
		{
			currentTime = 0;
			Instance = this;
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

