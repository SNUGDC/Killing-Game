using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{	
	public class CrimeSceneObject : MonoBehaviour 
	{
		public string hey = "hey";
		
		void OnMouseDown()
		{
			Debug.Log(name);
		}
	}	
}

