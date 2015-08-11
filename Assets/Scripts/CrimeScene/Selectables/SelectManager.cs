using UnityEngine;
using System.Collections.Generic;

namespace KillingGame.CrimeScene
{
	public abstract class Function : MonoBehaviour
	{
		public GameObject target;
		public float delay;
		public void Execute()
		{
			Invoke("Run", delay);
		}
		public virtual void Run()
		{
			
		}

		public void SetTarget(GameObject target)
		{
			this.target = target;
		}
	}
	
	public class SelectManager : MonoBehaviour, IEnable
	{
		public bool isActive = true;
		public bool isOnce = false;
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
					transform.parent.GetComponent<CrimeObject>().selectList.Remove(this);
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
			if (isOnce)
			{
				transform.parent.GetComponent<CrimeObject>().selectList.Remove(this);
				Destroy(gameObject);
			}
		}
	}
}