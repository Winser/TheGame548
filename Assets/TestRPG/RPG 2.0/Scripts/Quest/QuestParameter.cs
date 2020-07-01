using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestParameter : MonoBehaviour
{
	public string parameter;
	private AiBehaviour ai;
	private bool killed;
		
	private void Start ()
	{
		ai = GetComponent<AiBehaviour> ();
		if (ai != null) {
			ai.onDeath += OnDeath;
			ai.onRespawn += OnRespawn;
		}
	}
	
	public void OnMouseUp ()
	{
		if(Vector3.Distance(GameManager.Player.transform.position,transform.position)>5){
			return;
		}
		foreach (Quest quest in QuestManager.Instance.quests) {
			if (quest.GetCurrentTask () is TalkTo && (quest.GetCurrentTask () as TalkTo).parameter.Equals (parameter)) {
				QuestManager.Instance.questTalkWindow.SetActive(true);
				QuestManager.Instance.lastQuest=quest;
				QuestManager.Instance.questTalkName.text=quest.questName;
				QuestManager.Instance.questTalkText.text=(quest.GetCurrentTask() as TalkTo).text;
			}
		}
	}
	
	private void OnDeath (float exp,List<string> loot)
	{
		if (!killed) {
			foreach (Quest quest in QuestManager.Instance.quests) {
				if (quest.GetCurrentTask () is Kill && (quest.GetCurrentTask () as Kill).parameter.Equals (parameter)) {
					(quest.GetCurrentTask () as Kill).killedNpc += 1;
					killed = true;
				}
			}
		}
	}
	
	private void OnRespawn ()
	{
		killed = false;
	}

}
