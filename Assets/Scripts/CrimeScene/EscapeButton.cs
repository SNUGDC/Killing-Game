using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class EscapeButton : MonoBehaviour
	{
		void OnMouseDown()
		{
			if (!CrimeManager.Instance.isGUI)
				CrimeManager.Instance.ExitCrimeScene();
		}
	}
}
