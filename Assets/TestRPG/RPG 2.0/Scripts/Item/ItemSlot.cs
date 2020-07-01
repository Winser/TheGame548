using UnityEngine;
using System.Collections;

public class ItemSlot : Slot
{
	public int id;
	public UILabel stackLabel;
	
	public override bool Use ()
	{
		BaseItem item = GameManager.Player.Inventory.GetItem (this);
		if(item && item is UsableItem){
			coolDown= (item as UsableItem).coolDown;
			if((item as UsableItem).Use()){
				if(item.stackable && item.stack>0){
					stackLabel.text=item.stack.ToString();
				}else{
					stackLabel.text="";
					removeIcon=true;
				}
				return true;
			}
		}
		return false;
	}
	
	public override void Update ()
	{
		base.Update ();
		if (Input.GetMouseButtonUp (0) && InterfaceContainer.Instance.isDragging) {
			Ray ray = UICamera.currentCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.Equals (GetComponent<Collider>())) {
					BaseItem thisSlotItem = GameManager.Player.Inventory.GetItem (this);
					
					if (InterfaceContainer.Instance.draggingSlot is ItemSlot) {
						ItemSlot slot = InterfaceContainer.Instance.draggingSlot as ItemSlot;
						BaseItem draggingItem = GameManager.Player.Inventory.GetItem (slot);
						
						if (!thisSlotItem || (thisSlotItem.stackable && draggingItem.itemName.Equals (thisSlotItem.itemName))) {
							if (GameManager.Player.Inventory.RemoveItem (draggingItem)) {
								GameManager.Player.Inventory.AddItem (draggingItem, this);
								InterfaceContainer.Instance.isDragging = false;
							}
						}
					}
					if (InterfaceContainer.Instance.draggingSlot is EquipmentSlot) {
						EquipmentSlot equipmentSlot = InterfaceContainer.Instance.draggingSlot as EquipmentSlot;
						if (!thisSlotItem) {
							GameManager.Player.Inventory.UnEquip (equipmentSlot.region, this);
							InterfaceContainer.Instance.isDragging = false;
						}
					}
				}
			}
		}
	}
	
	private float lastTapTime;
	private float tapSpeed = 0.3f;
	
	private void OnClick (){
		BaseItem item= GameManager.Player.Inventory.GetItem(this);
		
		if ((Time.time - lastTapTime) < tapSpeed && item) {
			if(InterfaceContainer.Instance.shopWindow.activeSelf && item is SellableItem && GameManager.Player.Inventory.RemoveItem(item)){
				ShopManager.Instance.AddToSell((item as SellableItem));
			}else{
				Use();
			}
		}
		lastTapTime = Time.time;
	}
	
	void OnHover (bool isOver)
	{
		BaseItem item = GameManager.Player.Inventory.GetItem (this);
	
		if (isOver && item != null && item is EquipmentItem && (item as EquipmentItem).bonus != null && (item as EquipmentItem).bonus.Count>0) {

			InterfaceContainer.Instance.inventoryToolTipWindow.SetActive (true);
			InterfaceContainer.Instance.inventoryToolTipTable.Reposition ();
			foreach (ToolTip toolTip in InterfaceContainer.Instance.inventoryToolTipTable.GetComponentsInChildren<ToolTip>()) {
				Destroy (toolTip.gameObject);
			}
			
			InterfaceContainer.Instance.inventoryToolTipItemName.text = item.itemName;
			
			EquipmentItem equipment = item as EquipmentItem;

			foreach (Bonus b in equipment.bonus) {
				GameObject bonus = NGUITools.AddChild (InterfaceContainer.Instance.inventoryToolTipTable.gameObject, GameManager.GamePrefabs.bonus);
				ToolTip toolTip = bonus.GetComponent<ToolTip> ();
				toolTip.bonusName.text = b.attribute;
				toolTip.bonusValue.text = "+" + b.bonusValue.ToString();
				toolTip.bonusName.color = b.color;
				toolTip.bonusValue.color = b.color;
			}
			InterfaceContainer.Instance.inventoryToolTipTable.Reposition ();
			
			EquipmentItem eq = GameManager.Player.Inventory.GetEquipmentItem (equipment.equipmentRegion);
			if (eq != null && eq.bonus != null && eq.bonus.Count>0) {
				InterfaceContainer.Instance.equipmentToolTipWindow.SetActive (true);
				InterfaceContainer.Instance.equipmentToolTipTable.Reposition ();
				foreach (ToolTip toolTip in InterfaceContainer.Instance.equipmentToolTipTable.GetComponentsInChildren<ToolTip>()) {
					Destroy (toolTip.gameObject);
				}
			
				InterfaceContainer.Instance.equipmentToolTipItemName.text = eq.itemName;
			
				foreach (Bonus b in eq.bonus) {
					GameObject bonus = NGUITools.AddChild (InterfaceContainer.Instance.equipmentToolTipTable.gameObject, GameManager.GamePrefabs.bonus);
					ToolTip toolTip = bonus.GetComponent<ToolTip> ();
					toolTip.bonusName.text = b.attribute;
					toolTip.bonusValue.text = "+" + b.bonusValue.ToString();
					toolTip.bonusName.color = b.color;
					toolTip.bonusValue.color = b.color;
				}
				InterfaceContainer.Instance.equipmentToolTipTable.Reposition ();
			}
			
		} else {
			InterfaceContainer.Instance.inventoryToolTipWindow.SetActive (false);
			InterfaceContainer.Instance.equipmentToolTipWindow.SetActive (false);
		}
	}
}
