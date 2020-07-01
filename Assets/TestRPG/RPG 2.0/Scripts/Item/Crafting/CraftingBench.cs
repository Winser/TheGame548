using UnityEngine;
using System.Collections;

public class CraftingBench : MonoBehaviour
{
	public ItemTable craftingItems;
	public EquipmentItem tool;
	public float maxDistance = 2.0f;
	private Vector3 lastPlayerPosition;
	public AnimationClip craftingAnimation;
	
	private void OnMouseUp ()
	{
		if (Vector3.Distance (GameManager.Player.transform.position, transform.position) > maxDistance) {
			MessageManager.Instance.AddMessage (GameManager.GameMessages.farAway);
			return;
		}
		
		if (tool != null) {
			EquipmentItem equipedTool = GameManager.Player.Inventory.GetEquipmentItem (EquipmentItem.Region.Hands);
			if (equipedTool == null || equipedTool.itemName != tool.itemName) {
				MessageManager.Instance.AddMessage (GameManager.GameMessages.needTool.Replace ("@ItemName", tool.itemName));
				return;
			}
		}
		
		InterfaceContainer.Instance.craftingMenu.SetActive (true);
		ClearBench();
		FillBench(craftingItems);
		InterfaceContainer.Instance.craftingDetailsWindow.SetActive(false);
		if(craftingAnimation != null){
			CraftingDetail.craftingAnimation=craftingAnimation.name;
		}
		lastPlayerPosition = GameManager.Player.transform.position;
	}
	
	private void Update(){
		if(GameManager.Player != null){
			float delta = (GameManager.Player.transform.position - lastPlayerPosition).sqrMagnitude;
			if (delta > 0.1f) {
				InterfaceContainer.Instance.craftingMenu.SetActive (false);
				InterfaceContainer.Instance.craftingDetailsWindow.SetActive (false);
			}
		}
	}
	
	private void FillBench (ItemTable itemTable)
	{
		foreach (BaseItem item in itemTable.items) {
			BaseItem mItem = (BaseItem)Instantiate (item);
			if (mItem is BonusItem) {
				(mItem as BonusItem).RandomizeBonus ();
			}
				
			GameObject go = NGUITools.AddChild (InterfaceContainer.Instance.craftingTable.gameObject, GameManager.GamePrefabs.craftingItem);
			CraftingSlot slot = go.GetComponent<CraftingSlot> ();
			slot.item = mItem;
		}
		InterfaceContainer.Instance.craftingTable.Reposition ();
	}
	
	public void ClearBench ()
	{
		foreach (CraftingSlot slot in InterfaceContainer.Instance.craftingTable.GetComponentsInChildren<CraftingSlot>()) {
			Destroy (slot.gameObject);
		}
	} 
	
	private void OnMouseExit ()
	{
		SlotContainer.disableMouseTalent = false;
	}
	
	private void OnMouseEnter ()
	{
		SlotContainer.disableMouseTalent = true;
	}
}
