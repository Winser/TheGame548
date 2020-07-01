using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(WaypointManager))]
public class WaypointManagerInspector : Editor
{

	private WaypointManager manager;
	private List<bool> foldOut;
		
	private void OnEnable ()
	{
		manager = (WaypointManager)target;
		foldOut = new List<bool> ();
		for (int i=0; i<100; i++) {
			foldOut.Add (false);
		}
	}
	
	public override void OnInspectorGUI ()
	{
		GUI.changed = false;
		if (GUILayout.Button ("Add new path")) {
			manager.waypointPaths.Add (new WaypointPath ());
			foldOut.Add (true);
		}
		
		for (int i=0; i< manager.waypointPaths.Count; i++) {
			foldOut [i] = EditorGUILayout.Foldout (foldOut [i], "Waypoint " + manager.waypointPaths [i].id);
			if (foldOut [i]) {
				manager.waypointPaths [i].id = EditorGUILayout.IntField ("Id", manager.waypointPaths [i].id);
				manager.waypointPaths [i].type = (WaypointPathType)EditorGUILayout.EnumPopup ("Type", manager.waypointPaths [i].type);
				if (GUILayout.Button ("Add new Point")) {
					if(manager.waypointPaths[i].waypoints.Count>0){
						manager.waypointPaths [i].waypoints.Add (new Vector3(manager.waypointPaths[i].waypoints[manager.waypointPaths[i].waypoints.Count-1].x,manager.waypointPaths[i].waypoints[manager.waypointPaths[i].waypoints.Count-1].y,manager.waypointPaths[i].waypoints[manager.waypointPaths[i].waypoints.Count-1].z));
					}else{
						manager.waypointPaths [i].waypoints.Add (new Vector3 ());
					}
				}
				for (int v=0; v< manager.waypointPaths[i].waypoints.Count; v++) {
					GUILayout.BeginHorizontal ();
					if (GUILayout.Button ("X")) {
					
					}
					GUILayout.Label (manager.waypointPaths [i].waypoints [v].ToString ());
					GUILayout.EndHorizontal ();
				}
			}
		}
		
		if (GUI.changed) {
			SceneView.RepaintAll ();
			EditorUtility.SetDirty (target);
		}
	}
	
	private void OnSceneGUI ()
	{
		foreach (WaypointPath waypoint in manager.waypointPaths) {
		
			for (int i=0; i < waypoint.waypoints.Count; i++) {
				waypoint.waypoints [i] = Handles.PositionHandle (waypoint.waypoints [i], Quaternion.identity);
				if (!Application.isPlaying) {
					RaycastHit hit;
					if (Physics.Raycast (waypoint.waypoints [i] + Vector3.up * 500, Vector3.down, out hit)) {
						waypoint.waypoints [i] = new Vector3 (waypoint.waypoints [i].x, hit.point.y, waypoint.waypoints [i].z);
					}
				}
				if (i + 1 < waypoint.waypoints.Count) {
					Handles.DrawLine (waypoint.waypoints [i], waypoint.waypoints [i + 1]);
				} else {
					if (waypoint.type.Equals (WaypointPathType.Loop) && waypoint.waypoints.Count > 0) {
						Handles.DrawLine (waypoint.waypoints [i], waypoint.waypoints [0]);
					}
				}
						
			}
			
		}
	}
}
