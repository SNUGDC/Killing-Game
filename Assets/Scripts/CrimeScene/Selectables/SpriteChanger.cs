using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class SpriteChanger : Function
	{
		public Sprite baseSprite;
		public Sprite selectedSprite;
		
		public void Run()
		{
			if (!target)
				return;
			CrimeObject crimeObject = target.GetComponent<CrimeObject>();
			if (crimeObject == null)
				return;
			if (baseSprite != null)
			{
				crimeObject.baseSprite = baseSprite;
				target.GetComponent<SpriteRenderer>().sprite = baseSprite;		
			}
			if (selectedSprite != null)
				crimeObject.selectedSprite = selectedSprite;
		}
		public int ReturnIndex()
		{
			return 0;
		}
	}
}
