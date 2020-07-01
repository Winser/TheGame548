using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class BaseComparerTransition : BaseTransition {
	public float compareValue;
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode compareValueNode;
	
	public BaseComparerTransition(Vector2 position):base (position){
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Base Comparer";
		this.compareValueNode= new StateNode("Value",this,typeof(FloatField));
		this.Nodes.Add(compareValueNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Base Comparer";
		this.compareValueNode= new StateNode("Value",this,typeof(FloatField));
		this.Nodes.Add(compareValueNode);
	}
	
	public override void Init ()
	{
		base.Init();
		compareValue= compareValueNode.GetFloat();
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		compareValue=EditorGUILayout.FloatField("Value",compareValue);
	}
	
	public override void Setup ()
	{
		base.Setup ();
		compareValue= compareValueNode.GetFloat();
	}
#endif
	
}
