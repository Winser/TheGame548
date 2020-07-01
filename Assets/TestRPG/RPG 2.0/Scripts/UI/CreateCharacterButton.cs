using UnityEngine;
using System.Collections;

public class CreateCharacterButton : MonoBehaviour {

	public string singleplayerLevel="PreLoad";
	public string multiplayerLevel="Lobby";
	

	void OnClick ()
	{
		Application.LoadLevel(PhotonNetwork.offlineMode?singleplayerLevel:multiplayerLevel);
	}
}
