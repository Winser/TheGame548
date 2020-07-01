using UnityEngine;
using System.Collections;

public class ContainerItemSlot : MonoBehaviour {
	[HideInInspector]
	public BaseItem item;
	public UISprite sprite;
	public UILabel stack;
	
	private void OnClick(){
		if(sprite.gameObject.activeSelf && GameManager.Player.Inventory.AddItem(item)){
			sprite.gameObject.SetActive(false);
			stack.text="";
			ItemContainer.lastContainer.items.Remove(item);
		}
	}
	
	private void Update(){
		if(!sprite.gameObject.activeSelf && InterfaceContainer.Instance.isDragging && Input.GetMouseButtonUp (0) && InterfaceContainer.Instance.draggingSlot != null){
			Ray ray = UICamera.currentCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.Equals (GetComponent<Collider>())) {
					BaseItem draggingItem= GameManager.Player.Inventory.GetItem((ItemSlot)InterfaceContainer.Instance.draggingSlot);
					
					if(draggingItem == null){
						return;
					}
					if ( GameManager.Player.Inventory.RemoveItem(draggingItem)){
						item= draggingItem;
						sprite.gameObject.SetActive(true);
						sprite.spriteName=item.icon;
						stack.text=item.stack.ToString();
						InterfaceContainer.Instance.isDragging=false;
						ItemContainer.lastContainer.items.Add(item);
					}
				}
			}
		}
	}
	
	void OnHover (bool isOver)
	{
		if(isOver && sprite.gameObject.activeSelf){
			InterfaceContainer.Instance.ShowToolTip(item,new Vector2(-0.4f,-0.2f));
		}else{
			InterfaceContainer.Instance.toolTipWindow.SetActive(false);
		}
	}
}
