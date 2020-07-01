using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

[System.Serializable]
public class Say : BaseTaskState {
	public string text;
	
	public override void HandleTask (Quest quest)
	{
		base.HandleTask (quest);
		quest.questSign.SetActive(true);
		QuestManager.Instance.SetQuestBarDescription(quest,description);
	}
	
	public override void OnMouseUp (Quest quest)
	{
		if(active && Vector3.Distance(GameManager.Player.transform.position,quest.transform.position)<5){
			QuestManager.Instance.questSayWindow.SetActive(true);
			QuestManager.Instance.lastQuest=quest;
			
			QuestManager.Instance.questSayName.text=quest.questName;
			QuestManager.Instance.questSayText.text=text;
		}
	}
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode textNode;
	
	public Say(Vector2 position):base(position){
		this.Size= new Vector2(140,100);
		this.Title="Say";
		this.textNode= new StateNode("Text",this);
		this.Nodes.Add(textNode);
		
		this.Nodes.Remove(parameterNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Size= new Vector2(140,100);
		this.Title="Say";
		this.textNode= new StateNode("Text",this);
		this.Nodes.Add(textNode);
		
		this.Nodes.Remove(parameterNode);
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
		parameter="";
		text=textNode.GetText();
		
		x=Position.x;
		y= Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif
}
