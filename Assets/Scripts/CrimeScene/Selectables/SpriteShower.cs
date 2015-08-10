using UnityEngine;
using System.Collections.Generic;

namespace KillingGame.CrimeScene
{
	public class SpriteShower : Function
	{
		public List<Sprite> sprites = new List<Sprite>();
		public void Run()
		{
			CrimeManager.Instance.ShowSprite(sprites);
		}
		
		public int ReturnIndex()
		{
			return 6;
		}
	}
}
