using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WaypointPathType{Loop, PingPong}

[System.Serializable]
public class WaypointPath {
	public int id;
	public List<Vector3> waypoints= new List<Vector3>();
	public WaypointPathType type;
}
