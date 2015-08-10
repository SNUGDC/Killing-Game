using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class Enabler : Function
	{
		public EnableOption option;

		public void Run()
		{
			IEnable enable = target.GetComponent<IEnable>();
			if (enable != null)
				enable.SetEnable(option);
		}
		public int ReturnIndex()
		{
			return 1;
		}
	}
}
