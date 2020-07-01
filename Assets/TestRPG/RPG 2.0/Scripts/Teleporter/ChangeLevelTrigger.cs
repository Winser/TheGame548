using UnityEngine;
using System.Collections;

public class ChangeLevelTrigger : MonoBehaviour {
	public string level;
	
	private void OnTriggerEnter(Collider other){
		if(other.tag.Equals("Player")){
			if(PlayerCamera.Instance.isInFirstPersonView){
				PlayerCamera.Instance.SwitchCamera();
			}
			if(!PhotonNetwork.offlineMode){
				//PhotonView.Get(other).RPC("LoadLevel",PhotonTargets.MasterClient,level);
				PhotonNetworkPlayer[] players= (PhotonNetworkPlayer[])FindObjectsOfType(typeof(PhotonNetworkPlayer));
				for(int i=0; i< players.Length; i++){
					PhotonView.Get(players[i]).RPC("LoadLevel",PhotonView.Get(players[i]).owner,level);
				}
				
			}else{
				Application.LoadLevel(level);
			}
		}
	}
}
