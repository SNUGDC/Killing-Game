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
	public class CrimeObject : MonoBehaviour, IEnable 
	{
		public bool isActive;
		public Sprite baseSprite;
		public Sprite selectedSprite;
		Dictionary<string, Selectable> selectables;

		GameObject[] selectButtons;
		
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
		
		void Start()
		{
			baseSprite = GetComponent<SpriteRenderer>().sprite;
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
				isActive = false;
				return;
			}
			selectables[label].isActive = false;
		}
		public void enableSelectable(string label)
		{
			if (label == "Object")
			{
				isActive = true;
				return;
			}
			selectables[label].isActive = true;
		}
		public void eraseSelectable(string label)
		{
			if (label == "Object")
			{
				Destroy(gameObject);
				return;
			}
			Destroy(selectables[label]);
		}
		public void onTouchThis()
		{
			if (!isActive || CrimeManager.Instance.isGUI)
				return;
			CrimeManager.Instance.isGUI = true;
			if (selectedSprite != null)
				GetComponent<SpriteRenderer>().sprite = selectedSprite;
			Dictionary<string, Selectable> activeDic = new Dictionary<string, Selectable>();
			foreach (var item in selectables)
			{
				if (item.Value.isActive)
					activeDic.Add(item.Key, item.Value);
			}
			selectButtons = new GameObject[activeDic.Count + 1];
			int i = 0;
			foreach (var item in activeDic)
			{
				selectButtons[i] = Instantiate(Resources.Load("Prefabs/Select")) as GameObject;
				selectButtons[i].transform.position = transform.position + 1.5f * i * Vector3.down + 3 * Vector3.right;
				selectButtons[i].GetComponent<SelectableButton>().crimeObject = this;
				selectButtons[i].GetComponent<SelectableButton>().selectable = item.Value;
				i++;
			}
			selectButtons[i] = Instantiate(Resources.Load("Prefabs/Select")) as GameObject;
			selectButtons[i].transform.position = transform.position + 1.5f * i * Vector3.down + 3 * Vector3.right;
			selectButtons[i].GetComponent<SelectableButton>().crimeObject = this;
		}
		public void onCancelThis()
		{
			GetComponent<SpriteRenderer>().sprite = baseSprite;
			foreach (GameObject button in selectButtons)
			{
				Destroy(button);
			}
			CrimeManager.Instance.isGUI = false;
		}
		
		void OnMouseDown()
		{
			onTouchThis();
		}
	}
}