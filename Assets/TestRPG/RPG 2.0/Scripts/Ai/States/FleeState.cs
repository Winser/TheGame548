using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class FleeState : BaseState {
	public float fleeSpeed;
	
	public override void HandleState (AiBehaviour ai)
	{
		base.HandleState (ai);
		if(ai.target != null){
			Vector3 fleePosition= ai.transform.position + ai.target.forward*5; 
			ai.MoveAgent(fleePosition,fleeSpeed,5);
		}
	}
	
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode fleeSpeedNode;
	
	public override void Init (Vector2 position)
	{
		base.Init(position);
		this.Position=position;
		this.Size=new Vector2(140,80);
		this.Title="Flee";
		fleeSpeedNode= new StateNode("Speed",this,typeof(FloatField));
		this.Nodes.Add(fleeSpeedNode);
	}
	
	public FleeState(Vector2 position):base(position){
		this.Position=position;
		this.Size=new Vector2(140,80);
		this.Title="Flee";
		fleeSpeedNode= new StateNode("Speed",this,typeof(FloatField));
		this.Nodes.Add(fleeSpeedNode);
	}
	
	public override void Init ()
	{
		base.Init();
		fleeSpeed= fleeSpeedNode.GetFloat();
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		fleeSpeed=EditorGUILayout.FloatField("Flee Speed", fleeSpeed);
	}
	
	public override void Save (System.IO.FileStream fileStream, System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter)
	{
		AnimationClip animationClip= animationNode.GetAnimationClip();
		if(animationClip != null){
			animation= animationNode.GetAnimationClip().name;
		}else{
			animation=animationNode.GetString();
		}
		transitions= transitionNode.GetBaseTransition();
		foreach(BaseTransition transition in transitions){
			transition.Setup();
		}
		fleeSpeed= fleeSpeedNode.GetFloat();
		x = Position.x;
		y = Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif
}
