using UnityEngine;

namespace KillingGame.CrimeScene
{
	public class CrimeManager : MonoBehaviour
	{
		public static CrimeManager Instance;
		public bool isGUI = false;
		public GameObject messageHolder;
		public float maxTime;
		float currentTime;
		public GameObject needle;
		public int dangerCount;
		
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
			
		}
		
		public void spendTime(float timeSpent)
		{
			currentTime += timeSpent;
			if (currentTime > maxTime)
				Application.LoadLevel("GameOver");
			showTime();
		}
		
		public void SpendTime(float timeSpent)
		{
			
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

