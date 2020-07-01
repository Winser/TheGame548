using UnityEngine;
using System.Collections;

public class CraftingSlot : MonoBehaviour {
	[HideInInspector]
	public BaseItem item;
	public UILabel itemName;
	
	private void Update(){
		if(item != null){
			itemName.text=item.itemName;
		}
	}
	
	private void OnClick(){
		InterfaceContainer.Instance.craftingMenu.SetActive(false);
		InterfaceContainer.Instance.craftingDetailsWindow.SetActive(true);
		InterfaceContainer.Instance.craftingDetailsWindow.GetComponent<CraftingDetail>().ItemToCraft=item;
	}
}
