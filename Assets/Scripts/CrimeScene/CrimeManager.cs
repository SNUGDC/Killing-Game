using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class CrimeManager : MonoBehaviour
	{
		public static CrimeManager Instance;
		void Awake()
		{
			Instance = this;
		}
		
		public void obtainItem(GameObject item)
		{
			
		}
		
		public void showMessage(string texts)
		{
			
		}
		
		public void spendTime(float timeSpent)
		{
			
		}
	}	
}

