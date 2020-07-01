using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {
	public ItemTable itemTable;
	
	public void OnMouseUp(){
		if(InterfaceContainer.Instance.shopWindow.activeSelf){
			return;
		}
		if(Vector3.Distance(GameManager.Player.transform.position,transform.position)<4){
			InterfaceContainer.Instance.shopWindow.SetActive(true);
			ShopManager.Instance.Reset();
			ShopManager.Instance.ClearShopList();
			ShopManager.Instance.FillShopList(itemTable);
		}else{
			if(GameManager.Player.Movement.controllerType != ThirdPersonMovement.ControllerType.ClickToMove){
				MessageManager.Instance.AddMessage(GameManager.GameMessages.farAway);
			}
		}
	}
}
