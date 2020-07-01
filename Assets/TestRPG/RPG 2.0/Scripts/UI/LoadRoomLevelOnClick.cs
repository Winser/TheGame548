using UnityEngine;
using System.Collections;

public class LoadRoomLevelOnClick : MonoBehaviour {
	 public string level;
	
	private void OnClick(){
		string sceneToLoad=level;
		if(PhotonNetwork.room.customProperties.ContainsKey("scene")){
			sceneToLoad=(string)PhotonNetwork.room.customProperties["scene"];
		}
		Application.LoadLevel(sceneToLoad);
	}
}
