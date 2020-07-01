using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MeshHolder : MonoBehaviour {
	public List<Transform> thirdPerson;
	public List<Transform> firstPerson;
	
	public void EnableThirdPerson(){
		foreach(Transform tr in thirdPerson){
			tr.gameObject.SetActive(true);
		}
		
		foreach(Transform tr in firstPerson){
			tr.gameObject.SetActive(false);
		}
	}
	
	public void EnableFirstPerson(){
		foreach(Transform tr in thirdPerson){
			tr.gameObject.SetActive(false);
		}
		
		foreach(Transform tr in firstPerson){
			tr.gameObject.SetActive(true);
		}
	}
}

