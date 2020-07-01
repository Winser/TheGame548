using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class TimedTransition : BaseTransition {
	public float time;
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode timeNode;
	
	public TimedTransition(Vector2 position):base (position){
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Timed";
		timeNode= new StateNode("Time",this,typeof(FloatField));
		this.Nodes.Add(timeNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Timed";
		timeNode= new StateNode("Time",this,typeof(FloatField));
		this.Nodes.Add(timeNode);
	}
	
	public override void Init ()
	{
		base.Init();
		time= timeNode.GetFloat();
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		time=EditorGUILayout.FloatField("Time", time);
	}
	
	public override void Setup ()
	{
		base.Setup ();
		time= timeNode.GetFloat();
	}
#endif
}
