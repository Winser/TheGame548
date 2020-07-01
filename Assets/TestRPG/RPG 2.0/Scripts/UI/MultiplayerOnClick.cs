using UnityEngine;
using System.Collections;

public class MultiplayerOnClick : MonoBehaviour {
	public bool state=true;
	private void OnClick(){
		PhotonNetwork.offlineMode=state;
	}
}
