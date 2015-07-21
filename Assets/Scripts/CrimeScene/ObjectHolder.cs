using UnityEngine;
using System.Collections.Generic;

namespace KillingGame.CrimeScene
{
	public class ObjectHolder : MonoBehaviour
	{
		public CrimeObjectManager[] managers = new CrimeObjectManager[0];
		
		public CrimeObjectManager GetManager()
		{
			CrimeObjectManager[] newManagers = new CrimeObjectManager[managers.Length + 1];
			for (int i=1; i<managers.Length; i++)
			{
				newManagers[i] = managers[i];
			}
			newManagers[managers.Length] = new CrimeObjectManager();
			managers = newManagers;
			
			return managers[managers.Length - 1];
		} 
	}
}