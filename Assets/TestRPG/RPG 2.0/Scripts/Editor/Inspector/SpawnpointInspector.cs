using UnityEngine;
using UnityEditor;
	
[CustomEditor(typeof(Spawnpoint))]
public class SpawnpointInspector :  Editor
{
	Spawnpoint spawnpoint;
	private void OnEnable(){
		spawnpoint=(Spawnpoint)target;
	}
	
	public override void OnInspectorGUI ()
	{
		EditorGUIUtility.LookLikeControls();
		base.OnInspectorGUI ();
	}
	
	private void OnSceneGUI ()
	{
 
		RaycastHit hit;
		if(Physics.Raycast(spawnpoint.transform.position,Vector3.down,out hit)){
			spawnpoint.transform.position=hit.point;
		}
		
		Vector3 pos = spawnpoint.transform.position;
 
		Vector3[] verts = new Vector3[]{new Vector3 (pos.x - spawnpoint.range, pos.y, pos.z - spawnpoint.range), 
                   new Vector3 (pos.x - spawnpoint.range, pos.y, pos.z + spawnpoint.range), 
                   new Vector3 (pos.x + spawnpoint.range, pos.y, pos.z + spawnpoint.range), 
                   new Vector3 (pos.x + spawnpoint.range, pos.y, pos.z - spawnpoint.range)};
		
		Handles.DrawSolidRectangleWithOutline (verts,new Color (1, 1, 1, 0.2f),Color.green);
        
		foreach (Vector3 posCube in verts)
			spawnpoint.range = Handles.ScaleValueHandle (spawnpoint.range,
                                    posCube,
                                    Quaternion.identity,
                                    2,
                                    Handles.CubeCap,
                                    1);
		EditorUtility.SetDirty (target);
		
	}
}
