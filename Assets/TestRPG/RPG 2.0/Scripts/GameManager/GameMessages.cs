using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Base class to store ingame messages. 
/// </summary>
[System.Serializable]
public class GameMessages : ScriptableObject {
	public string welcome="Welcome to the [66FA33]RPG Kit 2.0[-] demo!";
	public string fullInventory="Your inventory is full!";
	public string canNotEquip="You can not wear this.";
	public string farAway="This is to far away!";
	public string dead="You are dead.";
	public string levelUp="Your level increased and is now @Level!";
	public string talentRaise="[66FA33][Talent][-] @TalentName has now @SpentPoints points!";
	public string pickUpItem="[FF0000][Inventory][-] @ItemName ";
	public string questAccepted="[66FA33][Quest][-] @QuestName accepted!";
	public string questCompleted="[66FA33][Quest][-] @QuestName completed!";
	public string startSaving="[FF0000]Saving world...[-]";
	public string endSaving="[FF0000]World saved![-]";
	public string emptySpot="This spot with @ItemName is empty.";
	public string canNotUse="You can not use this!";
	public string needTool="You need @ItemName for this.";
	public string needsItemInInventory="You need @ItemName for this.";
	public string enterPVPZone="[FF0000]You entered a pvp zone![-]";
	public string exitPVPZone="[FF0000]You exit the pvp zone![-]";
	
	#if UNITY_EDITOR
	public void OnGUI(){
		GUI.changed=false;
		GUILayout.BeginVertical("Game Messages","box");
		GUILayoutUtility.GetRect(0,15);
		welcome=EditorGUILayout.TextField("Welcome",welcome);
		fullInventory=EditorGUILayout.TextField("Full Inventory", fullInventory);
		canNotEquip=EditorGUILayout.TextField("Can Not Equip", canNotEquip);
		farAway=EditorGUILayout.TextField("Far Away", farAway);
		dead=EditorGUILayout.TextField("Dead", dead);
		levelUp=EditorGUILayout.TextField("Level Up", levelUp);
		talentRaise=EditorGUILayout.TextField("Talent Raise", talentRaise);
		pickUpItem=EditorGUILayout.TextField("Pick Up Item", pickUpItem);
		questAccepted=EditorGUILayout.TextField("Quest Accepted", questAccepted);
		questCompleted=EditorGUILayout.TextField("Quest Completed", questCompleted);
		startSaving=EditorGUILayout.TextField("Start Saving",startSaving);
		endSaving=EditorGUILayout.TextField("End Saving",endSaving);
		emptySpot=EditorGUILayout.TextField("Empty Spot",emptySpot);
		canNotUse=EditorGUILayout.TextField("Can't Use",canNotUse);
		needTool=EditorGUILayout.TextField("Needed Tool",needTool);
		needsItemInInventory=EditorGUILayout.TextField("Needs Item",needsItemInInventory);
		enterPVPZone=EditorGUILayout.TextField("Enter PVP zone",enterPVPZone);
		exitPVPZone=EditorGUILayout.TextField("Exit PVP zone",exitPVPZone);
		
		GUILayout.EndVertical();
		if(GUI.changed){
			EditorUtility.SetDirty(GameManager.GameMessages);
		}
	}
	#endif
}
