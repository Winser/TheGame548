using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class AttackState : BaseState {
	public float damage;
	public string attributeName;
	
	[System.NonSerialized]
	private float time;
	
	public override void HandleState (AiBehaviour ai)
	{
		base.HandleState (ai);
		ai.StopAgent();
		if(ai.target != null){
			ai.transform.LookAt(new Vector3(ai.target.position.x,ai.transform.position.y,ai.target.position.z));
		}
		
		if(Time.time > time){
			if(ai.GetComponent<Animation>()[animation].time>ai.GetComponent<Animation>()[animation].length/2){
				ai.StartApplyPlayerDamage(attributeName,damage);
			}
			time=Time.time+ ai.GetComponent<Animation>()[animation].length;
		}
	}
	
	
	/*public float GetBeyondDistance(){
		foreach(BaseTransition tr in transitions){
			if(tr is BeyondDistanceTransition){
				return (tr as BeyondDistanceTransition).distance;
			}
		}
		return -1;
	}*/
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode damageNode;
	[System.NonSerialized]
	public StateNode attributeNameNode;
	
	public AttackState(Vector2 position):base(position){
		this.Position=position;
		this.Size=new Vector2(140,100);
		this.Title="Attack";
		this.damageNode= new StateNode("Damage",this,typeof(FloatField));
		this.attributeNameNode= new StateNode("Player Attribute",this,typeof(StringField));
		this.Nodes.Add(damageNode);
		this.Nodes.Add(attributeNameNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init(position);
		this.Position=position;
		this.Size=new Vector2(140,100);
		this.Title="Attack";
		this.damageNode= new StateNode("Damage",this,typeof(FloatField));
		this.attributeNameNode= new StateNode("Player Attribute",this,typeof(StringField));
		this.Nodes.Add(damageNode);
		this.Nodes.Add(attributeNameNode);
	}
	
	public override void Init ()
	{
		base.Init ();
		damage= damageNode.GetFloat();
		attributeName= attributeNameNode.GetString();
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		damage=EditorGUILayout.FloatField("Damage", damage);
		attributeName=EditorGUILayout.TextField("Attribute", attributeName);
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
		damage= damageNode.GetFloat();
		attributeName= attributeNameNode.GetString();
		x = Position.x;
		y = Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif
}
