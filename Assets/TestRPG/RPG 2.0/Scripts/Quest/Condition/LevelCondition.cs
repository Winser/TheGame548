using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class LevelCondition : BaseCondition {
	public int minValue;
	public int maxValue;
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode minLevelNode;
	[System.NonSerialized]
	public StateNode maxLevelNode;
	
	public LevelCondition(Vector2 position):base(position){
		this.Position=position;
		this.Size= new Vector2(140,60);
		this.Title="Level Condition";
		
		minLevelNode=new StateNode("Min",this);
		maxLevelNode= new StateNode("Max",this);
		Nodes.Add(minLevelNode);
		Nodes.Add(maxLevelNode);
		
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,60);
		this.Title="Level Condition";
		
		minLevelNode=new StateNode("Min",this);
		maxLevelNode= new StateNode("Max",this);
		Nodes.Add(minLevelNode);
		Nodes.Add(maxLevelNode);
	}
	
	public override void Setup ()
	{
		base.Setup ();
		minValue=minLevelNode.GetInt();
		maxValue=maxLevelNode.GetInt();
	}
#endif
}