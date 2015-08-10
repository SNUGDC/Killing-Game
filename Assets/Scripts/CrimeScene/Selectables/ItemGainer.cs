using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class ItemGainer : Function
	{		
		public void Run()
		{
			if (target == null)
				return;
			CrimeManager.Instance.obtainItem(target);
		}
		
		public int ReturnIndex()
		{
			return 4;
		}	
	}
}