using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
	private static ShopManager instance;

	public static ShopManager Instance {
		get{ return instance;}
	}
	
	private List<SellableItem> purchaseList;
	private List<SellableItem> sellList;
	private int totalCost;
	private int totalPayout;
	
	public int TotalCost {
		get{ return totalCost;}
		set {
			totalCost = value;
			InterfaceContainer.Instance.totalCost.text = totalCost.ToString ();
		}
	}
	
	public int TotalPayout{
		get{return totalPayout;}
		set{totalPayout=value;
		InterfaceContainer.Instance.totalPayout.text=totalPayout.ToString();
		}
	}
	
	private void Awake ()
	{
		instance = this;
	}
	
	private void Start ()
	{
		purchaseList = new List<SellableItem> ();
		sellList = new List<SellableItem> ();
	}
	
	public void Purchase ()
	{
		foreach (SellableItem item in purchaseList) {
			if (GameManager.Player.Gold>= item.buyPrice && GameManager.Player.Inventory.AddItem ((SellableItem)Instantiate(item))) {
					GameManager.Player.Gold-=item.buyPrice;
			}
		}
		ClearPurchaseList();
	}
	
	public void AddToPurchase (SellableItem item)
	{
		UISprite icon = GetFreePurchaseSlot ();
		if (icon != null) {
			purchaseList.Add (item);
			icon.gameObject.SetActive (true);
			icon.spriteName = item.icon;
			TotalCost += item.buyPrice;
		}
	}
	
	private UISprite GetFreePurchaseSlot ()
	{
		foreach (SellSlot slot in InterfaceContainer.Instance.purchaseTable.GetComponentsInChildren<SellSlot>()) {
			if (!slot.itemIcon.gameObject.activeSelf) {
				return slot.itemIcon;
			}
		}
		return null;
	}
	
	public void ClearPurchaseList ()
	{
		foreach (SellSlot slot in InterfaceContainer.Instance.purchaseTable.GetComponentsInChildren<SellSlot>()) {
			slot.itemIcon.gameObject.SetActive (false);
		}
		TotalCost = 0;
		purchaseList.Clear ();
		
	}
	
	public void Sell(){
		GameManager.Player.Gold+=TotalPayout;
		foreach(SellSlot slot in InterfaceContainer.Instance.sellingTable.GetComponentsInChildren<SellSlot>()){
			slot.itemIcon.gameObject.SetActive(false);
		}
		TotalPayout=0;
		sellList.Clear();
	}
	
	public void AddToSell(SellableItem item){
		UISprite icon = GetFreeSellSlot ();
		if (icon != null) {
			sellList.Add (item);
			icon.gameObject.SetActive (true);
			icon.spriteName = item.icon;
			TotalPayout += item.sellPrice*item.stack;
		}
	}
	
	private UISprite GetFreeSellSlot(){
		foreach (SellSlot slot in InterfaceContainer.Instance.sellingTable.GetComponentsInChildren<SellSlot>()) {
			if (!slot.itemIcon.gameObject.activeSelf) {
				return slot.itemIcon;
			}
		}
		return null;
	}
	
	public void ClearSellList ()
	{
		foreach(SellSlot slot in InterfaceContainer.Instance.sellingTable.GetComponentsInChildren<SellSlot>()){
			slot.itemIcon.gameObject.SetActive(false);
		}
		TotalPayout=0;
		foreach(SellableItem item in sellList){
			GameManager.Player.Inventory.AddItem(item);
		}
		sellList.Clear();
	}
	
	public void Reset(){
		ClearPurchaseList();
		ClearSellList();
	}
	
	public void FillShopList(ItemTable itemTable){
		foreach(BaseItem item in itemTable.items){
			if(item is SellableItem){
				SellableItem sellableItem=(SellableItem)Instantiate(item);
				if(sellableItem is BonusItem){
					(sellableItem as BonusItem).RandomizeBonus();
				}
				
				GameObject sellItem= NGUITools.AddChild(InterfaceContainer.Instance.shopTable.gameObject,GameManager.GamePrefabs.sellItem);
				SellSlot slot= sellItem.GetComponent<SellSlot>();
				slot.itemIcon.spriteName=sellableItem.icon;
				slot.itemName.text=sellableItem.itemName;
				slot.itemPrice.text=sellableItem.buyPrice.ToString();
				slot.item=sellableItem;
			}
		}
		InterfaceContainer.Instance.shopTable.Reposition();
	}
	
	public void ClearShopList(){
		foreach(SellSlot slot in InterfaceContainer.Instance.shopTable.GetComponentsInChildren<SellSlot>()){
			Destroy(slot.gameObject);
		}
	}
}
