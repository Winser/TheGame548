using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class HelpState : BaseState {
	public float radius;
	public string shout;
	[System.NonSerialized]
	private float shoutTime;
	
	public override void HandleState (AiBehaviour ai)
	{
		base.HandleState (ai);
		if(Time.time > shoutTime){
		Collider[] hitColliders = Physics.OverlapSphere(ai.transform.position, radius);
        foreach(Collider collider in hitColliders) {
			if(collider.transform != ai.transform){
           	 	collider.SendMessage("ListenTo",shout,SendMessageOptions.DontRequireReceiver);
			}
        }
			shoutTime=Time.time+5;
		}
		
	}
	
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode radiusNode;
	[System.NonSerialized]
	public StateNode shoutNode;
	
	public HelpState(Vector2 position):base(position){
		this.Position=position;
		this.Size= new Vector2(140,100);
		this.Title="Help";
		this.radiusNode= new StateNode("Radius",this,typeof(FloatField));
		this.shoutNode= new StateNode("Shout",this,typeof(StringField));
		this.Nodes.Add(radiusNode);
		this.Nodes.Add(shoutNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,100);
		this.Title="Help";
		this.radiusNode= new StateNode("Radius",this,typeof(FloatField));
		this.shoutNode= new StateNode("Shout",this,typeof(StringField));
		this.Nodes.Add(radiusNode);
		this.Nodes.Add(shoutNode);
	}
	
	public override void Init ()
	{
		base.Init ();
		radius= radiusNode.GetFloat();
		shout= shoutNode.GetString();
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		radius=EditorGUILayout.FloatField("Radius", radius);
		shout=EditorGUILayout.TextField("Shout", shout);
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
		
		radius= radiusNode.GetFloat();
		shout= shoutNode.GetString();
		
		x = Position.x;
		y = Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif
}
