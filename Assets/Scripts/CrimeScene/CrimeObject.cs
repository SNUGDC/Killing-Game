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
		public bool isEditor = true;
		public bool useAsRoute = false;
		public bool isActive = true;
		public Sprite _baseSprite;
		public Sprite baseSprite
		{
			get
			{
				return _baseSprite;
			}
			set
			{
				_baseSprite = value;
				if (isEditor)
					return;
				Collider2D coll = gameObject.GetComponent<Collider2D>();
				if (coll != null)
					Destroy(coll);
				gameObject.AddComponent<PolygonCollider2D>();
			}
		}
		public Sprite selectedSprite;
		public List<GameObject> selectList = new List<GameObject>();
		List<GameObject> activeList = new List<GameObject>();
		
		GameObject[] selectButtons;
		
		void Start()
		{
			isEditor = false;
			foreach (Transform child in transform)
			{
				selectList.Add(child.gameObject);
			}
		}
		
		public void SetEnable(EnableOption option)
		{
			switch (option)
			{
				case EnableOption.enable:
					isActive = true;
					if (useAsRoute)
						foreach (Transform child in transform)
						{
							child.gameObject.SendMessage("Execute");
						}
					break;
				case EnableOption.disable:
					isActive = false;
					break;
				case EnableOption.erase:
					Destroy(gameObject);
					break;
			}
		}
		
		public void onTouchThis()
		{
			if (!isActive || CrimeManager.Instance.isGUI)
				return;
			if (selectedSprite != null)
				GetComponent<SpriteRenderer>().sprite = selectedSprite;
			
			activeList = new List<GameObject>();
			
			foreach (GameObject item in selectList)
			{
				// there is a bug which is happen when item is deleted.
				// When deleting item, programmer should update selectList.
				// This code is temporary fix.
				if (item == null) {
					continue;
				}

				if (item.GetComponent<SelectManager>().isActive)
					activeList.Add(item);
			}
			if (activeList.Count == 0)
				return;
			CrimeManager.Instance.isGUI = true;
			selectButtons = new GameObject[activeList.Count + 1];
			int i = 0;
			foreach (GameObject item in activeList)
			{
				selectButtons[i] = CrimeManager.Instance.GetButton();
				selectButtons[i].transform.Find("Label").GetComponent<TextMesh>().text = item.name;
				selectButtons[i].transform.position = transform.position + 1f * i * Vector3.down + 3 * Vector3.right;
				selectButtons[i].GetComponent<SelectableButton>().crimeObject = this;
				selectButtons[i].GetComponent<SelectableButton>().selectable = item;
				i++;
			}
			selectButtons[i] = CrimeManager.Instance.GetButton();
			selectButtons[i].transform.Find("Label").GetComponent<TextMesh>().text = "취소";
			selectButtons[i].transform.position = transform.position + 1f * i * Vector3.down + 3 * Vector3.right;
			selectButtons[i].GetComponent<SelectableButton>().crimeObject = this;
			selectButtons[i].GetComponent<SelectableButton>().selectable = null;
		}
		public void onCancelThis()
		{
			GetComponent<SpriteRenderer>().sprite = baseSprite;
			foreach (GameObject button in selectButtons)
			{
				button.GetComponent<SelectableButton>().crimeObject = null;
				button.GetComponent<SelectableButton>().selectable = null;
				button.SetActive(false);
			}
			if (!CrimeManager.Instance.messageHolder.activeSelf || !CrimeManager.Instance.spriteShower.activeSelf)
				CrimeManager.Instance.isGUI = false;
		}
		
		void OnMouseDown()
		{
			onTouchThis();
		}
	}
}