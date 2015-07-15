using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class Enabler : MonoBehaviour, IExecutable
	{
		public EnableOption option;
		public GameObject target;

		public void Execute()
		{
			IEnable enable = target.GetComponent<IEnable>();
			if (enable != null)
				enable.SetEnable(option);
		}
	}
}
