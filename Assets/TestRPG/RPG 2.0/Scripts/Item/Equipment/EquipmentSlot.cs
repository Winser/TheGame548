using UnityEngine;
using System.Collections;

public class EquipmentSlot : Slot {
	public EquipmentItem.Region region;
	
	public override void Update ()
	{
		base.Update ();
		if (Input.GetMouseButtonUp (0) && InterfaceContainer.Instance.isDragging) {
			Ray ray = UICamera.currentCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.Equals (GetComponent<Collider>())) {
					if(InterfaceContainer.Instance.draggingSlot is ItemSlot){
						BaseItem item= GameManager.Player.Inventory.GetItem((ItemSlot)InterfaceContainer.Instance.draggingSlot);
						if(item && (item is EquipmentItem) && ((EquipmentItem)item).equipmentRegion.Equals(region)){
							if(!((EquipmentItem)item).characterClasses.Contains( GameManager.Player.Character.characterClass)){
								MessageManager.Instance.AddMessage(GameManager.GameMessages.canNotEquip);
								InterfaceContainer.Instance.isDragging=false;
								return;
							}
							GameManager.Player.Inventory.Equip((EquipmentItem)item);
							GameManager.Player.Inventory.RemoveItem(item);
						}
					}
				}
			}
		}
	}
	
	private float lastTapTime;
	private float tapSpeed=0.3f;
	
	private void OnClick(){
		if ((Time.time - lastTapTime) < tapSpeed) {
			EquipmentItem item= GameManager.Player.Inventory.GetEquipmentItem(region);
			if(item){
				GameManager.Player.Inventory.UnEquip(item.equipmentRegion);
			}
		}
		lastTapTime = Time.time;
	}
	
	public override void OnPress (bool pressed)
	{
		if(pressed && GameManager.Player.Inventory.GetEquipmentItem(region)){
			base.OnPress (pressed);
		}else{
			InterfaceContainer.Instance.dragIcon.transform.position = new Vector3(transform.position.x,transform.position.y,InterfaceContainer.Instance.dragIcon.transform.position.z);
			InterfaceContainer.Instance.dragIcon.gameObject.SetActive (false);
			InterfaceContainer.Instance.isDragging=false;
		}
	}
}
