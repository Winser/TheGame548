using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemContainer : MonoBehaviour
{
	public static ItemContainer lastContainer;
	public Transform target;
	public AnimationClip anim;
	public float speed;
	public bool closeOnPlayerMove;
	private bool isOpen;
	private Vector3 lastPlayerPosition;
	public float openRange = 5;
	public bool updateWindowPosition;
	public bool addItems;
	public int min;
	public int max;
	public ItemTable table;
	[HideInInspector]
	public List<BaseItem> items;
	private List<ContainerItemSlot> slots;
	private bool clicked;
	
	private void Start ()
	{
		items = new List<BaseItem> ();
		slots = new List<ContainerItemSlot> ();
		if (addItems) {
			int rnd = Random.Range (min + 1, max);
		
			for (int i =1; i<rnd; i++) {
				BaseItem item = table.GetRandomItem ();
				if (item != null) {
					items.Add (item);
				}
			}
		}
		InterfaceContainer.Instance.containerWindow.SetActive (true);
		foreach (ContainerItemSlot slot in InterfaceContainer.Instance.containerTable.GetComponentsInChildren<ContainerItemSlot>()) {
			slots.Add (slot);
		}
		InterfaceContainer.Instance.containerWindow.SetActive (false);
	}
	
	public void RefreshContainer ()
	{
		
		foreach (ContainerItemSlot slot in slots) {
			slot.sprite.gameObject.SetActive (false);
			slot.stack.text = "";
		}
		
		for (int i=0; i< items.Count; i++) {
			slots [i].sprite.gameObject.SetActive (true);
			slots [i].sprite.spriteName = items [i].icon;
			slots [i].stack.text = items [i].stack.ToString ();
			slots [i].item = items [i];
		}
	}
	
	public void OnMouseUp ()
	{
		if (ItemContainer.lastContainer == this && ItemContainer.lastContainer.isOpen) {
			return;
		}
		

		if (ItemContainer.lastContainer != null && ItemContainer.lastContainer.isOpen) {
			ItemContainer.lastContainer.Close ();
		}
		
		if ( Vector3.Distance (GameManager.Player.transform.position, transform.position) < openRange) {
			if (!isOpen) {
				InterfaceContainer.Instance.containerWindow.SetActive (true);
		
				Vector3 v3 = PlayerCamera.Instance.GetComponent<Camera>().WorldToViewportPoint (transform.position); 
				v3 = UICamera.mainCamera.ViewportToWorldPoint (v3); 
				InterfaceContainer.Instance.containerWindow.transform.position = new Vector3 (v3.x, v3.y, 0);
				ItemContainer.lastContainer = this;
				RefreshContainer ();
				target.GetComponent<Animation>() [anim.name].speed = speed;
				target.GetComponent<Animation>().Play (anim.name);
				lastPlayerPosition = GameManager.Player.transform.position;
			}
		}else{
			MessageManager.Instance.AddMessage(GameManager.GameMessages.farAway);
		}
	}
	
	public void Close ()
	{
		target.GetComponent<Animation>() [anim.name].speed = -speed;
		target.GetComponent<Animation>() [anim.name].time = target.GetComponent<Animation>() [anim.name].length;
		target.GetComponent<Animation>().Play (anim.name);
		InterfaceContainer.Instance.containerWindow.SetActive (false);
		isOpen = false;
	}
	
	private void Update ()
	{
		if (isOpen) {
			if (updateWindowPosition) {
				Vector3 v3 = PlayerCamera.Instance.GetComponent<Camera>().WorldToViewportPoint (transform.position); 
				v3 = UICamera.mainCamera.ViewportToWorldPoint (v3); 
				InterfaceContainer.Instance.containerWindow.transform.position = new Vector3 (v3.x, v3.y, 0);
			}
			
			if (closeOnPlayerMove) {
				float delta = (GameManager.Player.transform.position - lastPlayerPosition).sqrMagnitude;
				if (delta > 0.1f) {
					Close ();
					InterfaceContainer.Instance.toolTipWindow.SetActive (false);
				}
			}
		}
	}
	
	private void OnMouseEnter ()
	{
		SlotContainer.disableMouseTalent = true;
	}
	
	private void OnMouseExit ()
	{
		SlotContainer.disableMouseTalent = false;
	}
}
