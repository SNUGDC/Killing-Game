using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class SelectableButton : MonoBehaviour 
	{
		public SelectManager selectable;
		public CrimeObject crimeObject;
		void OnMouseDown()
		{
			if (selectable)
			{
				selectable.ExecuteSelect();
			}
			crimeObject.OnCancelThis();
		}
	}	
}