using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class WalkState : BaseState {
	public float walkSpeed;
	public float walkRadius;
	public float walkRotation;
	[System.NonSerialized]
	public Vector3 nextPoint;
	
	public override void HandleState (AiBehaviour ai)
	{
		base.HandleState (ai);
		
		if(Vector3.Distance(ai.transform.position,nextPoint)< 1){
			 nextPoint=UnityTools.RandomPointInArea(ai.startPosition,walkRadius,~(1<<ai.gameObject.layer));
		}
		Debug.DrawLine(ai.transform.position,nextPoint);
		ai.MoveAgent(nextPoint,walkSpeed,walkRotation);
	}
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode walkSpeedNode;
	[System.NonSerialized]
	public StateNode walkRadiusNode;
	[System.NonSerialized]
	public StateNode walkRotationNode;
	
	public WalkState(Vector2 position):base(position){
		this.Position=position;
		this.Size=new Vector2(140,120);
		this.Title="Walk";
		this.walkSpeedNode= new StateNode("Speed",this,typeof(FloatField));
		this.walkRadiusNode= new StateNode("Walk Radius",this,typeof(FloatField));
		this.walkRotationNode= new StateNode("Rotation Speed",this,typeof(FloatField));
		
		this.Nodes.Add(walkSpeedNode);
		this.Nodes.Add(walkRotationNode);
		this.Nodes.Add(walkRadiusNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init(position);
		this.Position=position;
		this.Size=new Vector2(140,120);
		this.Title="Walk";
		this.walkSpeedNode= new StateNode("Speed",this,typeof(FloatField));
		this.walkRadiusNode= new StateNode("Walk Radius",this,typeof(FloatField));
		this.walkRotationNode= new StateNode("Rotation Speed",this,typeof(FloatField));
		
		this.Nodes.Add(walkSpeedNode);
		this.Nodes.Add(walkRotationNode);
		this.Nodes.Add(walkRadiusNode);
		
	}
	
	public override void Init ()
	{
		base.Init ();
		walkSpeed= walkSpeedNode.GetFloat();
		walkRadius= walkRadiusNode.GetFloat();
		walkRotation=walkRotationNode.GetFloat();
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		walkSpeed=EditorGUILayout.FloatField("Walk Speed",walkSpeed);
		walkRadius=EditorGUILayout.FloatField("Walk Radius",walkRadius);
		walkRotation=EditorGUILayout.FloatField("Walk Rotation",walkRotation);
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
		walkSpeed= walkSpeedNode.GetFloat();
		walkRadius= walkRadiusNode.GetFloat();
		walkRotation=walkRotationNode.GetFloat();
		
		x=Position.x;
		y=Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif
}
