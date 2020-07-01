using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

[System.Serializable]
public class QuestState : AbstractState {
	public string questName;
	public BaseTaskState initialTask;
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode questNameNode;

	[System.NonSerialized]
	public StateNode initialTaskNode;
		
	public QuestState(Vector2 position){
		this.Position=position;
		this.Size= new Vector2(140,60);
		this.Title="Quest";
		
		questNameNode= new StateNode("Name",this);
		initialTaskNode= new StateNode("Initial Task",this);
		
		Nodes.Add(questNameNode);
		Nodes.Add(initialTaskNode);
	}
	
	public override void Save (FileStream fileStream, BinaryFormatter formatter)
	{
		questName= questNameNode.GetString();
		initialTask= initialTaskNode.GetBaseTask();
	
		formatter.Serialize(fileStream,questName);
		formatter.Serialize(fileStream,initialTask.id);
		
		x=Position.x;
		y= Position.y;
		formatter.Serialize(fileStream,x);
		formatter.Serialize(fileStream,y);
		
	}
#endif
}
