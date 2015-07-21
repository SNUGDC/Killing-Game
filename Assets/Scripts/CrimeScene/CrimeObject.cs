using UnityEngine;
using System.Collections.Generic;

namespace KillingGame.CrimeScene
{
	public class CrimeObject : MonoBehaviour
	{
		void Start()
		{
			if (_manager == null)
				Debug.Log("Sigh");
			Debug.Log(_manager.isActive);
		}
		
		private CrimeObjectManager _manager = new CrimeObjectManager();
		public CrimeObjectManager Manager
		{
			get	
			{
				if (_manager == null)
					_manager = new CrimeObjectManager();
				return _manager;
			}
			set
			{
				_manager = value;
				if (rend == null)
					rend = gameObject.AddComponent<SpriteRenderer>();
				rend.sprite = _manager.selectedSprite;
				gameObject.name = _manager.label;
			}
		}
		public void Init()
		{
			if (rend == null)
				rend = gameObject.AddComponent<SpriteRenderer>();
			rend.sprite = _manager.selectedSprite;
			gameObject.name = _manager.label;
		}
		GameObject[] selectButtons;
		SpriteRenderer rend;
		public void OnTouchThis()
		{
			if (_manager == null)
				return;
			if (!_manager.isActive || CrimeManager.Instance.isGUI)
				return;
			CrimeManager.Instance.isGUI = true;
			if (_manager.selectedSprite != null)
				GetComponent<SpriteRenderer>().sprite = _manager.selectedSprite;
			
			List<SelectManager> activeList = _manager.GetActiveList();
			
			if (activeList.Count == 0)
				return;
			selectButtons = new GameObject[activeList.Count + 1];
			int i = 0;
			foreach (SelectManager item in activeList)
			{
				selectButtons[i] = CrimeManager.Instance.GetButton();
				selectButtons[i].transform.Find("Label").GetComponent<TextMesh>().text = item.label;
				selectButtons[i].transform.position = transform.position + 1f * i * Vector3.down + 3 * Vector3.right;
				selectButtons[i].GetComponent<SelectableButton>().crimeObject = this;
				selectButtons[i].GetComponent<SelectableButton>().selectable = item;
				i++;
			}
			selectButtons[i] = Instantiate(Resources.Load("Prefabs/UI/Select")) as GameObject;
			selectButtons[i].transform.Find("Label").GetComponent<TextMesh>().text = "취소";
			selectButtons[i].transform.position = transform.position + 1f * i * Vector3.down + 3 * Vector3.right;
			selectButtons[i].GetComponent<SelectableButton>().crimeObject = this;
		}
		public void OnCancelThis()
		{
			Debug.Log("Clicked");
			GetComponent<SpriteRenderer>().sprite = _manager.baseSprite;
			foreach (GameObject button in selectButtons)
			{
				Destroy(button);
			}
			if (!CrimeManager.Instance.messageHolder.activeSelf || !CrimeManager.Instance.spriteShower.activeSelf)
				CrimeManager.Instance.isGUI = false;
		}
		
		void OnMouseDown()
		{
			OnTouchThis();
		}
	}
}