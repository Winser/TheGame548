using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ListState
{
	Unassigned,
	Open,
	Close
};

[Serializable]
public class PathNode 
{
	public int id;
	public Vector3 position;
	public bool walkable;
    [NonSerialized]
	public PathNode[] neighbourNodes;
    [NonSerialized]
	public float[] neighbourCost;
    [NonSerialized]
	public PathNode parent;
	[NonSerialized]
	public ListState listState = ListState.Unassigned;
	[NonSerialized]
	public float scoreG;
	[NonSerialized]
	public float scoreH;
	[NonSerialized]
	public float scoreF;
	[NonSerialized]
	public float tempScoreG = 0;
	[NonSerialized]
	public int section=0;
	
	public PathNode (int id, Vector3 position, bool walkable)
	{
		this.id = id;
		this.position = position;
		this.walkable = walkable;
	}
	
	public void SetNeighbour (List<PathNode> arrNeighbour, List<float> arrCost)
	{
		neighbourNodes = arrNeighbour.ToArray ();
		neighbourCost = arrCost.ToArray ();
	}
	
	public void ProcessNeighbour (PathNode node)
	{
		ProcessNeighbour (node.position);
	}
	
	public void ProcessNeighbour (Vector3 pos)
	{
		for (int i=0; i<neighbourNodes.Length; i++) {

			if (neighbourNodes [i].listState == ListState.Unassigned) {
				neighbourNodes [i].scoreG = scoreG + neighbourCost [i];
				neighbourNodes [i].scoreH = Vector3.Distance (neighbourNodes [i].position, pos);
				neighbourNodes [i].UpdateScoreF ();
				neighbourNodes [i].parent = this;
			} else if (neighbourNodes [i].listState == ListState.Open) {
				tempScoreG = scoreG + neighbourCost [i];
				if (neighbourNodes [i].scoreG > tempScoreG) {
					neighbourNodes [i].parent = this;
					neighbourNodes [i].scoreG = tempScoreG;
					neighbourNodes [i].UpdateScoreF ();
				}
			}
			
		}
	}
	
	void UpdateScoreF ()
	{
		scoreF = scoreG + scoreH;
	}
	
	public PathNode CloneNode ()
	{
		PathNode node = new PathNode (id, position, walkable);
		node.neighbourNodes = neighbourNodes;
		node.neighbourCost = neighbourCost;
		node.parent = parent;
		node.scoreG = scoreG;
		node.scoreH = scoreH;
		node.scoreF = scoreF;
		node.listState = listState;
		return node;
	}
}
