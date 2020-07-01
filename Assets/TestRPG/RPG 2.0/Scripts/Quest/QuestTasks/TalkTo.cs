using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

[System.Serializable]
public class TalkTo : BaseTaskState {
	public string text;
	
	public override void HandleTask (Quest quest)
	{
		base.HandleTask(quest);
		QuestManager.Instance.SetQuestBarDescription(quest,description);
		quest.questSign.SetActive(false);
	}
	
	public override void OnMouseUp (Quest quest)
	{
		
	}
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode textNode;
	
	public TalkTo(Vector2 position):base(position){
		this.Size= new Vector2(140,120);
		this.Title="Talk To";
		this.textNode= new StateNode("Text",this);
		this.Nodes.Add(textNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Size= new Vector2(140,120);
		this.Title="Talk To";
		this.textNode= new StateNode("Text",this);
		this.Nodes.Add(textNode);
	}
	
	public override void Save (FileStream fileStream, BinaryFormatter formatter)
	{
		description= descriptionNode.GetString();
		conditions=conditionNode.GetBaseCondition();
		foreach(BaseCondition condition in conditions){
			condition.Setup();
		}
		BaseTaskState toTask=toTaskNode.GetBaseTask();
		if(toTask != null){
			toTaskId=toTask.id;
		}
		
		parameter=parameterNode.GetString();
		text=textNode.GetText();
		
		x=Position.x;
		y= Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif

}
