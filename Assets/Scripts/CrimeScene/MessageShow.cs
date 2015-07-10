using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace KillingGame.CrimeScene
{
	public class MessageShow : MonoBehaviour 
	{
		public Text messageText;
		string[] messages;
		int i = 0;
		public void getMessages(string[] message)
		{
			messageText = transform.Find("Message").GetComponent<Text>();
			i = 0;
			messages = message;
		}
		
		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (i >= messages.Length)
				{
					CrimeManager.Instance.isGUI = false;
					this.gameObject.SetActive(false);
					return;
				}
				messageText.text = messages[i];
				i++;
			}
		}
	}

}