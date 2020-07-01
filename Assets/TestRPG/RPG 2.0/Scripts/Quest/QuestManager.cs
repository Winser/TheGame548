using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
	private static QuestManager instance;

	public static QuestManager Instance {
		get{ return instance;}
	}
	
	[HideInInspector]
	public List<Quest> quests;
	public GameObject activeQuest;
	public GameObject questBarWindow;
	public UITable questBar;
	public GameObject questStartWindow;
	public UILabel questStartName;
	public UILabel questStartText;
	public UILabel questStartGold;
	public UILabel questStartItemName;
	public UISprite questStartItemIcon;
	public GameObject questSayWindow;
	public UILabel questSayName;
	public UILabel questSayText;
	public GameObject questTalkWindow;
	public UILabel questTalkName;
	public UILabel questTalkText;
	public GameObject questLogWindow;
	public UITable questLogTable;
	public GameObject questLog;
	[HideInInspector]
	public Quest lastQuest;
	
	private void Awake ()
	{
		instance = this;
	}
	
	private void Start ()
	{
		quests = new List<Quest> ();
	}
	
	private void Update ()
	{
		if (Input.GetKeyDown (GameManager.InputSettings.quest)) {
			questLogWindow.gameObject.SetActive (!questLogWindow.activeSelf);
		}
		
		if (Input.GetKeyDown (GameManager.InputSettings.close)) {
			questBarWindow.SetActive (true);
			questStartWindow.SetActive (false);
			questSayWindow.SetActive (false);
			questTalkWindow.SetActive (false);
			questLogWindow.SetActive (false);
		}
	}
	
	private void AcceptStartQuest ()
	{
		if (!quests.Contains (lastQuest)) {
			quests.Add (lastQuest);
			AddToQuestBar(lastQuest);
			AddToQuestLog(lastQuest);
			MessageManager.Instance.AddMessage (GameManager.GameMessages.questAccepted.Replace ("@QuestName", lastQuest.questName));
		}
		QuestManager.instance.questStartWindow.SetActive (false);
		lastQuest.curTaskId = lastQuest.tasks [lastQuest.curTaskId].toTaskId;
		lastQuest.tasks [lastQuest.curTaskId].HandleTask (lastQuest);
		lastQuest.OnMouseUp ();
	}
	
	public void AddToQuestBar(Quest questToAdd){
		GameObject quest = NGUITools.AddChild (questBar.gameObject, activeQuest);
		ActiveQuest activeQuestScript = quest.GetComponent<ActiveQuest> ();
		activeQuestScript.questName.text = questToAdd.questName;
		questBar.Reposition ();
	}
	
	public void AddToQuestLog (Quest questToLog)
	{	
		GameObject newQuestLog = NGUITools.AddChild (questLogTable.gameObject, questLog);
		ActiveQuest activeQuestLog = newQuestLog.GetComponent<ActiveQuest> ();
		activeQuestLog.questName.text = questToLog.questName;
		activeQuestLog.questDescription.text=questToLog.GetStartQuest() != null ? questToLog.GetStartQuest().text:"";

	}
	
	private void ContinueOnSay ()
	{
		QuestManager.instance.questSayWindow.SetActive (false);
		lastQuest.curTaskId = lastQuest.tasks [lastQuest.curTaskId].toTaskId;
		lastQuest.tasks [lastQuest.curTaskId].HandleTask (lastQuest);
		lastQuest.OnMouseUp ();
	}
	
	private void ContinueOnTalk ()
	{
		QuestManager.instance.questTalkWindow.SetActive (false);
		lastQuest.curTaskId = lastQuest.tasks [lastQuest.curTaskId].toTaskId;
	}
	
	public void SetQuestBarDescription (Quest quest, string desc)
	{
		foreach (ActiveQuest q in questBar.GetComponentsInChildren<ActiveQuest>()) {
			if (q.questName.text.Equals (quest.questName)) {
				q.questDescription.text = desc;
			}
		}
	}
	
	public void RemoveQuestFromQuestBar (Quest quest)
	{
		GameObject removeQuest = null;
		foreach (ActiveQuest q in questBar.GetComponentsInChildren<ActiveQuest>()) {
			if (q.questName.text.Equals (quest.questName)) {
				removeQuest = q.gameObject;
			}
		}
		Destroy (removeQuest);
		questBar.Reposition ();
	}
	
	public ActiveQuest GetQuestLog (Quest quest)
	{
		questLogWindow.SetActive (true);
		foreach (ActiveQuest activeQuest in questLogTable.GetComponentsInChildren<ActiveQuest>()) {
			if (activeQuest.questName.text.Equals (quest.questName)) {
				questLogWindow.SetActive (false);
				return activeQuest;
			}
		}
		questLogWindow.SetActive (false);
		return null;
	}
}
