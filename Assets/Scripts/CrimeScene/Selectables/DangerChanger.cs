using UnityEngine;
using System;

namespace KillingGame.CrimeScene
{
	[System.Serializable]	
	public class DangerChanger : IExecutable
	{
		public SelectManager target;
		public int newDanger;
		
		public void Execute()
		{
			if (target != null)
				target.dangers.dangerCount = newDanger;
		}
		public int ReturnIndex()
		{
			return 5;
		}
		
		public void SetTarget(IEnable target)
		{
			try
			{
				this.target = (SelectManager)target;	
			}
			catch (InvalidCastException e)
			{
				this.target = null;
			}
		}
	}
}
