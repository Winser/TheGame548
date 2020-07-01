using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TalentField : AbstractState {
	
#if UNITY_EDITOR
	public BaseTalent talent;
	
	public TalentField(Vector2 position){
		this.Position= position;
		this.Size= new Vector2(140,40);
		this.Title= "Talent";
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
		talent= (BaseTalent)EditorGUILayout.ObjectField("Talent",talent,typeof(BaseTalent),false);
		GUILayout.EndArea();
		InputNode.DrawNode(new Vector2(AreaRect.x+2,AreaRect.y+2));
		
	}
#endif
}