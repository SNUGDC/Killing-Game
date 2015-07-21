using UnityEngine;
using System.Collections.Generic;

namespace KillingGame.CrimeScene
{
	public interface IExecutable
	{
		void Execute();
		int ReturnIndex();
		void SetTarget(IEnable target);
	}
	
	[System.Serializable]	
	public class SelectManager : IEnable
	{
		public string label = "";
		public bool isActive = true;
		public bool isOnce = false;
		public bool isDestroyed = false;
		public float requireTime;
		public int displayOrder;
		
		public class Dangers
		{
			public bool isActive = true;
			public int dangerCount;
			public void ApplyDanger()
			{
				if (isActive)
					CrimeManager.Instance.dangerCount += dangerCount;
			}
		}
		
		public List<IExecutable> functions = new List<IExecutable>();
		
		public Dangers dangers = new Dangers();
		
		public void SetEnable(EnableOption option)
		{
			if (isDestroyed)
				return;
			switch (option)
			{
				case EnableOption.enable:
					isActive = true;
					break;
				case EnableOption.disable:
					isActive = false;
					break;
				case EnableOption.erase:
					isActive = false;
					isDestroyed = true;
					break;
			}
		}
		
		public void ExecuteSelect()
		{
			if (!isActive)
				return;
			foreach (IExecutable item in functions)
			{
				item.Execute();
			}
			CrimeManager.Instance.SpendTime(requireTime);
			dangers.ApplyDanger();
			if (isOnce)
				isDestroyed = true;
		}
	}
}