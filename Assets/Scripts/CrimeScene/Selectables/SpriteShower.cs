using UnityEngine;
using System.Collections.Generic;

namespace KillingGame.CrimeScene
{
	public class SpriteShower : MonoBehaviour, IExecutable
	{
		public List<Sprite> sprites = new List<Sprite>();
		public void Execute()
		{
			CrimeManager.Instance.ShowSprite(sprites);
		}
		
		public int ReturnIndex()
		{
			return 6;
		}
		
		public void SetTarget(GameObject target)
		{
			
		}
	}
}
