using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	[System.Serializable]
	public class Enabler : IExecutable
	{
		public EnableOption option;
		public IEnable target;

		public void Execute()
		{
			if (target != null)
				target.SetEnable(option);
		}
		public int ReturnIndex()
		{
			return 1;
		}
		
		public void SetTarget(IEnable target)
		{
			this.target = target;
		}
	}
}
