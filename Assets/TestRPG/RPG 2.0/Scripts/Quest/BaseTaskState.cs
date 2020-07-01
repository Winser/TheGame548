using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

[System.Serializable]
public class BaseTaskState : AbstractState
{
	public int id;
	public string description;
	public List<BaseCondition> conditions;
	public int toTaskId;
	public string parameter;
	[System.NonSerialized]
	public bool canComplete;
	[System.NonSerialized]
	public bool active;
	
	public virtual void HandleTask (Quest quest)
	{
		if(!CheckCondition()){
			quest.questSign.SetActive(false);
			return;
		}
		active=true;
	}
	
	public virtual void OnMouseUp( Quest quest){
		
	}
	
	private bool CheckCondition ()
	{
		if(GameManager.Player == null){
			return false;
		}
		
		foreach (BaseCondition condition in conditions) {
			if (condition is LevelCondition) {
				LevelCondition levelCondition = condition as LevelCondition;
				if (!IsWithin (GameManager.Player.Level, levelCondition.minValue, levelCondition.maxValue)) {
					//Debug.Log ("Level Condition " + PlayerManager.Instance.PlayerLevel);
					return false;
				}
			}
			
			if (condition is AttributeCondition) {
				AttributeCondition attrCondition = condition as AttributeCondition;
				if (!IsWithin (GameManager.Player.GetAttribute (attrCondition.attribute).BaseValue, attrCondition.minValue, attrCondition.maxValue)) {
					Debug.Log ("Attribute Condition");
					return false;
				}
			}
			
			if (condition is ClassCondition) {
				ClassCondition classCondition = condition as ClassCondition;
				if (!GameManager.Player.Character.characterClass.Equals (classCondition.cClass)) {
					Debug.Log ("Class Condition");
					return false;
				}
			}
		}
		
		return true;
	}
	
	public static bool IsWithin (float value, float minimum, float maximum)
	{
		return value >= minimum && value <= maximum;
	}
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode descriptionNode;
	[System.NonSerialized]
	public StateNode conditionNode;
	[System.NonSerialized]
	public StateNode toTaskNode;
	[System.NonSerialized]
	public StateNode parameterNode;
		
	public BaseTaskState(Vector2 position){
		this.Position=position;
		this.Size= new Vector2(140,100);
		this.Title="Base Task";
		descriptionNode= new StateNode("Description",this);
		conditionNode= new StateNode("Condition",this);
		toTaskNode= new StateNode("To Task",this);
		parameterNode= new StateNode("Parameter",this);
		
		Nodes.Add(descriptionNode);
		Nodes.Add(conditionNode);
		Nodes.Add(toTaskNode);
		Nodes.Add(parameterNode);
	}
	
	public override void Init (Vector2 position)
	{
		base.Init (position);
		
		this.Position=position;
		this.Size= new Vector2(140,100);
		this.Title="Base Task";
		descriptionNode= new StateNode("Description",this);
		conditionNode= new StateNode("Condition",this);
		toTaskNode= new StateNode("To Task",this);
		parameterNode= new StateNode("Parameter",this);
		
		Nodes.Add(descriptionNode);
		Nodes.Add(conditionNode);
		Nodes.Add(toTaskNode);
		Nodes.Add(parameterNode);
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
		
		x=Position.x;
		y= Position.y;
		formatter.Serialize(fileStream,this);
	}
#endif
}
