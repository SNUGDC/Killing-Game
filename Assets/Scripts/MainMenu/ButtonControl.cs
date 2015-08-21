using UnityEngine;
using System.Collections;

namespace KillingGame
{
	public class ButtonControl : MonoBehaviour 
	{
		public GameObject Buttons;
		public GameObject Slots;
		
		public bool isNewGame;
		
		public void NewGameButton()
		{
			isNewGame = true;
			Buttons.SetActive(false);
			Slots.SetActive(true);
		}
		
		public void LoadGameButton()
		{
			isNewGame = false;
			Buttons.SetActive(false);
			Slots.SetActive(true);
		}
		
		public void Slot1Button()
		{
			if (isNewGame)
				GameManager.NewGame(1);
			else
				GameManager.LoadGame(1);
		}
		
		public void Slot2Button()
		{
			if (isNewGame)
				GameManager.NewGame(2);
			else
				GameManager.LoadGame(2);
		}
	}	
}