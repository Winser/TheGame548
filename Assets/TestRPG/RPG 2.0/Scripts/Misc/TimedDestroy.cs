using UnityEngine;
using System.Collections;

public class TimedDestroy : Photon.MonoBehaviour {
	public float time;
	void Start () {
		if(photonView.isMine){
			StartCoroutine(TimedNetworkDestroy());
		}
	}
	
	private IEnumerator TimedNetworkDestroy(){
		yield return new WaitForSeconds(time);
		PhotonNetwork.Destroy(gameObject);
	}
}
