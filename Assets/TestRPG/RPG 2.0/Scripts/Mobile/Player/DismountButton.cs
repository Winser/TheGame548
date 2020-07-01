using UnityEngine;
using System.Collections;

public class DismountButton : MonoBehaviour {

	private void OnClick(){
		StartCoroutine(Dismount());
	}
	
	private IEnumerator Dismount(){
		yield return new WaitForSeconds(0.3f);
		GameManager.Player.Dismount();
	}
}
