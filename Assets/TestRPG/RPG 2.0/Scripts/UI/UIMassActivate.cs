using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMassActivate : MonoBehaviour {
	public List<GameObject> targets;
	public bool state;
	
	public void OnClick(){
		foreach(GameObject target in targets){
			target.SetActive(state);
		}
	}
}
