using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomAStarAgent : MonoBehaviour {
	private CharacterController controller;
	private AiBehaviour behaviour;
	private List<Vector3> path;
	private float lastPathSearch;
	private Vector3 currentDestination;
	private bool searching;
	
	private void Start(){
		controller=GetComponent<CharacterController>();
		behaviour= GetComponent<AiBehaviour>();
		path= new List<Vector3>();
		
		behaviour.externalMove+=Move;
		behaviour.externalStopMove+=StopMove;
	}
	
	private void SetPath(List<Vector3> p){
		this.path=p;
		this.path.RemoveAt(0);
		searching=false;
	}
	
	private void SearchPath(Vector3 start, Vector3 end){
		searching=true;
		PathFinder.Instance.GetPath(start,end, SetPath);
	}
	
	public void Move(Vector3 point, float moveSpeed, float rotationSpeed){
		
		if (Vector3.Distance (point, transform.position) > 0.5f && Time.time > lastPathSearch && !searching) {
			lastPathSearch = Time.time + 2.0f;
			SearchPath(transform.position,point);
		}
		
		if (path.Count > 0) {
			currentDestination = path [0];
			if (Vector3.Distance (currentDestination, transform.position) < 1f) {
				path.RemoveAt (0);
			}
		}else{
			currentDestination=point;
		}
		
		Vector3 dir = currentDestination - transform.position;
		transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);

		if (dir != Vector3.zero) {
			Quaternion wantedRotation = Quaternion.LookRotation (dir);
			transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationSpeed);
		}
			
		controller.SimpleMove (transform.TransformDirection (Vector3.forward) * moveSpeed);
	}
	
	public void StopMove(){
		path.Clear();
	}
	
	private void Update(){
		controller.SimpleMove(Vector3.down*15);
	}
}
