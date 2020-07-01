using UnityEngine;
using System.Collections;

public class SpinObject : MonoBehaviour {
	public Transform target;
	public float speed;
	
	private void Start(){
		if(target == null){
			target=transform;
		}
	}

	void Update () {
		if(Input.GetMouseButton(0)){
			  target.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * speed);
		}
	}
}
