using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class ViewFieldTransition : BaseTransition {
	public float distance;
	public float viewField;
	public float agentHeight;
	
	#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode distanceNode;
	[System.NonSerialized]
	public StateNode viewFieldNode;
	[System.NonSerialized]
	public StateNode agentHeightNode;
	
	public ViewFieldTransition(Vector2 position):base (position){
		this.Position=position;
		this.Size= new Vector2(140,120);
		this.Title="View Field";
		distanceNode= new StateNode("Distance",this,typeof(FloatField));
		viewFieldNode=new StateNode("View Angle",this,typeof(FloatField));
		agentHeightNode= new StateNode("Agent Height",this,typeof(FloatField));
		this.Nodes.Add(distanceNode);
		this.Nodes.Add(viewFieldNode);
		this.Nodes.Add(agentHeightNode);
		
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Size= new Vector2(140,120);
		this.Title="View Field";
		distanceNode= new StateNode("Distance",this,typeof(FloatField));
		viewFieldNode=new StateNode("View Angle",this,typeof(FloatField));
		agentHeightNode= new StateNode("Agent Height",this,typeof(FloatField));
		this.Nodes.Add(distanceNode);
		this.Nodes.Add(viewFieldNode);
		this.Nodes.Add(agentHeightNode);
	}
	
	public override void Init ()
	{
		base.Init();
		distance= distanceNode.GetFloat();
		viewField= viewFieldNode.GetFloat();
		agentHeight= agentHeightNode.GetFloat();
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		distance=EditorGUILayout.FloatField("Distance",distance);
		viewField=EditorGUILayout.FloatField("View Angle",viewField);
		agentHeight=EditorGUILayout.FloatField("Agent Height", agentHeight);
	}
	
	public override void Setup ()
	{
		base.Setup ();
		distance= distanceNode.GetFloat();
		viewField= viewFieldNode.GetFloat();
		agentHeight= agentHeightNode.GetFloat();
	}
#endif
	
}
