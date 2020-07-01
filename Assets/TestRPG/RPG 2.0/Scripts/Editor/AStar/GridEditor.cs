using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(PathGridGenerator))]
public class GridEditor : Editor
{
	PathGridGenerator generator;

	public void OnEnable ()
	{
		generator = (PathGridGenerator)target;
	}
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		GUI.changed = false;

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Add Grid")) {
			generator.grids.Add (new PathGrid ());
		}
		GUILayout.EndHorizontal ();

		PathGrid removegrid = null;
		
		foreach (PathGrid grid in generator.grids) {
			GUILayout.BeginVertical ("box");
			grid.expand = EditorGUILayout.Foldout (grid.expand, "Path Grid");
			if (grid.expand) {
				grid.area = EditorGUILayout.RectField ("Area", grid.area);
				EditorGUILayout.Separator ();
				grid.gridSize = EditorGUILayout.FloatField ("Grid Size", grid.gridSize);
				grid.scanHeight=EditorGUILayout.FloatField("Scan Height", grid.scanHeight);
				grid.maxSlope = EditorGUILayout.FloatField ("Max Slope", grid.maxSlope);
				grid.walkableLayer = EditorUtils.LayerMaskField ("Walkable Layer", grid.walkableLayer);
				grid.ignore = EditorUtils.LayerMaskField ("Ignore Layer", grid.ignore);
				grid.showNodes = EditorGUILayout.Toggle ("Show Nodes", grid.showNodes);
				grid.showConnection = EditorGUILayout.Toggle ("Show Connection", grid.showConnection);
				grid.showWalkable = EditorGUILayout.Toggle ("Show Walkable", grid.showWalkable);
				
                #region Last Scan
				GUILayout.Label ("Last Scan");
				GUILayout.Label ("Time used: " + grid.timeUsed + " seconds");
				if (grid.nodes != null) {
					GUILayout.Label ("Generated Nodes: " + grid.nodes.Length);
				} else {
					GUILayout.Label ("Generated Nodes: 0");
				}
                #endregion Last Scan

				GUILayout.BeginHorizontal ();
				
				if (GUILayout.Button ("Scan")) {
					grid.Scan ();
				}

				if (GUILayout.Button ("Remove")) {
					removegrid = grid;
				}
				GUILayout.EndHorizontal ();
			}
			GUILayout.EndVertical ();
		}

		if (removegrid != null) {
			generator.grids.Remove (removegrid);
		}
		
		if (GUI.changed) {
			SceneView.RepaintAll ();
			EditorUtility.SetDirty (target);
		}
	}
	
	private void OnSceneGUI ()
	{
		foreach (PathGrid grid in generator.grids) {
			Vector3 pos = new Vector3 (grid.area.x, 0, grid.area.y);
 
			Vector3[] verts = new Vector3[]{
					new Vector3 (pos.x, pos.y, pos.z), 
                   	new Vector3 (pos.x + grid.area.width, pos.y, pos.z), 
                   	new Vector3 (pos.x + grid.area.width, pos.y, pos.z + grid.area.height), 
                   	new Vector3 (pos.x, pos.y, pos.z + grid.area.height)
			};
		
			Handles.DrawSolidRectangleWithOutline (verts, new Color (0, 0, 0.5f, 0.2f), Color.green);
		
	
			
			
			Vector3 gridPos = new Vector3 (grid.area.x, 0, grid.area.y);
			gridPos = Handles.PositionHandle (gridPos, Quaternion.identity);
			grid.area.x = gridPos.x;
			grid.area.y = gridPos.z;
			
			Vector3 gridScale = new Vector3 (grid.area.width, 0, grid.area.height);
			gridScale = Handles.ScaleHandle (gridScale, 
                        gridPos,
                        Quaternion.identity,
                        5);
			grid.area.width = (int)gridScale.x;
			grid.area.height = (int)gridScale.z;
			//EditorUtility.SetDirty (target);
		}
		
		
		/*foreach (Waypoint waypoint in generator.waypoints) {
			if (waypoint.expand) {
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
						if (waypoint.type.Equals (Waypoint.WaypointType.Loop) && waypoint.waypoints.Count > 0) {
							Handles.DrawLine (waypoint.waypoints [i], waypoint.waypoints [0]);
						}
					}
						
				}
			}
		}*/
		
		EditorUtility.SetDirty (target);
	}
}
