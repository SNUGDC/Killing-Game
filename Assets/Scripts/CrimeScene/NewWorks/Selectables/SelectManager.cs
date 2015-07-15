using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public interface IExecutable
	{
		void Execute();
	}
	
	public class SelectManager : MonoBehaviour, IEnable
	{
		public bool isActive;
		public string label;
		public float requireTime;
		public int displayOrder;
		
		[System.Serializable]
		public class Dangers
		{
			public bool isActive;
			public int dangerCount;
			public void ApplyDanger()
			{
				if (isActive)
					CrimeManager.Instance.dangerCount += dangerCount;
			}
		}
		public Dangers dangers;
		
		public void SetEnable(EnableOption option)
		{
			switch (option)
			{
				case EnableOption.enable:
					isActive = true;
					break;
				case EnableOption.disable:
					isActive = false;
					break;
				case EnableOption.erase:
					Destroy(gameObject);
					break;
			}
		}
		
		void OnMouseDown()
		{
			if (!isActive || CrimeManager.Instance.isGUI)
				return;
			
			CrimeManager.Instance.SpendTime(requireTime);
			dangers.ApplyDanger();
			gameObject.SendMessage("Execute");
		}
	}
}