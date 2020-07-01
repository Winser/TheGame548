using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

[System.Serializable]
public class EndQuest : BaseTaskState
{
	
	public override void HandleTask (Quest quest)
	{
		base.HandleTask (quest);
		quest.questSign.SetActive(true);
		QuestManager.Instance.SetQuestBarDescription(quest,description);
		if(Vector3.Distance(GameManager.Player.transform.position,quest.transform.position)>5){
			return;
		}
	}
	
	public override void OnMouseUp (Quest quest)
	{
		quest.completed = true;
		quest.questSign.SetActive (false);
		QuestManager.Instance.RemoveQuestFromQuestBar (quest);
		
		foreach (KeyValuePair<int, BaseTaskState> kvp in quest.tasks) {
			if (kvp.Value is StartQuest) {
				StartQuest startQuest = kvp.Value as StartQuest;
				GameManager.Player.Inventory.AddItem (GameManager.ItemDatabase.GetItem (startQuest.rewardItemName));
				GameManager.Player.Gold += startQuest.gold;
				GameManager.Player.Exp.ApplyExp(startQuest.exp);
				
				ActiveQuest log=QuestManager.Instance.GetQuestLog(quest);
				MessageManager.Instance.AddMessage(GameManager.GameMessages.questCompleted.Replace("@QuestName",quest.questName));
				if(log != null){
					log.questName.text=log.questName.text+ "  [66FA33][Completed][-]";
				}else{
					Debug.Log("is null");
				}
			}
		}
	}
	
#if UNITY_EDITOR
	public EndQuest(Vector2 position):base(position){
		this.Size= new Vector2(140,60);
		this.Title="End Quest";
		Nodes.Remove(parameterNode);
		Nodes.Remove(toTaskNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Size= new Vector2(140,60);
		this.Title="End Quest";
		Nodes.Remove(parameterNode);
		Nodes.Remove(toTaskNode);
	}
	
	public override void Save (FileStream fileStream, BinaryFormatter formatter)
	{
		description= descriptionNode.GetString();
		conditions=conditionNode.GetBaseCondition();
		parameter="";
		foreach(BaseCondition condition in conditions){
			condition.Setup();
		}

		x=Position.x;
		y= Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif
}
