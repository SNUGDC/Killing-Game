using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public enum Functions
	{
		erase, disable, enable
	}
	public class Selectable : MonoBehaviour 
	{
		public int enables;
		public int disables;
		public string label;
		public bool isActive;
		public float requireTime;
		
		[System.Serializable]
		public class Result
		{
			public Functions function;
			public GameObject target;
			public string label;
			public void execute()
			{
				switch(function)
				{
					case Functions.enable:
						target.GetComponent<CrimeObject>().enableSelectable(label);
					break;
					case Functions.disable:
						target.GetComponent<CrimeObject>().disableSelectable(label);
					break;
					case Functions.erase:
						target.GetComponent<CrimeObject>().eraseSelectable(label);					
					break;
				}
			}
		}
		
		[System.Serializable]
		public class Obtain
		{
			public GameObject item;
			public void execute()
			{
				CrimeManager.Instance.obtainItem(item);
			}
		}

		[System.Serializable]
		public class ChangeSprite
		{
			public GameObject target;
			public Sprite newSprite;
			
			public void execute()
			{
				target.GetComponent<SpriteRenderer>().sprite = newSprite;
				target.GetComponent<CrimeObject>().baseSprite = newSprite;
			}
		}
		
		public Result[] results;
		public Obtain[] obtains;
		public ChangeSprite[] spriteChanges;
		public string[] showMessages;
		
		public void execute()
		{
			if (!isActive)
				return;
			
			CrimeManager.Instance.spendTime(requireTime);
			
			foreach (Result result in results)
			{
				result.execute();
			}
			foreach (Obtain obtain in obtains)
			{
				obtain.execute();
			}
			foreach (ChangeSprite changesprite in spriteChanges)
			{
				changesprite.execute();
			}
			if (showMessages != null)
			{
				CrimeManager.Instance.showMessage(showMessages);
			}
		}
	}	
}

