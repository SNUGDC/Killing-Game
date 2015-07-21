using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	[System.Serializable]	
	public class ItemGainer : IExecutable
	{
		public IEnable target;	
		
		public void Execute()
		{
			if (target == null)
				return;
			//  CrimeManager.Instance.obtainItem(target);
		}
		
		public int ReturnIndex()
		{
			return 4;
		}

		public void SetTarget(IEnable target)
		{
			this.target = target;
		}		
	}
}