using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class SelectableButton : MonoBehaviour 
	{
		public Selectable selectable;
		public CrimeObject crimeObject;
		void OnMouseDown()
		{
			if (selectable!=null)
				selectable.execute();
			crimeObject.onCancelThis();
		}
	}	
}

