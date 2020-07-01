using UnityEngine;
using System.Collections;

public class EnterRoom : MonoBehaviour {
	public UILabel roomLabel;
	private void OnClick(){
		PhotonNetwork.JoinRoom(roomLabel.text);
	}
}
