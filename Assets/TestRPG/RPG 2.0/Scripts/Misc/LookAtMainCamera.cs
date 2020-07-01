using UnityEngine;
using System.Collections;


[AddComponentMenu("RPG Kit 2.0/LookAtMainCamera")]
public class LookAtMainCamera : MonoBehaviour {
	void Update () {
		transform.LookAt(Camera.main.transform);
	}
}
