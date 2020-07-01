using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class FollowState : BaseState {
	public float followSpeed;
	public float followRotationSpeed;
	
	public override void HandleState (AiBehaviour ai)
	{
		base.HandleState(ai);	
		if(ai.target != null){
			ai.MoveAgent(ai.target.position,followSpeed,followRotationSpeed);
		}
		
	}
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode followSpeedNode;
	[System.NonSerialized]
	public StateNode followRotationSpeedNode;
	
	public override void Init (Vector2 position)
	{
		base.Init(position);
		this.Position=position;
		this.Size=new Vector2(140,100);
		this.Title="Follow";
		followSpeedNode= new StateNode("Speed",this,typeof(FloatField));
		followRotationSpeedNode= new StateNode("Rotation Speed",this,typeof(FloatField));
		this.Nodes.Add(followSpeedNode);
		this.Nodes.Add(followRotationSpeedNode);
	}
	
	public FollowState(Vector2 position):base(position){
		this.Position=position;
		this.Size=new Vector2(140,100);
		this.Title="Follow";
		followSpeedNode= new StateNode("Speed",this,typeof(FloatField));
		followRotationSpeedNode= new StateNode("Rotation Speed",this,typeof(FloatField));
		this.Nodes.Add(followSpeedNode);
		this.Nodes.Add(followRotationSpeedNode);
	}
	
	public override void Init ()
	{
		base.Init ();
		followSpeed= followSpeedNode.GetFloat();
		followRotationSpeed= followRotationSpeedNode.GetFloat();
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		followSpeed=EditorGUILayout.FloatField("Follow Speed", followSpeed);
		followRotationSpeed=EditorGUILayout.FloatField("Follow Rotation", followRotationSpeed);
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
		followSpeed= followSpeedNode.GetFloat();
		followRotationSpeed= followRotationSpeedNode.GetFloat();
		x = Position.x;
		y = Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif
}
