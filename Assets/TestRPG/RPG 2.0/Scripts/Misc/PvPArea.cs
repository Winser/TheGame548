using UnityEngine;
using System.Collections;

public class PvPArea : MonoBehaviour {
	public float range=5.0f;
	
	private void Start(){
		BoxCollider box=gameObject.AddComponent<BoxCollider>();
		box.extents=new Vector3(range,range,range);
		box.isTrigger=true;
		box.tag=GameManager.GameSettings.checkpointTag;
	}
	
	private void OnTriggerEnter(Collider other) {
 		if(other.transform == GameManager.Player.transform){
			GameManager.GameSettings.allowPvp=true;
			MessageManager.Instance.AddMessage(GameManager.GameMessages.enterPVPZone);
		}
    }
	
	private void OnTriggerExit(Collider other) {
		if(other.transform == GameManager.Player.transform){
			GameManager.GameSettings.allowPvp=false;
			MessageManager.Instance.AddMessage(GameManager.GameMessages.exitPVPZone);
		}
    }
}
