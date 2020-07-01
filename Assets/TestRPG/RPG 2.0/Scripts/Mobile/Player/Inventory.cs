using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Storage class for inventory and equipment
/// </summary>
public class Inventory
{
	/// <summary>
	/// The inventory slots and items.
	/// </summary>
	private Dictionary<ItemSlot, BaseItem> items;
	
	/// <summary>
	/// The equipment slots and items
	/// </summary>
	private Dictionary<EquipmentSlot, EquipmentItem> equipment;
	/// <summary>
	/// Reference to the PlayerEquipment script that is attached to the player
	/// </summary>
	private PlayerEquipment playerEquipment;
	
	private List<EquipmentItem> startEquipment;
	
	public PlayerEquipment PlayerEquipment{
		get{
			if(!playerEquipment){
				playerEquipment=GameManager.Player.transform.GetComponent<PlayerEquipment>();
			}
			return playerEquipment;
		}
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="Inventory"/> class.
	/// </summary>
	/// <param name='eq'>
	/// PlayerEquipment
	/// </param>
	public Inventory (PlayerEquipment eq)
	{
		items = new Dictionary<ItemSlot, BaseItem> ();
		equipment = new Dictionary<EquipmentSlot, EquipmentItem> ();
		startEquipment= new List<EquipmentItem>();
		defaultStartAttack=GameManager.PlayerSettings.mouseTalent;
		
		playerEquipment= eq;
		
		InterfaceContainer.Instance.profileWindow.SetActive (true);
		foreach (ItemSlot slot in InterfaceContainer.Instance.inventoryGrid.GetComponentsInChildren<ItemSlot>()) {
			items.Add (slot, null);
		}
		
		foreach (EquipmentSlot slot in InterfaceContainer.Instance.equipmentTable.GetComponentsInChildren<EquipmentSlot>()) {
			equipment.Add (slot, null);
		}	
		
		InterfaceContainer.Instance.profileWindow.SetActive (false);
	}
	
	public Inventory(){
		items = new Dictionary<ItemSlot, BaseItem> ();
		equipment = new Dictionary<EquipmentSlot, EquipmentItem> ();
		startEquipment= new List<EquipmentItem>();
		defaultStartAttack=GameManager.PlayerSettings.mouseTalent;
		
		InterfaceContainer.Instance.profileWindow.SetActive (true);
		foreach (ItemSlot slot in InterfaceContainer.Instance.inventoryGrid.GetComponentsInChildren<ItemSlot>()) {
			items.Add (slot, null);
		}
		
		foreach (EquipmentSlot slot in InterfaceContainer.Instance.equipmentTable.GetComponentsInChildren<EquipmentSlot>()) {
			equipment.Add (slot, null);
		}	
		
		InterfaceContainer.Instance.profileWindow.SetActive (false);
	}
	
	/// <summary>
	/// Gets the item slot.
	/// </summary>
	/// <returns>
	/// The item slot.
	/// </returns>
	/// <param name='id'>
	/// Identifier.
	/// </param>
	public ItemSlot GetItemSlot(int id){
		foreach(KeyValuePair<ItemSlot, BaseItem> kvp in items){
			if(kvp.Key.id.Equals(id)){
				return kvp.Key;
			}
		}
		return null;
	}
	
	public List<BaseItem> GetItems(){
		return new List<BaseItem>(items.Values);
	}
	
	public List<EquipmentItem> GetEquipmentItems(){
		return new List<EquipmentItem>(equipment.Values);
	}
	
	/// <summary>
	/// Adds the item to the inventory
	/// </summary>
	/// <param name='item'>
	/// Item.
	/// </param>
	/// <param name='slot'>
	/// Slot.
	/// </param>
	public void AddItem (BaseItem item, ItemSlot slot)
	{
		slot.icon.gameObject.SetActive (true);
		slot.icon.spriteName = item.icon;
		slot.stackLabel.text = item.stack.ToString ();
		items [slot] = item;
		item.itemSlotId=slot.id;
	}
	
	/// <summary>
	/// Adds the item to the inventory
	/// </summary>
	/// <returns>
	/// True if item could be added.
	/// </returns>
	/// <param name='item'>
	/// Item
	/// </param>
	public bool AddItem (BaseItem item)
	{
		BaseItem mItem = GetItem (item.itemName);
		if (mItem != null && mItem.stackable) {
			mItem.stack += item.stack;
			ItemSlot slot = GetItemSlot (item);
			slot.stackLabel.text = mItem.stack.ToString ();
		} else {
			ItemSlot slot = GetFreeSlot ();
			if (slot == null) {
				MessageManager.Instance.AddMessage(GameManager.GameMessages.fullInventory);
				return false;
			}
			items [slot] = item;
			item.itemSlotId=slot.id;
			
			slot.icon.gameObject.SetActive (true);
			slot.icon.spriteName = item.icon;
			slot.stackLabel.text = item.stack.ToString ();
			
		}
		return true;
	}
	
	/// <summary>
	/// Gets the item by name
	/// </summary>
	/// <returns>
	/// The item.
	/// </returns>
	/// <param name='itemName'>
	/// Item name.
	/// </param>
	public BaseItem GetItem (string itemName)
	{
		foreach (KeyValuePair<ItemSlot,BaseItem> kvp in items) {
			if (kvp.Value && kvp.Value.itemName.Equals (itemName)) {
				return kvp.Value;
			}
		}
		return null;
	}
	
	/// <summary>
	/// Gets the item by ItemSlot
	/// </summary>
	/// <returns>
	/// The item.
	/// </returns>
	/// <param name='slot'>
	/// Slot.
	/// </param>
	public BaseItem GetItem (ItemSlot slot)
	{
		foreach (KeyValuePair<ItemSlot,BaseItem> kvp in items) {
			if (kvp.Value && kvp.Key.Equals (slot)) {
				return kvp.Value;
			}
		}
		return null;
	}
	
	/// <summary>
	/// Gets the item slot.
	/// </summary>
	/// <returns>
	/// The item slot.
	/// </returns>
	/// <param name='item'>
	/// Item.
	/// </param>
	public ItemSlot GetItemSlot (BaseItem item)
	{
		foreach (KeyValuePair<ItemSlot,BaseItem> kvp in items) {
			if (kvp.Value && kvp.Value.itemName.Equals (item.itemName)) {
				return kvp.Key;
			}
		}
		return null;
	}
	
	/// <summary>
	/// Gets the free inventory slot.
	/// </summary>
	/// <returns>
	/// The free slot.
	/// </returns>
	public ItemSlot GetFreeSlot ()
	{
		foreach (KeyValuePair<ItemSlot, BaseItem> kvp in items) {
			if (kvp.Value == null) {
				return kvp.Key;
			}
		}
		return null;
	}
	
	/// <summary>
	/// Removes the item from the inventory
	/// </summary>
	/// <returns>
	/// The item.
	/// </returns>
	/// <param name='item'>
	/// Item.
	/// </param>
	public bool RemoveItem (BaseItem item)
	{
		ItemSlot slot = null;
		foreach (KeyValuePair<ItemSlot, BaseItem> kvp in items) {
			if (kvp.Value != null && kvp.Value.Equals (item)) {
				slot = kvp.Key;
				break;
			}
		}
		
		if (slot != null) {
			items [slot] = null;
			slot.icon.gameObject.SetActive (false);
			slot.stackLabel.text = "";
			return true;
		}
		return false;
	}
	
	/// <summary>
	/// Drops the item to the ground
	/// </summary>
	/// <param name='item'>
	/// Item.
	/// </param>
	/// <param name='pos'>
	/// Position to drop the item.
	/// </param>
	public void DropItem (BaseItem item, Vector3 pos)
	{
		if (!item) {
			return;
		}
		GameObject go = (GameObject)PhotonNetwork.Instantiate (item.prefab.name, pos, UnityTools.RandomQuaternion(Vector3.up,0,360),0);
		Lootable loot = go.GetComponent<Lootable> ();
		if (loot != null) {
			loot.item = item;
		}
		GameManager.Player.PlaySound(item.dropSound);
		RemoveItem (item);
		InterfaceContainer.Instance.isDragging=false;
	}
	
	/// <summary>
	/// Drops the item by slot
	/// </summary>
	/// <param name='slot'>
	/// Slot.
	/// </param>
	/// <param name='pos'>
	/// Position to drop the item.
	/// </param>
	public void DropItem (ItemSlot slot, Vector3 pos)
	{
		BaseItem item = GetItem (slot);
		if (!item) {
			return;
		}
		GameObject go = (GameObject)PhotonNetwork.Instantiate(item.prefab.name, pos, UnityTools.RandomQuaternion(Vector3.up,0,360),0);
		Lootable loot = go.GetComponent<Lootable> ();
		if (loot != null) {
			loot.item = item;
		}
		GameManager.Player.PlaySound(item.dropSound);
		RemoveItem (item);
		InterfaceContainer.Instance.isDragging=false;
	}
	
	private DamageTalent defaultStartAttack;
	
	/// <summary>
	/// Equip the specified item.
	/// </summary>
	/// <param name='item'>
	/// Item.
	/// </param>
	public void Equip (EquipmentItem item)
	{
		EquipmentSlot slot = GetEquipmentSlot (item.equipmentRegion);
		
		if (equipment [slot]) {
			UnEquip (item.equipmentRegion);
		}
		equipment [slot] = item;
		slot.icon.gameObject.SetActive (true);
		slot.icon.spriteName = item.icon;
		if(!PhotonNetwork.offlineMode){
			PhotonView.Get(GameManager.Player.transform).RPC("SetActiveEquipment", PhotonTargets.All,item.itemName,true);
		}else{
			PlayerEquipment.SetActiveEquipment (item.itemName, true);
		}
		
		if(item.bonus != null){
			foreach (Bonus bonus in item.bonus) {
				PlayerAttribute attr = GameManager.Player.GetAttribute (bonus.attribute);
				if (attr != null) {
					attr.TempValue += bonus.bonusValue;
				}
			}
		}
		
		if(item.overrideMouseTalent != null){
			switch(item.equipmentRegion){
			case EquipmentItem.Region.Hands:
				item.overrideMouseTalent.spentPoints+=1;
				GameManager.PlayerSettings.mouseTalent=item.overrideMouseTalent;
				break;
			}
		}
		
		if(item.overrideIdle != null){
			GameManager.Player.Character.idle=item.overrideIdle;
		}
		
		InterfaceContainer.Instance.isDragging=false;
		RemoveItem(item);
		
	}
		
	/// <summary>
	/// Unequip the item by region
	/// </summary>
	/// <param name='region'>
	/// Region.
	/// </param>
	public void UnEquip (EquipmentItem.Region region)
	{
		EquipmentSlot slot = GetEquipmentSlot (region);
		if (equipment [slot]) {
			AddItem (equipment [slot]);
			slot.icon.gameObject.SetActive (false);
			if(!PhotonNetwork.offlineMode){
				PhotonView.Get(GameManager.Player.transform).RPC("SetActiveEquipment", PhotonTargets.All,equipment [slot].itemName,false);
			}else{
				PlayerEquipment.SetActiveEquipment (equipment [slot].itemName, false);
			}
			if(equipment[slot].bonus != null){
				foreach (Bonus bonus in equipment [slot].bonus) {
					PlayerAttribute attr = GameManager.Player.GetAttribute (bonus.attribute);
					if (attr != null) {
						attr.TempValue -= bonus.bonusValue;
					}
				}
			}
			if(defaultStartAttack != GameManager.PlayerSettings.mouseTalent){
				switch(region){
				case EquipmentItem.Region.Hands:
					equipment [slot].overrideMouseTalent.spentPoints-=1;
					GameManager.PlayerSettings.mouseTalent=defaultStartAttack;
					break;
				}
			}
			
			if(equipment[slot].overrideIdle != null && equipment[slot].overrideIdle != GameManager.CharacterDatabase.GetCharacter(GameManager.Player.Character.characterClass).idle){
				GameManager.Player.Character.idle=GameManager.CharacterDatabase.GetCharacter(GameManager.Player.Character.characterClass).idle;
			}
			GameManager.Player.PlaySound(equipment[slot].dropSound);
			equipment [slot] = null;
			InterfaceContainer.Instance.isDragging=false;
			
		}
	}
	
	/// <summary>
	/// Unequip the item by region to the ItemSlot
	/// </summary>
	/// <param name='region'>
	/// Region.
	/// </param>
	/// <param name='itemSlot'>
	/// Item slot.
	/// </param>
	public void UnEquip (EquipmentItem.Region region, ItemSlot itemSlot)
	{
		EquipmentSlot slot = GetEquipmentSlot (region);
		if (equipment [slot] && !GetItem (itemSlot)) {
			AddItem (equipment [slot], itemSlot);
			slot.icon.gameObject.SetActive (false);
			if(!PhotonNetwork.offlineMode){
				PhotonView.Get(GameManager.Player.transform).RPC("SetActiveEquipment", PhotonTargets.All,equipment [slot].itemName,false);
			}else{
				PlayerEquipment.SetActiveEquipment (equipment [slot].itemName, false);
			}
			
			if(equipment[slot].bonus != null){
				foreach (Bonus bonus in equipment [slot].bonus) {
					PlayerAttribute attr = GameManager.Player.GetAttribute (bonus.attribute);
					if (attr != null) {
						attr.TempValue -= bonus.bonusValue;
					}
				}
			}
			if(defaultStartAttack != GameManager.PlayerSettings.mouseTalent){
				switch(region){
				case EquipmentItem.Region.Hands:
					equipment [slot].overrideMouseTalent.spentPoints-=1;
					GameManager.PlayerSettings.mouseTalent=defaultStartAttack;
					break;
				}
			}
			
			if(equipment[slot].overrideIdle != null && equipment[slot].overrideIdle != GameManager.CharacterDatabase.GetCharacter(GameManager.Player.Character.characterClass).idle){
				GameManager.Player.Character.idle=GameManager.CharacterDatabase.GetCharacter(GameManager.Player.Character.characterClass).idle;
			}
			GameManager.Player.PlaySound(equipment[slot].dropSound);
			equipment [slot] = null;
			InterfaceContainer.Instance.isDragging=false;
			
		}
	}
	
	/// <summary>
	/// Gets the equipment slot by region
	/// </summary>
	/// <returns>
	/// The equipment slot.
	/// </returns>
	/// <param name='region'>
	/// Region.
	/// </param>
	public EquipmentSlot GetEquipmentSlot (EquipmentItem.Region region)
	{
		foreach (KeyValuePair<EquipmentSlot, EquipmentItem> kvp in equipment) {
			if (kvp.Key.region.Equals (region)) {
				return kvp.Key;
			}
		}
		return null;
	}
	
	/// <summary>
	/// Gets the equipment item by region
	/// </summary>
	/// <returns>
	/// The equipment item.
	/// </returns>
	/// <param name='region'>
	/// Region.
	/// </param>
	public EquipmentItem GetEquipmentItem (EquipmentItem.Region region)
	{
		return equipment [GetEquipmentSlot (region)];
	}
	
	public void InitializeStartEquipment(){
		foreach(EquipmentItem item in startEquipment){
			Equip(item);
		}
	}
	
	public void AddStartEquipmentItem(EquipmentItem item){
		startEquipment.Add(item);
	}
}
