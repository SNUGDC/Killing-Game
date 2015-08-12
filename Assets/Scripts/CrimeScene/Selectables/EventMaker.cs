using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class EventMaker : Function
	{
		public string eventFlag;
		
		public override void Run()
		{
			CrimeManager.Instance.RegisterEvent(eventFlag);
		}
	}
}