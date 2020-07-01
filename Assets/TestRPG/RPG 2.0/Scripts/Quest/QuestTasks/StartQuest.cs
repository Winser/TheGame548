using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

[System.Serializable]
public class StartQuest : BaseTaskState {
	public string text;
	public string rewardItemName;
	public int gold;
	public float exp;
	
	public override void HandleTask (Quest quest)
	{
		base.HandleTask (quest);
		quest.questSign.SetActive(true);
	}
	
	public override void OnMouseUp (Quest quest)
	{
		if(active && Vector3.Distance(GameManager.Player.transform.position,quest.transform.position)<5){
			
			QuestManager.Instance.questStartWindow.SetActive(true);
			QuestManager.Instance.lastQuest=quest;
			
			QuestManager.Instance.questStartName.text=quest.questName;
			QuestManager.Instance.questStartText.text=text;
			QuestManager.Instance.questStartItemName.text=rewardItemName;
			QuestManager.Instance.questStartItemIcon.spriteName=GameManager.ItemDatabase.GetItem(rewardItemName).icon;
			QuestManager.Instance.questStartGold.text=gold.ToString();
		}
	}
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode textNode;
	[System.NonSerialized]
	public StateNode rewardItemNode;
	[System.NonSerialized]
	public StateNode goldNode;
	[System.NonSerialized]
	public StateNode expNode;
	
	public StartQuest(Vector2 position):base(position){
		this.Size= new Vector2(140,140);
		this.Title="Start Quest";
		this.textNode= new StateNode("Text",this);
		this.rewardItemNode= new StateNode("Reward Item",this);
		this.goldNode= new StateNode("Gold",this);
		this.expNode= new StateNode("Exp",this);
		
		this.Nodes.Add(textNode);
		this.Nodes.Add(rewardItemNode);
		this.Nodes.Add(goldNode);
		this.Nodes.Add(expNode);
		
		this.Nodes.Remove(parameterNode);
		this.Nodes.Remove(descriptionNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Size= new Vector2(140,140);
		this.Title="Start Quest";
		this.textNode= new StateNode("Text",this);
		this.rewardItemNode= new StateNode("Reward Item",this);
		this.goldNode= new StateNode("Gold",this);
		this.expNode= new StateNode("Exp",this);
		
		this.Nodes.Add(textNode);
		this.Nodes.Add(rewardItemNode);
		this.Nodes.Add(goldNode);
		this.Nodes.Add(expNode);
		
		this.Nodes.Remove(parameterNode);
		this.Nodes.Remove(descriptionNode);
	}
	
	public override void Save (FileStream fileStream, BinaryFormatter formatter)
	{
		description= "";
		parameter="";
		
		conditions=conditionNode.GetBaseCondition();
		foreach(BaseCondition condition in conditions){
			condition.Setup();
		}
		BaseTaskState toTask=toTaskNode.GetBaseTask();
		if(toTask != null){
			toTaskId=toTask.id;
		}
		
		x=Position.x;
		y= Position.y;
		
		text=textNode.GetText();
		
		BaseItem item=rewardItemNode.GetItem();
		if(item != null){
			rewardItemName=item.itemName;
			GameManager.ItemDatabase.AddItem(item);
		}
		
		gold=goldNode.GetInt();
		exp=expNode.GetFloat();
		
		formatter.Serialize(fileStream,this);
	}
#endif
}
