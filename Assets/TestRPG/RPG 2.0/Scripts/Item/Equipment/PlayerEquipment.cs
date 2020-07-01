using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEquipment : MonoBehaviour {
	public List<EquipmentMesh> equipment;
	
	private void Start(){
		if(!PhotonView.Get (gameObject).isMine){
			RequestActiveEquipment();
		}
	}
	
	private void RequestActiveEquipment(){
		PhotonView.Get(gameObject).RPC("UpdateEquipment",PhotonView.Get(gameObject).owner);
	}
	
	//[RPC]
	private void UpdateEquipment(){
		PhotonView view= PhotonView.Get(gameObject);
		
		foreach(EquipmentMesh e in equipment){	
			foreach(string itemName in e.itemNames){
				foreach(Transform tr in e.mesh){	
					view.RPC("SetActiveEquipment",PhotonTargets.Others,itemName,tr.gameObject.activeSelf);	
				}
			}
		}
	}
	
	//[RPC]
	public void SetActiveEquipment(string itemName, bool state){
		foreach(EquipmentMesh e in equipment){
			foreach(string n in e.itemNames){
				if(n.Trim().Equals(itemName.Trim())){
					foreach(Transform tr in e.mesh){
						tr.gameObject.SetActive(state);
					}
				}
			}
		}
	}
	
	
	
}
