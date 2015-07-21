using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class DangerChanger : MonoBehaviour, IExecutable
	{
		public GameObject target;
		public int newDanger;
		
		public void Execute()
		{
			SelectManager selector = target.GetComponent<SelectManager>();
			if (selector != null)
				selector.dangers.dangerCount = newDanger;
		}
		public int ReturnIndex()
		{
			return 5;
		}
		
		public void SetTarget(GameObject target)
		{
			this.target = target;
		}
	}
}
