using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

[System.Serializable]
public class TakeItem : BaseTaskState
{
	public string itemName;
	
	public override void HandleTask (Quest quest)
	{
		base.HandleTask (quest);
		
		if (GameManager.Player.Inventory.GetItem (itemName) != null) {
			canComplete=true;
			quest.questSign.SetActive (true);
		}else{
			quest.questSign.SetActive (false);
		}
		
		QuestManager.Instance.SetQuestBarDescription (quest, description);
	}
	
	
	public override void OnMouseUp (Quest quest)
	{
		if (canComplete && Vector3.Distance(GameManager.Player.transform.position,quest.transform.position)<5) {
			if (GameManager.Player.Inventory.RemoveItem (GameManager.Player.Inventory.GetItem (itemName))) {
				quest.curTaskId=quest.tasks[quest.curTaskId].toTaskId;
				quest.tasks[quest.curTaskId].HandleTask(quest);
				quest.OnMouseUp();
			}
		}
	}
	
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode itemNode;
	
	public TakeItem(Vector2 position):base(position){
		this.Size= new Vector2(140,100);
		this.Title="Take Item";
		itemNode= new StateNode("Item",this);
		this.Nodes.Add(itemNode);
		this.Nodes.Remove(parameterNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		this.Size= new Vector2(140,100);
		this.Title="Take Item";
		itemNode= new StateNode("Item",this);
		this.Nodes.Add(itemNode);
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
		BaseItem item= itemNode.GetItem();
		if(item != null){
			itemName=item.itemName;
			GameManager.ItemDatabase.AddItem(item);
		}
		
		x=Position.x;
		y= Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif
}
