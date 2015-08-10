using UnityEngine;
using System;

namespace KillingGame.CrimeScene
{
	public class MessageDisplayer : Function
	{
		public string inputMessage;
		public override void Run()
		{
			if (inputMessage == "" || inputMessage == null)
				return;
			string[] messages = inputMessage.Split(new [] {'@'});
			CrimeManager.Instance.ShowMessage(messages);
		}
	}
}