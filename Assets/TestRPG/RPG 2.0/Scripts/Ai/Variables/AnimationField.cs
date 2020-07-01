using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AnimationField : AbstractState {
	
#if UNITY_EDITOR
	public AnimationClip anim;
	
	public AnimationField(Vector2 position){
		this.Position= position;
		this.Size= new Vector2(140,40);
		this.Title= "Animation";
	}
	
	public override void DrawState ()
	{
		Color color = GUI.color;
		if(AbstractState.LastSelection != null && AbstractState.LastSelection.Equals(this)){
			GUI.color=Color.green;
		}
		GUILayout.BeginArea(AreaRect,Title,"window");
		GUI.color=color;
		foreach(StateNode node in Nodes){
			node.DrawNode(AreaRect);
		}
		anim= (AnimationClip)EditorGUILayout.ObjectField("Animation",anim,typeof(AnimationClip),false);
		GUILayout.EndArea();
		InputNode.DrawNode(new Vector2(AreaRect.x+2,AreaRect.y+2));
		
	}
#endif
}
