using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void SetPathCallback (List<Vector3> path);

[AddComponentMenu("RPG Kit 2.0/PathFinder")]
public class PathFinder : MonoBehaviour
{
	private static PathFinder instance;
	
	public static PathFinder Instance {
		get{ return instance;}
	}
	
	private bool searching;
	private PathGrid startGrid;
	private PathGrid endGrid;
	
	private void Awake ()
	{
		instance = this;
	}

	public void GetPath (Vector3 start, Vector3 end, SetPathCallback callBack)
	{
			if (!searching) {
				startGrid = PathGridGenerator.Instance.GetPathGrid (start);
				endGrid = PathGridGenerator.Instance.GetPathGrid (end);
				
				bool oneGrid = (startGrid == endGrid);
			
			
				PathNode startNode = GetNearestNode (start, startGrid);
				PathNode endNode = GetNearestNode (end, startGrid);
				PathNode realEndNode = null;
			
				if (startNode.section != endNode.section) {
					endNode = GetNearestNodeToCluster (endNode, startGrid, startNode.section);
				}
			
				if (!oneGrid) {
					if (endGrid != null) {
						realEndNode = GetNearestNode (end, endGrid);
					}
				}

        		Search (startNode, endNode, realEndNode, callBack);
			
			}

		   
	}
	
	private void Search (PathNode start, PathNode end, PathNode realEnd, SetPathCallback callBack)
	{
		#region First Grid	
		searching = true;
		bool pathFound = true;

		List<PathNode> closeList = new List<PathNode> ();
		List<PathNode> openList = new List<PathNode> ();
		startGrid.ResetList ();

		PathNode currentNode = start;

		while (true) {
			closeList.Add (currentNode);
			currentNode.listState = ListState.Close;
			currentNode.ProcessNeighbour (end.position);

			foreach (PathNode neighbour in currentNode.neighbourNodes) {
				if (neighbour.listState == ListState.Unassigned && neighbour.walkable) {
					neighbour.listState = ListState.Open;
					openList.Add (neighbour);
					openList = startGrid.InsertToList (openList, neighbour.scoreF);
				}
			}

			if (openList.Count == 0) {
				pathFound = false;
				break;
			} else {
				currentNode = openList [0];
				openList [0] = openList [openList.Count - 1];
				openList.RemoveAt (openList.Count - 1);

				openList = startGrid.RemoveFromList (openList, 0);
			}

			if (currentNode == end) {
				break;
			}
		}

		List<Vector3> p = new List<Vector3> ();
		if (pathFound) {
			while (currentNode != null) {
				p.Add (currentNode.position);
				currentNode = currentNode.parent;
			}
		}
		
		p = InvertArray (p);
		p.Add (end.position);
		
		#endregion First Grid
		
		#region Second Grid
		if (realEnd != null) {
			pathFound = true;

			closeList = new List<PathNode> ();
			openList = new List<PathNode> ();
			endGrid.ResetList ();

			currentNode = GetNearestNode (end.position, endGrid);
			if (Vector3.Distance (end.position, currentNode.position) < 1.5f * endGrid.gridSize) {
				
				if (currentNode.section != realEnd.section) {
					realEnd = GetNearestNodeToCluster (realEnd, endGrid, currentNode.section);
				}
			
			
				while (true) {
					closeList.Add (currentNode);
					currentNode.listState = ListState.Close;
					currentNode.ProcessNeighbour (realEnd.position);

					foreach (PathNode neighbour in currentNode.neighbourNodes) {
						if (neighbour.listState == ListState.Unassigned && neighbour.walkable) {
							neighbour.listState = ListState.Open;
							openList.Add (neighbour);
							openList = endGrid.InsertToList (openList, neighbour.scoreF);
						}
					}

					if (openList.Count == 0) {
						pathFound = false;
						break;
					} else {
						currentNode = openList [0];
						openList [0] = openList [openList.Count - 1];
						openList.RemoveAt (openList.Count - 1);

						openList = endGrid.RemoveFromList (openList, 0);
					}

					if (currentNode == realEnd) {
						break;
					}
				}

				List<Vector3> p2 = new List<Vector3> ();
				if (pathFound) {
					while (currentNode != null) {
						p2.Add (currentNode.position);
						currentNode = currentNode.parent;
					}
				}
			
				p2 = InvertArray (p2);
				p2.Add (realEnd.position);
		
				p.AddRange (p2);
				ResetPathNodes (endGrid);
			}
		}
		#endregion Second Grid
		
		callBack (p);

		ResetPathNodes (startGrid);
		
		searching = false;
	}
	
	private List<Vector3> InvertArray (List<Vector3> p)
	{
		List<Vector3> pInverted = new List<Vector3> ();
		for (int i = p.Count - 1; i >= 0; i--) {
			pInverted.Add (p [i]);
		}
		return pInverted;
	}

	private void ResetPathNodes (PathGrid grid)
	{
		foreach (PathNode node in grid.nodes) {
			node.listState = ListState.Unassigned;
			node.parent = null;
		}
	}
	
	public PathNode GetNearestNode (Vector3 point, PathGrid grid)
	{
		float dist = Mathf.Infinity;
		float currentNearest = Mathf.Infinity;
		PathNode nearestNode = null;

		foreach (PathNode node in grid.nodes) {
			if (node.walkable) {
				dist = Vector3.Distance (point, node.position);
				if (dist < currentNearest) {
					currentNearest = dist;
					nearestNode = node;
				}
			}
		}
		
		return nearestNode;
	}
	
	private PathNode GetNearestNodeToCluster (PathNode targetNode, PathGrid grid, int section)
	{
		float dist = Mathf.Infinity;
		float currentNearest = Mathf.Infinity;
		PathNode nearestNode = null;
		
		foreach (PathNode node in grid.nodes) {
			if (node.walkable && node.section == section) {
				dist = Vector3.Distance (targetNode.position, node.position);
				if (dist < currentNearest) {
					currentNearest = dist;
					nearestNode = node;
				}
			}
		}
		return nearestNode;
	}
}
