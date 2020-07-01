using UnityEngine;
using System.Collections;

public class UIToogleActivate : MonoBehaviour {
	public GameObject target;
	
	private void OnClick(){
		target.SetActive(!target.activeSelf);
	}
}
