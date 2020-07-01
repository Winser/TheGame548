using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class AttributeCondition : BaseCondition {
	public int minValue;
	public int maxValue;
	public string attribute;
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode minValueNode;
	[System.NonSerialized]
	public StateNode maxValueNode;
	[System.NonSerialized]
	public StateNode attributeNode;
	
	public AttributeCondition(Vector2 position):base(position){
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Attribute Condition";
		
		attributeNode= new StateNode("Attribute",this);
		minValueNode= new StateNode("Min",this);
		maxValueNode= new StateNode("Max",this);
		
		Nodes.Add(attributeNode);
		Nodes.Add(minValueNode);
		Nodes.Add(maxValueNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Attribute Condition";
		
		attributeNode= new StateNode("Attribute",this);
		minValueNode= new StateNode("Min",this);
		maxValueNode= new StateNode("Max",this);
		
		Nodes.Add(attributeNode);
		Nodes.Add(minValueNode);
		Nodes.Add(maxValueNode);
	}
		
	public override void Setup ()
	{
		base.Setup ();
		attribute= attributeNode.GetString();
		minValue=minValueNode.GetInt();
		maxValue=maxValueNode.GetInt();
	}
#endif
}