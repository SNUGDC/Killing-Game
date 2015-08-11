using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class ItemGainer : Function
	{		
		public override void Run()
		{
			if (target == null)
				return;
			CrimeManager.Instance.ObtainItem(target);
		}
	}
}