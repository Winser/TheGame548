using UnityEngine;
using System.Collections;

public class DataStorage : MonoBehaviour {
private static DataStorage instance;
	public static DataStorage Instance{
		get{return instance;}
	}
	
	private void Awake(){
		DontDestroyOnLoad (transform.gameObject);
		instance=this;
	}
	
	[HideInInspector]
	public string playerName;
	[HideInInspector]
	public Character character;
	
	private void SetName(string pName){
		playerName=pName;
		PhotonNetwork.playerName=pName;
	}
}
