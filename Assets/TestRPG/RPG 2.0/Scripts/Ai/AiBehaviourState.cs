using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

[System.Serializable]
public class AiBehaviourState:AbstractState
{
	public List<string> playerTags;
	public BaseState initialState;
	public List<BaseAttribute> attributes;
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode tagNode;
	[System.NonSerialized]
	public StateNode initialStateNode;
	[System.NonSerialized]
	public StateNode attributeNode;
	
	public AiBehaviourState(Vector2 position){
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Ai Behaviour";
		tagNode= new StateNode("Player Tag",this, typeof(StringField));
		attributeNode= new StateNode("Attribute",this,typeof(BaseAttribute));
		initialStateNode= new StateNode("Initial State",this);
		
		Nodes.Add(tagNode);
		Nodes.Add(attributeNode);
		Nodes.Add(initialStateNode);
	}
	
	public override void Init ()
	{
		playerTags=tagNode.GetStringList();
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
	
		GUILayout.BeginVertical("Box");
		if(GUILayout.Button("Add Player Tag")){
			if(playerTags == null){
				playerTags= new List<string>();
			}
			playerTags.Add("Player");
		}
		
		GUIStyle xButton = GUI.skin.button;
		xButton.margin.top = 2;
		int index=-1;
		if(playerTags != null && playerTags.Count>0) {
			for(int i=0; i< playerTags.Count;i++){
				GUILayout.BeginHorizontal();
				if(GUILayout.Button("X",xButton,GUILayout.Width(25))){
					index=i;
				}
				playerTags[i]=EditorGUILayout.TextField("Player Tag", playerTags[i]);
				GUILayout.EndHorizontal();
			}
		}
		
		if(index != -1){
			playerTags.RemoveAt(index);
		}
		GUILayout.EndVertical();
	}
	
	public override void Save (FileStream fileStream, BinaryFormatter formatter)
	{
		playerTags= tagNode.GetStringList();
		initialState= initialStateNode.GetBaseState();
		attributes= attributeNode.GetBaseAttribute();
		
		foreach(BaseAttribute attribute in attributes){
			attribute.Setup();
		}
			
		formatter.Serialize(fileStream,playerTags);
		formatter.Serialize(fileStream,initialState.id);
		formatter.Serialize(fileStream,attributes);
		
		x=Position.x;
		y= Position.y;
		formatter.Serialize(fileStream,x);
		formatter.Serialize(fileStream,y);
		
	}
#endif
}
