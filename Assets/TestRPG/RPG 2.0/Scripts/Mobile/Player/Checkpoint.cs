using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	private void Start(){
		tag= GameManager.GameSettings.checkpointTag;
	}
	
	private void OnTriggerEnter(Collider collider){
		if(GameManager.Player.Checkpoint.sqrMagnitude != transform.position.sqrMagnitude){
			GameManager.Player.Checkpoint=transform.position;
			InterfaceContainer.Instance.checkpointWindow.SetActive(true);
			StartCoroutine(DisableUI());
		}
	}
	
	private IEnumerator DisableUI(){
		yield return new WaitForSeconds(2);
		InterfaceContainer.Instance.checkpointWindow.SetActive(false);
	}
}
