using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class SelectableButton : MonoBehaviour 
	{
		public GameObject selectable;
		public CrimeObject crimeObject;
		void OnMouseDown()
		{
			if (selectable)
			{
				selectable.GetComponent<SelectManager>().ExecuteSelect();
			}
			crimeObject.onCancelThis();
		}
	}	
}