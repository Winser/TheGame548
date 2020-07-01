using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointManager : MonoBehaviour {
	private static WaypointManager instance;
	
	public static WaypointManager Instance{
		get{return instance;}
	}
	
	public List<WaypointPath> waypointPaths= new List<WaypointPath>();
	private void Awake(){
		instance=this;
	}
	
	public WaypointPath GetPath(int id){
		foreach(WaypointPath p in waypointPaths){
			if(p.id.Equals(id)){
				return p;
			}
		}
		return null;
	}
	
	public void InvertWaypointPath (WaypointPath path)
	{
		List<Vector3> pInverted = new List<Vector3> ();
		for (int i = path.waypoints.Count - 1; i >= 0; i--) {
			pInverted.Add (path.waypoints [i]);
		}
		path.waypoints = pInverted;
	}
	
}
