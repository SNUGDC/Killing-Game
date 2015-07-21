using UnityEngine;
using System;

namespace KillingGame.CrimeScene
{
	[System.Serializable]	
	public class SoundPlayer : IExecutable
	{
		public AudioClip sound;
		public void Execute()
		{
			if (sound == null)
				return;
			CrimeManager.Instance.PlaySound(sound);
		}
		public int ReturnIndex()
		{
			return 2;
		}
		public void SetTarget(IEnable target)
		{
			
		}
	}
}