using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class BaseTransition : AbstractState {
	public int toStateId;
	public float priority;
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode priorityNode;
	[System.NonSerialized]
	public StateNode toStateNode;
	
	public BaseTransition(Vector2 position){
		this.Position=position;
		this.Size= new Vector2(140,60);
		this.Title="Base Transition";
		priorityNode= new StateNode("Priority",this,typeof(FloatField));
		toStateNode= new StateNode("To State",this);
		this.Nodes.Add(priorityNode);
		this.Nodes.Add(toStateNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,60);
		this.Title="Base Transition";
		priorityNode= new StateNode("Priority",this,typeof(FloatField));
		toStateNode= new StateNode("To State",this);
		this.Nodes.Add(toStateNode);
		this.Nodes.Add(priorityNode);
	}
	
	public override void Init ()
	{
		base.Init ();
		priority=priorityNode.GetFloat();
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		priority=EditorGUILayout.FloatField("Priority",priority);
	}
	
	public virtual void Setup(){
		priority=priorityNode.GetFloat();
		toStateId=toStateNode.GetBaseState().id;
		x = Position.x;
		y = Position.y;
	}
#endif
}
