using UnityEngine;
using System.Collections;

public class TimedDestoryLocal : MonoBehaviour {
	public float time;
	void Start () {
		Destroy(gameObject, time);
	}
}
