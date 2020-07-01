using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PvPArea))]
public class PVPAreaInspector : Editor {
	PvPArea area;
	private void OnEnable(){
		area=(PvPArea)target;
	}
	
	private void OnSceneGUI ()
	{
 
		RaycastHit hit;
		if(Physics.Raycast(area.transform.position,Vector3.down,out hit)){
			area.transform.position=hit.point;
		}
		
		Vector3 pos = area.transform.position;
 
		Vector3[] verts = new Vector3[]{new Vector3 (pos.x - area.range, pos.y, pos.z - area.range), 
                   new Vector3 (pos.x - area.range, pos.y, pos.z + area.range), 
                   new Vector3 (pos.x + area.range, pos.y, pos.z + area.range), 
                   new Vector3 (pos.x + area.range, pos.y, pos.z - area.range)};
		
		Handles.DrawSolidRectangleWithOutline (verts,new Color (1, 1, 1, 0.2f),Color.green);
        
		foreach (Vector3 posCube in verts)
			area.range = Handles.ScaleValueHandle (area.range,
                                    posCube,
                                    Quaternion.identity,
                                    2,
                                    Handles.CubeCap,
                                    1);
		EditorUtility.SetDirty (target);
		
	}
}
