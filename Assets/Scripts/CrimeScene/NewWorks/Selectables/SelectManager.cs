using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public interface IExecutable
	{
		void Execute();
		int ReturnIndex();
		void SetTarget(GameObject target);
	}
	
	public class SelectManager : MonoBehaviour, IEnable
	{
		public bool isActive = true;
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
		public Dangers dangers = new Dangers();
		
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
		
		public void ExecuteSelect()
		{
			if (!isActive)
				return;
			gameObject.SendMessage("Execute");
			CrimeManager.Instance.SpendTime(requireTime);
			dangers.ApplyDanger();
		}
	}
}