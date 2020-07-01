using UnityEngine;
using System.Collections;

public class UIDeactivateOnMove : MonoBehaviour {
	public GameObject target;
	private Vector3 lastPlayerPosition;
	
	private void OnEnable () {
       lastPlayerPosition=GameManager.Player.transform.position;
    }
	
	private void Update(){
		float delta = (GameManager.Player.transform.position - lastPlayerPosition).sqrMagnitude;
		if (delta > 0.1f) {
			target.SetActive(false);
		}
	}
}
