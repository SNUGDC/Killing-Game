using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class DangerChanger : Function
	{
		public int newDanger;
		
		public override void Run()
		{
			SelectManager selector = target.GetComponent<SelectManager>();
			if (selector != null)
				selector.dangers.dangerCount = newDanger;
		}
	}
}
