using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace KillingGame.CrimeScene
{
	public class SpriteShower : Function
	{
		public Sprite[] sprites;
		public override void Run()
		{
			if (sprites != null)
				CrimeManager.Instance.ShowSprite(sprites.ToList());
		}
	}
}
