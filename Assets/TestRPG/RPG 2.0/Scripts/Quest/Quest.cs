using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Quest : MonoBehaviour
{
	public TextAsset file;
	public GameObject questSign;
	[HideInInspector]
	public string questName;
	[HideInInspector]
	public Dictionary<int,BaseTaskState> tasks;
	[HideInInspector]
	public int curTaskId;
	[HideInInspector]
	public bool completed;
	
	
	private void Awake ()
	{
		MemoryStream stream = new MemoryStream (file.bytes);
		BinaryFormatter formatter = new BinaryFormatter ();
		formatter.Binder = new VersionDeserializationBinder ();
		
		
		questName = (string)formatter.Deserialize (stream);
		curTaskId = (int)formatter.Deserialize (stream);
		#pragma warning disable 0219
		float x = (float)formatter.Deserialize (stream);//unused
		#pragma warning disable 0219
		float y = (float)formatter.Deserialize (stream);//unused
		
		int cnt = (int)formatter.Deserialize (stream);
		tasks= new Dictionary<int, BaseTaskState>();
		for (int i=0; i< cnt; i++) {
			BaseTaskState state = (BaseTaskState)formatter.Deserialize (stream);
			tasks.Add (state.id, state);
		}
	}
	
	private void Update(){
		if(!completed){
			tasks[curTaskId].HandleTask(this);
		}
	}
	
	public void OnMouseUp(){
		if(!completed){
			tasks[curTaskId].OnMouseUp(this);
		}
	}
	
	public BaseTaskState GetCurrentTask(){
		return tasks[curTaskId];
	}
	
	public StartQuest GetStartQuest(){
		foreach(KeyValuePair<int, BaseTaskState> kvp in tasks){
			if(kvp.Value is StartQuest){
				return (StartQuest)kvp.Value;
			}
		}
		return null;
	}
}
