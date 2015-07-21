using UnityEngine;
using System;

namespace KillingGame.CrimeScene
{
	[System.Serializable]	
	public class SpriteChanger : IExecutable
	{
		public CrimeObjectManager target;
		public Sprite baseSprite;
		public Sprite selectedSprite;
		
		public int ReturnIndex()
		{
			return 0;
		}
		
		public void SetTarget(IEnable target)
		{
			try
			{
				this.target = (CrimeObjectManager)target;	
			}
			catch (InvalidCastException e)
			{
				this.target = null;
			}
		}
		
		public void Execute()
		{
			if (target == null)
				return;
			
			target.baseSprite = baseSprite;
			target.body.GetComponent<SpriteRenderer>().sprite = baseSprite;		
			target.selectedSprite = selectedSprite;
			return;
		}
	}
}
