using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingDetail : MonoBehaviour {
	public UISprite icon;
	public UILabel itemName;
	public List<RequireItemSlot> requiredIcons;
	public static string craftingAnimation;
	private BaseItem itemToCraft;
	public BaseItem ItemToCraft{
		get{return itemToCraft;}
		set{itemToCraft=value;
			if(itemToCraft){
				DisableRequiredItems();
				itemName.text=itemToCraft.itemName;
				icon.spriteName=itemToCraft.icon;
				SetupRequiredItems();
			}
		}
	}
	
	private void OnCraft(){
		if(ItemToCraft.neededCraftingItems!= null){
			bool hasResources=true;
			foreach(BaseItem item in ItemToCraft.neededCraftingItems.items){
				if(GameManager.Player.Inventory.GetItem(item.itemName) == null){
					hasResources=false;
					MessageManager.Instance.AddMessage(GameManager.GameMessages.needsItemInInventory.Replace("@ItemName",item.itemName));
				}
			}
			if(!hasResources){
				return;
			}
			
			foreach(BaseItem item in ItemToCraft.neededCraftingItems.items){
				BaseItem invItem=GameManager.Player.Inventory.GetItem(item.itemName);
				invItem.stack-=1;
				ItemSlot slot=GameManager.Player.Inventory.GetItemSlot(invItem);
				slot.stackLabel.text=invItem.stack.ToString();
				
				if(invItem.stack<=0){
					GameManager.Player.Inventory.RemoveItem(invItem);
				}
			}
		}
		
		ProgressBar progress = InterfaceContainer.Instance.progressBar;
		if (progress.isDone) {	
			gameObject.SetActive(false);
			progress.StartProgress (3,OnCraftFinish);
			if(craftingAnimation != string.Empty){
				GameManager.Player.Movement.PlayAnimation(craftingAnimation,3);
			}
		}
	}
	
	private void OnCraftFinish(){
		gameObject.SetActive(true);
		BaseItem item= (BaseItem)ScriptableObject.Instantiate(ItemToCraft);
		if(item is BonusItem){
			(item as BonusItem).RandomizeBonus();
		}
		GameManager.Player.Inventory.AddItem(item);
		GameManager.Player.Movement.PlayAnimation(GameManager.Player.Character.pickUp.name,0);
	}
	
	private void SetupRequiredItems(){
		if(ItemToCraft.neededCraftingItems == null){
			return;
		}
		for(int i=0; i< ItemToCraft.neededCraftingItems.items.Count;i++){
			requiredIcons[i].gameObject.SetActive(true);
			requiredIcons[i].icon.spriteName=ItemToCraft.neededCraftingItems.items[i].icon;
		}
	}
	
	public void DisableRequiredItems(){
		foreach(RequireItemSlot slot in requiredIcons){
			slot.gameObject.SetActive(false);
		}
	}
}
