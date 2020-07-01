using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class ClassCondition : BaseCondition {
	public string cClass;
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode classNode;
	
	public ClassCondition(Vector2 position):base(position){
		this.Position=position;
		this.Size= new Vector2(140,40);
		this.Title="Class Condition";
		
		classNode= new StateNode("Class",this);
		Nodes.Add(classNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,40);
		this.Title="Class Condition";
		
		classNode= new StateNode("Class",this);
		Nodes.Add(classNode);
	}
	
	public override void Setup ()
	{
		base.Setup ();
		cClass=classNode.GetString();
	}
#endif
}