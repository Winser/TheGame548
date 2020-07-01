using UnityEngine;
using System.Collections;
using System.IO;

public class LoadLevelOnStart : MonoBehaviour {
	public string level;
	private void Start(){
		if(PhotonNetwork.room != null && PhotonNetwork.room.customProperties.ContainsKey("scene")){
			Application.LoadLevel((string)PhotonNetwork.room.customProperties["scene"]);
		}else{
			GameManager.GameDatabase.LoadScene(LoadScene);
		}
	}
	
	private void LoadScene(string scene){
		if(!scene.Equals(string.Empty) && DataStorage.Instance == null){
			Application.LoadLevel(scene);
		}else{
			Application.LoadLevel(level);
		}
	}
}
