using UnityEngine;
using System.Collections.Generic;

namespace KillingGame.CrimeScene
{
	public class CrimeObject : MonoBehaviour 
	{
		public bool isActive;
		Dictionary<string, Selectable> selectables;
		void Start()
		{
			selectables = new Dictionary<string, Selectable>();
			foreach(Selectable item in GetComponents<Selectable>())
			{
				selectables.Add(item.label, item);
			}
		}
		
		public void disableSelectable(string label)
		{
			if (label == "Object")
			{
				
			}
		}
		public void enableSelectable(string label)
		{
			if (label == "Object")
			{
				
			}
		}
		public void onTouchThis()
		{
			if (!isActive)
				return;
			Dictionary<string, Selectable> activeDic = new Dictionary<string, Selectable>();
			foreach(var item in selectables)
			{
				if (item.Value.isActive)
					activeDic.Add(item.Key, item.Value);
			}
			
		}
	}

}