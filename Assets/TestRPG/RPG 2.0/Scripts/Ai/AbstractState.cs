using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
#endif

[System.Serializable]
public abstract class AbstractState {
	public float x;
	public float y;
	#if UNITY_EDITOR

	[System.NonSerialized]
	private Rect areaRect;
	[System.NonSerialized]
	private Vector2 position;
	[System.NonSerialized]
	private Vector2 size;
	[System.NonSerialized]
	private List<StateNode> nodes;
	[System.NonSerialized]
	private static AbstractState selection;
	[System.NonSerialized]
	private static AbstractState lastSelection;
	[System.NonSerialized]
	private StateNode input;
	[System.NonSerialized]
	private string title;
	
	public virtual void Init(Vector2 position){
		
	}
	
	public virtual void Init(){
		
	}
	
	public StateNode InputNode{
		get{
			if(input == null){
				input= new StateNode("",this);
			}
			return input;
		}
		set{input=value;}
	}
	
	public string Title{
		get{return title;}
		set{title=value;}
	}
	
	public Vector2 Position{
		get{return position;}
		set{position=value;
			areaRect = new Rect (
                position.x - size.x * 0.5f,
                position.y - size.y * 0.5f,
                size.x,
                size.y
            );
		}
	}
	
	public Vector2 Size {
		get { return size; }
		set {
			size = value;
			areaRect = new Rect (
                position.x - size.x * 0.5f,
                position.y - size.y * 0.5f,
                size.x,
                size.y
            );
		}
	}
	
	public Rect AreaRect{
		get{return areaRect;}
	}
	
	public List<StateNode> Nodes{
		get{
			if(nodes == null){
				nodes= new List<StateNode>();
			}
			return nodes;
		}
	}
	
	public static AbstractState Selection{
		get{return selection;}
		set{
			selection= value;
			if(selection != null){
				lastSelection=selection;
			}
		}
	}
	
	public static AbstractState LastSelection{
		get{return lastSelection;}
		set{lastSelection= value;}
	}
	
	public virtual void DrawState(){
		Color color = GUI.color;
		if(AbstractState.LastSelection != null && AbstractState.LastSelection.Equals(this)){
			GUI.color=Color.green;
		}
		GUILayout.BeginArea(AreaRect,Title,"window");
		GUI.color=color;
		foreach(StateNode node in Nodes){
			node.DrawNode(areaRect);
		}
		GUILayout.EndArea();
		InputNode.DrawNode(new Vector2(areaRect.x+2,areaRect.y+2));
	}
	
	public void HandleEvents(){
		switch (Event.current.type) {
		case EventType.MouseDown:
			if (AreaRect.Contains (Event.current.mousePosition)) {
				Selection= this;
				StateNode.Selection=null;
				GUIUtility.keyboardControl = 0;
				Event.current.Use ();
			}
			break;
		}
	}
	
	public virtual void Save(FileStream fileStream, BinaryFormatter formatter){
		
	}
	
	public virtual void OnSceneGUI(){
		
	}
	
	public virtual void OnGUI(){
		GUILayout.BeginHorizontal (EditorStyles.toolbar);
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Apply",EditorStyles.toolbarButton,GUILayout.Width(60))){
			
		}
		GUILayout.EndHorizontal ();
	}
#endif
}
