using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lootable : MonoBehaviour
{
	public BaseItem item;
	public float pickUpDistance=5;
	public bool lookAtItem=true;
	public bool playPickUpAnimation=true;
	private bool nameFixed;
	
	private void Start(){
		if(!PhotonView.Get(gameObject).isMine){
			RequestBonusUpdate();
		}
		
		GameObject itemName = (GameObject)GameObject.Instantiate (GameManager.GamePrefabs.itemName, transform.position, Quaternion.identity);
		itemName.GetComponentInChildren<UILabel> ().text = "[" + item.stack.ToString () + "] " + item.itemName;
			
		Vector3 labelPos=UnityTools.RandomPointInArea(transform.position,0);
		itemName.transform.position = labelPos;
		itemName.transform.parent = transform;
	}
	
	public void OnMouseUp ()
	{
		if (Vector3.Distance (transform.position, GameManager.Player.transform.position) < pickUpDistance) {
			if (GameManager.Player.Inventory.AddItem (item)) {
				if(lookAtItem){
					GameManager.Player.transform.LookAt(new Vector3(transform.position.x,GameManager.Player.transform.position.y,transform.position.z));
				}
				
				if(playPickUpAnimation){
					GameManager.Player.Movement.PlayAnimation(GameManager.Player.Character.pickUp.name,GameManager.Player.Character.pickUp.length);
				}
				
				if(PhotonNetwork.offlineMode || PhotonView.Get(gameObject).isMine){
					DestroyItem();
				}else{
					PhotonView.Get(gameObject).RPC("DestroyItem",PhotonView.Get(gameObject).owner);
				}
				InterfaceContainer.Instance.worldToolTipWindow.SetActive (false);
				MessageManager.Instance.AddMessage (GameManager.GameMessages.pickUpItem.Replace ("@ItemName", item.itemName));
			}
		} else {
			if (GameManager.Player.Movement.controllerType != ThirdPersonMovement.ControllerType.ClickToMove) {
				MessageManager.Instance.AddMessage (GameManager.GameMessages.farAway);
			}
		}
	}
	
	private void OnMouseEnter ()
	{
		SlotContainer.disableMouseTalent = true;
		
		if (item is EquipmentItem && (item as EquipmentItem).bonus != null && (item as EquipmentItem).bonus.Count>0) {
			InterfaceContainer.Instance.worldToolTipWindow.SetActive (true);
			
			Vector3 v3 = PlayerCamera.Instance.GetComponent<Camera>().WorldToViewportPoint (transform.position); 
			v3 = UICamera.mainCamera.ViewportToWorldPoint (v3); 
			InterfaceContainer.Instance.worldToolTipWindow.transform.position = new Vector3 (v3.x - 0.3f, v3.y + 0.2f, 0);
			
			InterfaceContainer.Instance.worldToolTipTable.Reposition ();
			foreach (ToolTip toolTip in InterfaceContainer.Instance.worldToolTipTable.GetComponentsInChildren<ToolTip>()) {
				Destroy (toolTip.gameObject);
			}
			
			InterfaceContainer.Instance.worldToolTipItemName.text = item.itemName;
			
			EquipmentItem equipment = item as EquipmentItem;
			foreach (Bonus b in equipment.bonus) {
				GameObject bonus = NGUITools.AddChild (InterfaceContainer.Instance.worldToolTipTable.gameObject, GameManager.GamePrefabs.bonus);
				ToolTip toolTip = bonus.GetComponent<ToolTip> ();
				toolTip.bonusName.text = b.attribute;
				toolTip.bonusValue.text = "+" + b.bonusValue.ToString ();
				toolTip.bonusName.color = b.color;
				toolTip.bonusValue.color = b.color;
			}
			InterfaceContainer.Instance.worldToolTipTable.Reposition ();
		}
	}
	
	private void OnMouseExit ()
	{
		SlotContainer.disableMouseTalent = false;
		InterfaceContainer.Instance.worldToolTipWindow.SetActive (false);
	}
	
	private void OnDestroy ()
	{
		SlotContainer.disableMouseTalent = false;
	}
	
	//[RPC]
	public void DestroyItem(){
		PhotonNetwork.Destroy(gameObject);
	}
	
	private void RequestBonusUpdate ()
	{
		PhotonView.Get(gameObject).RPC("SendBonusUpdate",PhotonView.Get(gameObject).owner);	
	}
				
	//[RPC]
	private void SendBonusUpdate ()
	{		
		if (item is BonusItem) {
			BonusItem bonusItem= item as BonusItem;
			foreach (Bonus bonus in bonusItem.bonus) {
				PhotonView.Get (gameObject).RPC ("ReceiveBonus", PhotonTargets.Others, bonus.attribute, bonus.bonusValue);
			}
		}
	}
	
	//[RPC]
	private void ReceiveBonus (string attribute, int bonusValue)
	{
		if (item is BonusItem) {
			Bonus bonus = (item as BonusItem).GetBonus (attribute);
			if (bonus != null) {
				bonus.bonusValue = bonusValue;
			}
			
		}
	}
}
