using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class SayState : BaseState {
	public List<string> sayText;
	
	public override void HandleState (AiBehaviour ai)
	{
		base.HandleState (ai);
		ai.StopAgent();
		if(sayText.Count>0){
			string toSay= sayText[Random.Range(0,sayText.Count)];
			if (ai.onSay != null) {
				ai.onSay (toSay, ai.GetComponent<Animation>()[animation].length);
			}
		}
	
	}
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode sayTextNode;
	
	public SayState(Vector2 position):base(position){
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Say";
		sayTextNode= new StateNode("Text", this,typeof(TextField));
		this.Nodes.Add(sayTextNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,80);
		this.Title="Say";
		sayTextNode= new StateNode("Text", this,typeof(TextField));
		this.Nodes.Add(sayTextNode);
	}
	
	public override void Init ()
	{
		base.Init ();
		sayText= sayTextNode.GetStringList();
		
	}
	
	public override void OnGUI ()
	{
		GUILayout.BeginHorizontal (EditorStyles.toolbar);
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Apply",EditorStyles.toolbarButton,GUILayout.Width(60))){
			
		}
		GUILayout.EndHorizontal ();
		GUILayout.BeginVertical("Box");
		base.OnGUI ();
		
		if(GUILayout.Button("Add Text")){
			if(sayText == null){
				sayText= new List<string>();
			}
			sayText.Add("");
		}
		
		GUIStyle xButton = GUI.skin.button;
		xButton.margin.top = 2;
		int index=-1;
		if(sayText != null && sayText.Count>0) {
			for(int i=0; i< sayText.Count;i++){
				GUILayout.BeginHorizontal();
				if(GUILayout.Button("X",xButton,GUILayout.Width(25))){
					index=i;
				}
				sayText[i]=EditorGUILayout.TextField("Text", sayText[i]);
				GUILayout.EndHorizontal();
			}
		}
		
		if(index != -1){
			sayText.RemoveAt(index);
		}
		GUILayout.EndVertical();
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
	
		sayText= sayTextNode.GetStringList();
		
		
		x = Position.x;
		y = Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif

}
