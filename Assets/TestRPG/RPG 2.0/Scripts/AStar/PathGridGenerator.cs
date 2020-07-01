using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("RPG Kit 2.0/PathGridGenerator")]
public class PathGridGenerator : MonoBehaviour
{
	[HideInInspector]
	public List<PathGrid> grids = new List<PathGrid> ();
	private static PathGridGenerator instance;
	public static PathGridGenerator Instance {
		get{ return instance;}
	}
	
	private void Awake ()
	{
		instance = this;
	}
	
	private void Start ()
	{
		RuntimeScan();
	}
	
	public void RuntimeScan ()
	{
		foreach (PathGrid grid in grids) {
			grid.Scan ();
		}
	}
	
	public IEnumerator RuntimeDynamicScan (float delay)
	{
		while (true) {
			foreach (PathGrid grid in grids) {
				grid.Scan ();
				yield return new WaitForSeconds(delay);
			}
		}
	}
	
	public PathGrid GetPathGrid (Vector3 position)
	{
		
		float dist = Mathf.Infinity;
		float currentNearest = Mathf.Infinity;
		PathGrid nearest = null;

		foreach (PathGrid grid in grids) {
			
			dist = Vector3.Distance (position, new Vector3(grid.area.center.x,0,grid.area.center.y));
			if (dist < currentNearest) {
				currentNearest = dist;
				nearest = grid;
			}
			
		}
		
		return nearest;
	}
	
	private void OnDrawGizmos ()
	{
		foreach (PathGrid grid in grids) {
			grid.OnDrawGizmos ();
		}
	}
}
