using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class DangerChanger : Function
	{
		public int newDanger;
		
		public void Run()
		{
			SelectManager selector = target.GetComponent<SelectManager>();
			if (selector != null)
				selector.dangers.dangerCount = newDanger;
		}
		public int ReturnIndex()
		{
			return 5;
		}
	}
}
