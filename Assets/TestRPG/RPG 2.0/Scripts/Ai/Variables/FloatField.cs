using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FloatField : AbstractState {
	
#if UNITY_EDITOR
	public float floatVar;
	
	public FloatField(Vector2 position){
		this.Position= position;
		this.Size= new Vector2(140,40);
		this.Title= "Float";
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
		floatVar= EditorGUILayout.FloatField("Value",floatVar);
		GUILayout.EndArea();
		InputNode.DrawNode(new Vector2(AreaRect.x+2,AreaRect.y+2));
		
	}
#endif
}
