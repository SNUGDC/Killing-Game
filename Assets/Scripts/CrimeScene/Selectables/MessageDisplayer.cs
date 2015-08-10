using UnityEngine;
using System;

namespace KillingGame.CrimeScene
{
	public class MessageDisplayer : Function
	{
		public string inputMessage;
		public void Execute()
		{
			if (inputMessage == "" || inputMessage == null)
				return;
			string[] messages = inputMessage.Split(new [] {'@'});
			CrimeManager.Instance.ShowMessage(messages);
		}
		public int ReturnIndex()
		{
			return 3;
		}
		public void SetTarget(GameObject target)
		{
			
		}
	}
}