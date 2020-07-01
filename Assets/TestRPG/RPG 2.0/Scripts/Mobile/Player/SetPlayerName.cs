using UnityEngine;
using System.Collections;

public class SetPlayerName : MonoBehaviour {
	void Start () {
		if(DataStorage.Instance != null){
			GetComponent<UILabel>().text=DataStorage.Instance.playerName;
		}
	}
}
