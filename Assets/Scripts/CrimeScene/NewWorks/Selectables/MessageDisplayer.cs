using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class MessageDisplayer : MonoBehaviour, IExecutable
	{
		public string[] messages;
		public void Execute()
		{
			CrimeManager.Instance.ShowMessage(messages);
		}
	}
}