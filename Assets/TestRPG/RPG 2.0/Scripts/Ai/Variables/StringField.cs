using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StringField : AbstractState {
	
#if UNITY_EDITOR
	public string stringVar;
	
	public StringField(Vector2 position){
		this.Position= position;
		this.Size= new Vector2(140,40);
		this.Title= "String";
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
		stringVar= EditorGUILayout.TextField("Value",stringVar);
		GUILayout.EndArea();
		InputNode.DrawNode(new Vector2(AreaRect.x+2,AreaRect.y+2));
		
	}
#endif
}
