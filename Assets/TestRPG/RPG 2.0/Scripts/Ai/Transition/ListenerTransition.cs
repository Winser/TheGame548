using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class ListenerTransition : BaseTransition {
	public string listenTo;
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode listenToNode;
	
	public ListenerTransition(Vector2 position):base (position){
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Listener";
		listenToNode= new StateNode("Listen To",this,typeof(StringField));
		this.Nodes.Add(listenToNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Listener";
		listenToNode= new StateNode("Listen to",this,typeof(StringField));
		this.Nodes.Add(listenToNode);
	}
	
	public override void Init ()
	{
		base.Init();
		listenTo= listenToNode.GetString();
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		listenTo=EditorGUILayout.TextField("Listen to",listenTo);
	}
	
	public override void Setup ()
	{
		base.Setup ();
		listenTo= listenToNode.GetString();
	}
#endif
}
