using UnityEngine;
using System.Collections;

public class ChatManager : Photon.MonoBehaviour {
	private static ChatManager instance;
	public static ChatManager Instance{
		get{return instance;}
	}
	
	public UIInput chatInput;
	
	private void Awake(){
		instance=this;
	}
	
	private void OnSubmit(){
		if(!chatInput.text.Equals(string.Empty)){
			photonView.RPC("OnNetworkSubmit",PhotonTargets.All,chatInput.text,PhotonNetwork.player.name);
			chatInput.text="";
		}
	}
	
	private void Update(){
		if(Input.GetKeyUp(GameManager.InputSettings.chat)){
			chatInput.gameObject.SetActive(!chatInput.gameObject.activeSelf);
			if(chatInput.gameObject.activeSelf){
				chatInput.selected=true;
			}else{
				GameManager.Player.Movement.killInput=false;
			}
		}
		
		if(chatInput.gameObject.activeSelf){
			GameManager.Player.Movement.killInput=true;
		}
	}
	
	//[RPC]
	private void OnNetworkSubmit(string msg, string playerName){
		MessageManager.Instance.AddMessage("[0000CD]"+playerName+":[-] "+msg);
	}
}
