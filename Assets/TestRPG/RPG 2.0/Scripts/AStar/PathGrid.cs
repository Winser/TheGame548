using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public class PathGrid 
{
	public Rect area = new Rect (0, 0, 100, 100);
	public float gridSize = 1.0f;
	public LayerMask walkableLayer;
	public LayerMask ignore;
	public float maxSlope = 25;
	[NonSerialized]
	public PathNode[] nodes;
	public bool showConnection;
	public bool showNodes;
	public bool showWalkable;
	[HideInInspector]
	public float timeUsed = 0;
	public bool expand;
	public float scanHeight=500;
    public PathGrid() {
    }

	public void Scan ()
	{
		float timeStart = Time.realtimeSinceStartup;
		
		nodes = new PathNode[(int)Mathf.Ceil ((area.width) / gridSize) * (int)Mathf.Ceil ((area.height) / gridSize)];

		int cnt = 0;
		for (float i = area.x; i < area.x + area.width; i += gridSize) {
			for (float j = area.y; j < area.y + area.height; j += gridSize) {
				RaycastHit hit;
				if (Physics.Raycast (new Vector3 (i + gridSize / 2, scanHeight, j + gridSize / 2), Vector3.down, out hit, Mathf.Infinity,~ignore)) {
					
					if ((walkableLayer & (1 << hit.collider.gameObject.layer)) != 0) {
						
						nodes [cnt] = new PathNode (cnt, new Vector3 (i + gridSize / 2, hit.point.y, j + gridSize / 2), true);
					} else {
						nodes [cnt] = new PathNode (cnt, new Vector3 (i + gridSize / 2, hit.point.y, j + gridSize / 2), false);
					}
					
				}
				cnt++;
			}
		}
		
		SetupNeighbours ();
		Clustering();
		timeUsed = Time.realtimeSinceStartup - timeStart;
		
	}
	
	public void SetupNeighbours ()
	{
		float neighbourRange = gridSize * 1.5f;
		float neighbourDistance = 0;
		
		int rowLength = (int)(area.height / gridSize);
		int columnLength = (int)(area.width / gridSize);
		
		foreach (PathNode currentNode in nodes) {
			if(currentNode == null){
				continue;
			}
			List<PathNode> neighbourNodeList = new List<PathNode> ();
			List<float> neighbourCostList = new List<float> ();
				
			PathNode[] neighbour;
			List<PathNode> neighbourList = new List<PathNode> ();
			int id = currentNode.id;
			
			if (id > rowLength - 1 && id < rowLength * columnLength - rowLength) {
				if (id != rowLength)
					neighbourList.Add (nodes [id - rowLength - 1]);
				neighbourList.Add (nodes [id - rowLength]);
				neighbourList.Add (nodes [id - rowLength + 1]);
				neighbourList.Add (nodes [id - 1]);
				neighbourList.Add (nodes [id + 1]);
				neighbourList.Add (nodes [id + rowLength - 1]);
				neighbourList.Add (nodes [id + rowLength]);
				if (id != rowLength * columnLength - rowLength - 1)
					neighbourList.Add (nodes [id + rowLength + 1]);
			} else if (id <= rowLength - 1) {
				if (id != 0)
					neighbourList.Add (nodes [id - 1]);
				if (nodes.Length > id + 1)
					neighbourList.Add (nodes [id + 1]);
				if (columnLength > 0) {
					if (nodes.Length > id + rowLength - 1)
						neighbourList.Add (nodes [id + rowLength - 1]);
					if (nodes.Length > id + rowLength)
						neighbourList.Add (nodes [id + rowLength]);
					if (nodes.Length > id + rowLength + 1)
						neighbourList.Add (nodes [id + rowLength + 1]);
				}
			} else if (id >= rowLength * columnLength - rowLength) {
				neighbourList.Add (nodes [id - 1]);
				if (id != rowLength * columnLength - 1)
					neighbourList.Add (nodes [id + 1]);
				neighbourList.Add (nodes [id - rowLength - 1]);
				neighbourList.Add (nodes [id - rowLength]);
				neighbourList.Add (nodes [id - rowLength + 1]);
			}
				
			neighbour = neighbourList.ToArray ();
			
			foreach (PathNode node in neighbour) {
				if (node != null) {
					
					neighbourDistance = GetHorizontalDistance (currentNode.position, node.position);
					if (neighbourDistance <= neighbourRange) { 
						
						if (Mathf.Abs (GetSlope (currentNode.position, node.position)) > maxSlope) {
							node.walkable = false;
						}
						neighbourNodeList.Add (node);
						neighbourCostList.Add (neighbourDistance);
						
						
					}
				}
				
			}
			currentNode.SetNeighbour (neighbourNodeList, neighbourCostList);
		}
	}
	
	private float GetHorizontalDistance (Vector3 p1, Vector3 p2)
	{
		p1.y = 0;
		p2.y = 0;
		float distH = Vector3.Distance (p1, p2);
		return distH;
	}

	private float GetSlope (Vector3 p1, Vector3 p2)
	{
		float h1 = p1.y;
		float h2 = p2.y;

		float distH = GetHorizontalDistance (p1, p2);
		float slope = (h1 - h2) / distH;

		return Mathf.Atan (slope) * Mathf.Rad2Deg;
	}
	
	public void Clustering ()
	{
		int sectionCount = 1;
		List<PathNode> openList = new List<PathNode> ();
		PathNode node = GetUnsectionedNode ();
		if(node != null){
			node.section = sectionCount;
		}
		while (node!=null) {
			foreach (PathNode neighbour in node.neighbourNodes) {
				if (neighbour.section == 0 && neighbour.walkable) {
					neighbour.section = sectionCount;
					openList.Add (neighbour);
				}
			}

			if (openList.Count > 0) {
				node = openList [0];
				openList.RemoveAt (0);
			} else
				node = null;


			if (node == null) {
				node = GetUnsectionedNode ();
				if (node != null) {
					sectionCount += 1;
					node.section = sectionCount;
				}
			}
		}
	}
	
	private PathNode GetUnsectionedNode ()
	{
		PathNode targetNode = null;
		foreach (PathNode node in nodes) {
			if (node != null && node.walkable && node.section == 0) {
				targetNode = node;
				break;
			} 
		}
		
		return targetNode;
	}
	
	private List<float> heapF = new List<float> ();

	public void ResetList ()
	{
		heapF = new List<float> ();
	}

	public void InsertToHeap (float val)
	{

		heapF.Add (val);

		int currentPos = heapF.Count - 1;

		while (currentPos > 0) {
			int parentPos = (currentPos - 1) / 2;
			if (heapF [currentPos] < heapF [parentPos]) {
				heapF [currentPos] = heapF [parentPos];
				heapF [parentPos] = val;

				currentPos = parentPos;
			} else {
				break;
			}
		}

		string displayMsg = "";
		foreach (float value in heapF) {
			displayMsg += value.ToString ("f1") + "  -  ";
		}
		Debug.Log (displayMsg);

	}

	public void RemoveFromHeap (int pos)
	{
		heapF [pos] = heapF [heapF.Count - 1];
		heapF.RemoveAt (heapF.Count - 1);

		int currentPos = pos;
		while (currentPos < heapF.Count) {

			int childPos1 = currentPos * 2 + 1;
			int childPos2 = currentPos * 2 + 2;
			int childPos = childPos1;

			if (childPos2 < heapF.Count) {
				if (heapF [childPos1] > heapF [childPos2]) {
					childPos = childPos2;
				}
			}

			if (childPos < heapF.Count && heapF [currentPos] > heapF [childPos]) {
				float temp = heapF [currentPos];
				heapF [currentPos] = heapF [childPos];
				heapF [childPos] = temp;

				currentPos = childPos;
			} else {
				break;
			}
		}

		string displayMsg = "";
		foreach (float value in heapF) {
			displayMsg += value.ToString ("f1") + "  -  ";
		}
		Debug.Log (displayMsg);

	}

	public List<PathNode> InsertToList (List<PathNode> nodeList, float val)
	{

		heapF.Add (val);

		int currentPos = heapF.Count - 1;

		while (currentPos > 0) {
			int parentPos = (currentPos - 1) / 2;
			if (heapF [currentPos] <= heapF [parentPos]) {
				float temp = heapF [currentPos];
				heapF [currentPos] = heapF [parentPos];
				heapF [parentPos] = temp;

				PathNode tempNode = nodeList [currentPos];
				nodeList [currentPos] = nodeList [parentPos];
				nodeList [parentPos] = tempNode;

				currentPos = parentPos;
			} else {
				break;
			}
		}

		return nodeList;
	}

	public List<PathNode> RemoveFromList (List<PathNode> nodeList, int pos)
	{

		heapF [pos] = heapF [heapF.Count - 1];
		heapF.RemoveAt (heapF.Count - 1);

		int currentPos = pos;
		while (currentPos < heapF.Count) {

			int childPos1 = currentPos * 2 + 1;
			int childPos2 = currentPos * 2 + 2;
			int childPos = childPos1;

			if (childPos2 < heapF.Count) {
				if (heapF [childPos1] >= heapF [childPos2]) {
					childPos = childPos2;
				}
			}

			if (childPos < heapF.Count && heapF [currentPos] >= heapF [childPos]) {
				float temp = heapF [currentPos];
				heapF [currentPos] = heapF [childPos];
				heapF [childPos] = temp;

				PathNode tempNode = nodeList [currentPos];
				nodeList [currentPos] = nodeList [childPos];
				nodeList [childPos] = tempNode;

				currentPos = childPos;
			} else {
				break;
			}
		}

		return nodeList;
	}
	
	public void OnDrawGizmos ()
	{
		if (nodes != null) {
			foreach (PathNode node in nodes) {
				if (node != null && node.neighbourNodes !=null) {
					if (showConnection) {
						foreach (PathNode neighbour in node.neighbourNodes) {
							if (neighbour != null && neighbour.id > node.id) {
							
								if (neighbour.walkable) {
									Gizmos.color = Color.blue;
								} else {
									Gizmos.color = Color.red;
									if (showWalkable) {
										continue;
									}
								}
							
								if (!node.walkable) {
									Gizmos.color = Color.red;
									if (showWalkable) {
										continue;
									}
								}
							
								if (Vector3.Distance (node.position + Vector3.up * 0.2f, neighbour.position + Vector3.up * 0.2f) < 4f * gridSize)
									Gizmos.DrawLine (node.position + Vector3.up * 0.2f, neighbour.position + Vector3.up * 0.2f);
							}
						}
					}
					
					if (showNodes) {
						if (node.walkable) {
							Gizmos.color = Color.green;
						} else {
							Gizmos.color = Color.red;
						}
						Gizmos.DrawCube (node.position + Vector3.up * 0.2f, new Vector3 (0.2f, 0.2f, 0.2f));
					}
					
				}
			}
		}
	}
}
