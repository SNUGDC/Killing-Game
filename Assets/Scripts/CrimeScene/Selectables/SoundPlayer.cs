using UnityEngine;
using System;

namespace KillingGame.CrimeScene
{
	public class SoundPlayer : Function
	{
		public AudioClip sound;
		public override void Run()
		{
			if (sound == null)
				return;
			CrimeManager.Instance.PlaySound(sound);
		}
	}
}