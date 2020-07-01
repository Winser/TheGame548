using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

[System.Serializable]
public class Kill : BaseTaskState
{
	public int amount;
	[System.NonSerialized]
	public int killedNpc;
	
	public override void HandleTask (Quest quest)
	{
		base.HandleTask (quest);
		if (killedNpc >= amount) {
			quest.questSign.SetActive (true);
			canComplete = true;
			quest.curTaskId = quest.tasks [quest.curTaskId].toTaskId;
		} else {
			quest.questSign.SetActive (false);
			QuestManager.Instance.SetQuestBarDescription (quest, description);
		}
	}
	
	public override void OnMouseUp (Quest quest)
	{
		if (canComplete) {
			quest.curTaskId = quest.tasks [quest.curTaskId].toTaskId;
			quest.tasks [quest.curTaskId].HandleTask (quest);
			quest.OnMouseUp ();
		}
	}
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode amountNode;

	public Kill(Vector2 position):base(position){
		this.Size= new Vector2(140,120);
		this.Title="Kill";
		amountNode= new StateNode("Amount",this);
		this.Nodes.Add(amountNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Size= new Vector2(140,120);
		this.Title="Kill";
		amountNode= new StateNode("Amount",this);
		this.Nodes.Add(amountNode);
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
		amount=amountNode.GetInt();
		
		x=Position.x;
		y= Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif
}
