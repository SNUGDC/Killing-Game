using UnityEngine;
using System.Collections;

namespace KillingGame.CrimeScene
{
	public class ItemButton : MonoBehaviour
	{
		public SpriteRenderer spriteRenderer;
		public GameObject backGround;
		public Transform buttonTrans;
		public CrimeObject target;
		
		public void SetTarget(CrimeObject newTarget)
		{
			if (newTarget == null || !newTarget.isItem)
				return;
			target = newTarget;
			target.buttonTrans = buttonTrans;
			backGround.SetActive(true);
			spriteRenderer.sprite = target.baseSprite;
		}
		public void SetInactive()
		{
			target.buttonTrans = null;
			target = null;
			backGround.SetActive(false);
			spriteRenderer.sprite = null;
		}
		void OnMouseDown()
		{
			if (target == null)
				return;
			target.OnTouchThis();
		}
	}
}