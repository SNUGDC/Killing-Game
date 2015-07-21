using UnityEngine;
using System.Collections.Generic;

namespace KillingGame.CrimeScene
{
	public enum EnableOption
	{
		enable, disable, erase
	}
	public interface IEnable
	{
		void SetEnable(EnableOption option);
	}
	[System.Serializable]
	public class CrimeObjectManager : IEnable 
	{
		public GameObject body;
		public string label;
		public bool useAsRoute = false;
		public bool isActive = true;
		public Sprite baseSprite;
		public Sprite selectedSprite;
		public List<SelectManager> selectList = new List<SelectManager>();
		
		GameObject[] selectButtons;
		
		public void SetEnable(EnableOption option)
		{
			switch (option)
			{
				case EnableOption.enable:
					isActive = true;
					if (useAsRoute)
						foreach (SelectManager selection in selectList)
						{
							selection.ExecuteSelect();
						}
					break;
				case EnableOption.disable:
					isActive = false;
					break;
				case EnableOption.erase:
					GameObject.Destroy(body);
					break;
			}
		}
		
		public List<SelectManager> GetActiveList()
		{
			List<SelectManager> result = new List<SelectManager>();
			
			foreach (SelectManager selection in selectList)
			{
				if (selection.isActive = true)
					result.Add(selection);
			}
			
			return result;
		}
		
		public void GetCopy(CrimeObjectManager targetObject)
		{
			targetObject.body = body;
			targetObject.label = label;
			targetObject.useAsRoute = useAsRoute;
			targetObject.isActive = isActive;
			targetObject.baseSprite = baseSprite;
			targetObject.selectedSprite = selectedSprite;
			//  targetObject.List<SelectManager> selectList = new List<SelectManager>();
		}
	}
}