using UnityEngine;
using System.Collections;

public class DontDestroyOnLevelChange : MonoBehaviour {
	private void Awake(){
		DontDestroyOnLoad (transform.gameObject);
	}
}
