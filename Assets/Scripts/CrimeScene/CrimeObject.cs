using UnityEngine;
using System.Collections;
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
		public bool isDestroyed = false;
		public bool isEditor = true;
		public bool useAsRoute = false;
		public bool isItem = false;
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
				if (isEditor || isItem)
				{
					_baseSprite = value;
				}
				else
				{
					StartCoroutine(ChangeSprite(value));
				}
			}
		}
		public Sprite selectedSprite;
		public List<SelectManager> selectList = new List<SelectManager>();
		List<SelectManager> activeList = new List<SelectManager>();
		
		public Transform buttonTrans;
		GameObject[] selectButtons;
		
		public void Apply()
		{
			Collider2D coll = gameObject.GetComponent<Collider2D>();
			if (coll != null)
				DestroyImmediate(coll);
			if (!isItem)
			{
				GetComponent<SpriteRenderer>().sprite = baseSprite;
				gameObject.AddComponent<PolygonCollider2D>();
			}	
			transform.localPosition = Vector3.zero;
		}
		
		void Start()
		{
			isEditor = false;
			foreach (Transform child in transform)
			{
				SelectManager selectManager = child.GetComponent<SelectManager>();
				if (selectManager != null)
					selectList.Add(selectManager);
			}
		}
		
		public void SetEnable(EnableOption option)
		{
			if (isDestroyed)
				return;
			switch (option)
			{
				case EnableOption.enable:
					isActive = true;
					if (useAsRoute)
						foreach (Transform child in transform)
						{
							if (child.GetComponent<SelectManager>() == null)
								continue;
							child.GetComponent<SelectManager>().ExecuteSelect();
						}
					break;
				case EnableOption.disable:
					isActive = false;
					break;
				case EnableOption.erase:
					isActive = false;
					isDestroyed = true;
					baseSprite = null;
					break;
			}
		}
		
		
		
		public void OnTouchThis()
		{
			if (isDestroyed || !isActive || CrimeManager.Instance.isGUI)
				return;
			if (!isItem && selectedSprite != null)
				GetComponent<SpriteRenderer>().sprite = selectedSprite;
			
			activeList = new List<SelectManager>();
			
			foreach (SelectManager item in selectList)
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
			if (activeList.Count == 0 || buttonTrans == null)
				return;
			CrimeManager.Instance.isGUI = true;
			
			selectButtons = new GameObject[activeList.Count + 1];
			int i = 0;
			foreach (SelectManager item in activeList)
			{
				selectButtons[i] = CrimeManager.Instance.GetButton();
				selectButtons[i].transform.Find("Label").GetComponent<TextMesh>().text = item.name;
				selectButtons[i].transform.position = buttonTrans.position + 1f * i * Vector3.down + 3 * Vector3.right;
				Vector3 pos = selectButtons[i].transform.position;
				selectButtons[i].transform.position = new Vector3(pos.x, pos.y, -7);
				selectButtons[i].GetComponent<SelectableButton>().crimeObject = this;
				selectButtons[i].GetComponent<SelectableButton>().selectable = item;
				i++;
			}
			selectButtons[i] = CrimeManager.Instance.GetCanceler();
			selectButtons[i].GetComponent<SelectableButton>().crimeObject = this;
		}
		public void OnCancelThis()
		{
			if (!isItem)
				GetComponent<SpriteRenderer>().sprite = _baseSprite;
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
			OnTouchThis();
		}
		
		IEnumerator ChangeSprite(Sprite newSprite)
		{
			Collider2D coll = gameObject.GetComponent<Collider2D>();
			if (coll != null)
				Destroy(coll);
			
			GameObject tempSpritor = CrimeManager.Instance.GetTempSpritor();
			SpriteRenderer thisRenderer = GetComponent<SpriteRenderer>();
			SpriteRenderer tempRenderer = tempSpritor.GetComponent<SpriteRenderer>();
			tempRenderer.sprite = _baseSprite;

			float timer = 0;
			
			while (timer <= 1)
			{
				thisRenderer.sprite = newSprite;
				thisRenderer.color = new Color(1, 1, 1, timer);
				tempRenderer.color = new Color(1, 1, 1, 1 - timer);
				timer += Time.deltaTime;
				yield return null;
			}
			
			_baseSprite = newSprite;
			thisRenderer.color = new Color(1, 1, 1, 1);
			tempSpritor.SetActive(false);

			if (newSprite != null)
				gameObject.AddComponent<PolygonCollider2D>();
			yield break;
		}
	}
}