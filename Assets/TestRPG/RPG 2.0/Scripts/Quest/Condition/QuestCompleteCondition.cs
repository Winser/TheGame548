using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class QuestCompleteCondition : BaseCondition {
	public string questName;
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode questCompletedNode;
	
	public QuestCompleteCondition(Vector2 position):base(position){
		this.Position=position;
		this.Size= new Vector2(140,40);
		this.Title="Quest Complete Condition";
		questCompletedNode= new StateNode("Quest Name",this);
		Nodes.Add(questCompletedNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Position=position;
		this.Size= new Vector2(140,40);
		this.Title="Quest Complete Condition";
		questCompletedNode= new StateNode("Quest Name",this);
		Nodes.Add(questCompletedNode);
	}
	
	public override void Setup ()
	{
		base.Setup ();
		questName=questCompletedNode.GetString();
	}
#endif
}