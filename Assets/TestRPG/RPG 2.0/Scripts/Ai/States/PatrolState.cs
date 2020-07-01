using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class PatrolState : BaseState {
	public float patrolSpeed;
	public float patrolRotation;
	public int patrolId;
	
	[System.NonSerialized]
	public WaypointPath path;
	[System.NonSerialized]
	public int curWaypoint=0;
	
	public override void HandleState (AiBehaviour ai)
	{
		base.HandleState (ai);
		if (curWaypoint >= path.waypoints.Count) {
			if (path.type.Equals (WaypointPathType.PingPong)) {
				WaypointManager.Instance.InvertWaypointPath(path);
			}
			curWaypoint = 0;
		}
		
		Vector3 point= path.waypoints[curWaypoint];
		if(Vector3.Distance(ai.transform.position,point)< 1f){
			curWaypoint++;
		}
		//ai.transform.LookAt(new Vector3(point.x,ai.transform.position.y,point.z));	
		ai.MoveAgent(point,patrolSpeed,patrolRotation);
	}
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode patrolSpeedNode;
	[System.NonSerialized]
	public StateNode patrolRotationNode;
	[System.NonSerialized]
	public StateNode patrolIdNode;
	
	public PatrolState(Vector2 position):base(position){
		this.Position=position;
		this.Size=new Vector2(140,120);
		this.Title="Patrol";
		this.patrolIdNode= new StateNode("Path Id",this,typeof(IntField));
		this.patrolSpeedNode= new StateNode("Speed",this,typeof(FloatField));
		this.patrolRotationNode=new StateNode("Rotation Speed",this,typeof(FloatField));
		
		this.Nodes.Add(patrolIdNode);
		this.Nodes.Add(patrolSpeedNode);
		this.Nodes.Add(patrolRotationNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init(position);
		this.Position=position;
		this.Size=new Vector2(140,120);
		this.Title="Patrol";
		patrolIdNode= new StateNode("Path Id",this,typeof(IntField));
		this.patrolSpeedNode= new StateNode("Speed",this,typeof(FloatField));
		this.patrolRotationNode=new StateNode("Rotation Speed",this,typeof(FloatField));
		
		this.Nodes.Add(patrolIdNode);
		this.Nodes.Add(patrolSpeedNode);
		this.Nodes.Add(patrolRotationNode);
		
	}
	
	public override void Init ()
	{
		base.Init ();
		patrolSpeed= patrolSpeedNode.GetFloat();
		patrolRotation=patrolRotationNode.GetFloat();
		patrolId= patrolIdNode.GetInt();
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		patrolSpeed=EditorGUILayout.FloatField("Patrol Speed", patrolSpeed);
		patrolRotation=EditorGUILayout.FloatField("Patrol Rotation", patrolRotation);
		patrolId=EditorGUILayout.IntField("Path Id", patrolId);
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
		patrolSpeed= patrolSpeedNode.GetFloat();
		patrolRotation=patrolRotationNode.GetFloat();
		patrolId= patrolIdNode.GetInt();
		
		x=Position.x;
		y=Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif
}
