using UnityEngine;
using System.Collections;

public class SellSlot : MonoBehaviour {
	public UISprite itemIcon;
	public UILabel itemName;
	public UILabel itemPrice;
	[HideInInspector]
	public SellableItem item;
	
	private float lastTapTime;
	private float tapSpeed=0.5f;
	
	private void OnClick(){
		if ((Time.time - lastTapTime) < tapSpeed) {
			if(item != null){
				ShopManager.Instance.AddToPurchase(item);
			}
		}
		lastTapTime = Time.time;
	}
	
	void OnHover (bool isOver)
	{
		if(isOver){
			InterfaceContainer.Instance.ShowToolTip(item,new Vector2(-0.4f,-0.2f));
		}else{
			InterfaceContainer.Instance.toolTipWindow.SetActive(false);
		}
	}
	
	private void OnDisable(){
		InterfaceContainer.Instance.toolTipWindow.SetActive(false);
	}
}
