using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class ItemGainer : Function
	{
		public GameObject target;
		
		
		public void Execute()
		{
			if (target == null)
				return;
			CrimeManager.Instance.obtainItem(target);
		}
		
		public int ReturnIndex()
		{
			return 4;
		}

		public void SetTarget(GameObject target)
		{
			this.target = target;
		}		
	}
}