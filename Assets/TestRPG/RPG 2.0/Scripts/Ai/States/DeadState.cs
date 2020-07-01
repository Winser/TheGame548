using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class DeadState : BaseState
{
	public float exp;
	public List<string> loot;
	
	public override void HandleState (AiBehaviour ai)
	{
		base.HandleState (ai);
		ai.StopAgent ();
		if (ai.onDeath != null && PhotonNetwork.isMasterClient) {
			ai.onDeath (exp,loot);
		}
	}
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode expNode;
	[System.NonSerialized]
	public StateNode lootNode;
	
	public DeadState(Vector2 position):base(position){
		this.Position=position;
		this.Size= new Vector2(140,100);
		this.Title="Dead";
		expNode= new StateNode("Exp",this,typeof(FloatField));
		lootNode= new StateNode("Loot", this,typeof(ItemTableField));
		this.Nodes.Add(expNode);
		this.Nodes.Add(lootNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,100);
		this.Title="Dead";
		expNode= new StateNode("Exp",this, typeof(FloatField));
		lootNode= new StateNode("Loot",this, typeof(ItemTableField));
		this.Nodes.Add(expNode);
		this.Nodes.Add(lootNode);
	}
	
	public override void Init ()
	{
		base.Init ();
		exp=expNode.GetFloat();
		ItemTable table= lootNode.GetItemTable();
		
		loot= new List<string>();
		if(table != null){
			foreach(BaseItem item in table.items){
				loot.Add(item.itemName);
				GameManager.ItemDatabase.AddItem(item);
			}
		}
		
		foreach(string s in lootNode.GetStringList()){
			loot.Add(s);
		}
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		exp=EditorGUILayout.FloatField("Exp", exp);
		
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
		exp=expNode.GetFloat();
		ItemTable table= lootNode.GetItemTable();
		
		loot= new List<string>();
		if(table != null){
			foreach(BaseItem item in table.items){
				loot.Add(item.itemName);
				GameManager.ItemDatabase.AddItem(item);
			}
		}
		
		foreach(string s in lootNode.GetStringList()){
			loot.Add(s);
		}
		
		
		x = Position.x;
		y = Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif
}
