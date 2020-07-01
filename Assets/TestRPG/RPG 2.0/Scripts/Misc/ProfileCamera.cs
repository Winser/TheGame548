using UnityEngine;
using System.Collections;

public class ProfileCamera : MonoBehaviour {
	public PhotonView photonView;
	
	void Start () {
		if(!photonView.isMine){
			Destroy(gameObject);
		}
	}
	
}
