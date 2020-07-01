using UnityEngine;
using System.Collections;

public class UIActivate : MonoBehaviour {
	public GameObject target;
	public bool state;
	
	public void OnClick(){
		if(target){
			target.SetActive(state);
		}
	}
	
}
