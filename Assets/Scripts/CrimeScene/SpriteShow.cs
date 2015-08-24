using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace KillingGame.CrimeScene
{
	public class SpriteShow : MonoBehaviour 
	{
		Image spriter;
		List<Sprite> sprites;
		int showCounter;
		public void GetSprites(List<Sprite> sprites)
		{
			spriter = transform.Find("Sprites").GetComponent<Image>();
			showCounter = 0;
			this.sprites = sprites;
			spriter.sprite = sprites[showCounter];
			showCounter++;
		}
		
		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (showCounter >= sprites.Count)
				{
					CrimeManager.Instance.isGUI = false;
					this.gameObject.SetActive(false);
					return;
				}
				spriter.sprite = sprites[showCounter];
				showCounter++;
			}
		}
	}	
}

