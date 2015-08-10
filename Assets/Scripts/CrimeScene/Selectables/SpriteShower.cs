using UnityEngine;
using System.Collections.Generic;

namespace KillingGame.CrimeScene
{
	public class SpriteShower : Function
	{
		public List<Sprite> sprites = new List<Sprite>();
		public override void Run()
		{
			CrimeManager.Instance.ShowSprite(sprites);
		}
	}
}
