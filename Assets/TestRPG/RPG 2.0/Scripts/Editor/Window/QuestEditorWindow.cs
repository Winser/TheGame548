using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class QuestEditorWindow : EditorWindow {

	[MenuItem("RPG Kit 2.0/Editor/Quest")]
	static void Init ()
	{
		editor = (QuestEditorWindow)EditorWindow.GetWindow (typeof(QuestEditorWindow));
		editor.scrollPosition = new Vector2 ();
		editor.scrollView = new Vector2 (10000, 10000);
		editor.states = new List<AbstractState> ();
		EditorApplication.SaveCurrentSceneIfUserWantsTo();
		EditorApplication.OpenScene("Assets/RPG 2.0/Scenes/PreLoad.unity");
	}
	
	private static QuestEditorWindow editor;
	private Vector2 scrollPosition;
	private Vector2 scrollView;
	private List<AbstractState> states;
	
	private void OnGUI ()
	{
		GUILayout.BeginHorizontal (EditorStyles.toolbar);
		FileMenu ();
		GUILayout.EndHorizontal ();
		
		scrollPosition = GUI.BeginScrollView (new Rect (0, 20, this.position.width, this.position.height - 20), scrollPosition, new Rect (0, 20, scrollView.x, scrollView.y), true, true);
		GUILayout.BeginArea (new Rect (0, 20, scrollView.x, scrollView.y));
		
		foreach (AbstractState state in states) {
			if (!state.Equals (AbstractState.LastSelection)) {
				state.DrawState ();
			}
		}
		
		if (AbstractState.LastSelection != null) {
			AbstractState.LastSelection.DrawState ();
			AbstractState.LastSelection.HandleEvents ();
		}
		
		foreach (AbstractState state in states) {
			if (!state.Equals (AbstractState.LastSelection)) {
				state.HandleEvents ();
			}
		}
		
		if (StateNode.Selection != null) {
			StateNode.DrawConnection (StateNode.Selection.position, Event.current.mousePosition, true);
			Repaint ();
		}
		
		foreach (AbstractState state in states) {
			foreach (StateNode node in state.Nodes) {
				foreach (StateNode target in node.Targets) {
					StateNode.DrawConnection (node.position, target.position, false);
				}
			}
		}
		if (AbstractState.Selection != null) {
			AutoPan (0.6f);
		}
		GUILayout.EndArea ();
		GUI.EndScrollView ();
	
		
		switch (Event.current.type) {
		case EventType.MouseUp:
			AbstractState.Selection = null;
			StateNode.Selection = null;
			Event.current.Use ();
			break;
		case EventType.MouseDrag:
			if (AbstractState.Selection != null) {
				AbstractState.Selection.Position += Event.current.delta;
				Event.current.Use ();
			}
			break;
		
		}

	}
	
	private void FileMenu ()
	{
		
		if (GUILayout.Button ("File", EditorStyles.toolbarDropDown, GUILayout.Width (60))) {
			GenericMenu toolsMenu = new GenericMenu ();
			Vector2 pos = new Vector2 (position.width / 2 + scrollPosition.x, position.height / 2 + scrollPosition.y);
			
			toolsMenu.AddItem (new GUIContent ("Create/Quest/New Quest"), false, OnCreate, new QuestState (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Quest/State/Start Quest"), false, OnCreate, new StartQuest (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Quest/State/Talk To"), false, OnCreate, new TalkTo (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Quest/State/Kill"), false, OnCreate, new Kill (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Quest/State/Take Item"), false, OnCreate, new TakeItem (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Quest/State/Say"), false, OnCreate, new Say (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Quest/State/End Quest"), false, OnCreate, new EndQuest (pos));
			
			toolsMenu.AddItem (new GUIContent ("Create/Quest/Condition/Level"), false, OnCreate, new LevelCondition (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Quest/Condition/Attribute"), false, OnCreate, new AttributeCondition (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Quest/Condition/Exp"), false, OnCreate, new ExpCondition (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Quest/Condition/Quest Complete"), false, OnCreate, new QuestCompleteCondition (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Quest/Condition/Class"), false, OnCreate, new ClassCondition (pos));
			
			toolsMenu.AddItem (new GUIContent ("Create/Variable/Animation"), false, OnCreate, new AnimationField (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Variable/Float"), false, OnCreate, new FloatField (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Variable/Int"), false, OnCreate, new IntField (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Variable/String"), false, OnCreate, new StringField (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Variable/Text"), false, OnCreate, new TextField (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Variable/Item"), false, OnCreate, new ItemField (pos));
			
			
			toolsMenu.AddItem (new GUIContent ("Save"), false, OnSave);
			toolsMenu.AddItem (new GUIContent ("Load"), false, OnLoad);
			
			toolsMenu.DropDown (new Rect (4, 0, 0, 16));
			EditorGUIUtility.ExitGUI ();
		}
		  
		if (GUILayout.Button ("Clear", EditorStyles.toolbarDropDown, GUILayout.Width (60))) {
			GenericMenu toolsMenu = new GenericMenu ();
			toolsMenu.AddItem (new GUIContent ("All"), false, OnClearAll);
			toolsMenu.AddItem (new GUIContent ("Selected"), false, OnClearSelected);
			toolsMenu.DropDown (new Rect (64, 0, 0, 16));
			EditorGUIUtility.ExitGUI ();
			
		}
		GUILayout.FlexibleSpace ();
	}
		
	private void OnSave ()
	{
		SaveQuest();
	}
	
	private void SaveQuest(){
		QuestState quest = null;
		foreach (AbstractState mState in states) {
			if (mState is QuestState) {
				quest = mState as QuestState;
			}
		}
		
		if (quest == null) {
			if (EditorUtility.DisplayDialog ("No Quest", "There is no Quest, you should create one before save.", "Create", "Cancel")) {
				OnCreate (new QuestState (new Vector2 (position.width / 2 + scrollPosition.x, position.height / 2 + scrollPosition.y)));
			}
			return;
		}
		
		string mPath = EditorUtility.SaveFilePanelInProject (
         	       "Save Quest",
            	    "New Quest.bytes",
                	"bytes", "");
		
		if (mPath.Equals (string.Empty)) {
			return;
		}
		
		int cnt = 0;
		for (int i=0; i< states.Count; i++) {
			if (states [i] is BaseTaskState) {
				((BaseTaskState)states [i]).id = i;
				cnt++;
			}
		}
		
		FileStream fileStream = new FileStream (mPath, FileMode.Create); 
		BinaryFormatter formatter = new BinaryFormatter ();
		formatter.Binder = new VersionDeserializationBinder ();
		
		quest.Save (fileStream, formatter);
		formatter.Serialize (fileStream, cnt);
		
		for (int i=0; i< states.Count; i++) {
			if (states [i] is BaseTaskState) {
				states [i].Save (fileStream, formatter);
			}
		}
		
		fileStream.Close ();
		AssetDatabase.Refresh ();
	}
	
	private void OnLoad ()
	{
		string mPath = EditorUtility.OpenFilePanel (
                "Open Quest",
                "",
                "bytes");
		
		FileStream fs = new FileStream (mPath, FileMode.Open);
		BinaryFormatter formatter = new BinaryFormatter ();
		formatter.Binder = new VersionDeserializationBinder ();
		
		string questName = (string)formatter.Deserialize (fs);
		int initialTaskId = (int)formatter.Deserialize (fs);
		
		QuestState quest = new QuestState (new Vector2 ((float)formatter.Deserialize (fs), (float)formatter.Deserialize (fs)));
		OnCreate (quest);
		
		int cnt = (int)formatter.Deserialize (fs);
		
		StringField questNameField = new StringField (quest.Position + new Vector2 (quest.Size.x + 50, -50));
		questNameField.stringVar = questName;
		OnCreate (questNameField);
		quest.questNameNode.ConnectTo (questNameField.InputNode);
		
		
		for (int i=0; i< cnt; i++) {
			BaseTaskState state = (BaseTaskState)formatter.Deserialize (fs);
			state.Init (new Vector2 (state.x, state.y));
			OnCreate (state);
			
			if(state is StartQuest){
				StartQuest startQuest= state as StartQuest;
				
				TextField startQuestText=new TextField(startQuest.Position+new Vector2(startQuest.Size.x+50,-50));
				startQuestText.stringVar=startQuest.text;
				OnCreate(startQuestText);
				startQuest.textNode.ConnectTo(startQuestText.InputNode);
				
				ItemField startQuestItem= new ItemField(startQuest.Position+new Vector2(startQuest.Size.x+50,50));
				//Load Item
				

				startQuestItem.item=GameManager.ItemDatabase.GetItem(startQuest.rewardItemName);
				
				OnCreate(startQuestItem);
				startQuest.rewardItemNode.ConnectTo(startQuestItem.InputNode);
				
				IntField goldField= new IntField(startQuest.Position+new Vector2(startQuest.Size.x+50,100));
				goldField.intVar=startQuest.gold;
				OnCreate(goldField);
				startQuest.goldNode.ConnectTo(goldField.InputNode);
				
				FloatField expField= new FloatField(startQuest.Position+new Vector2(startQuest.Size.x+50,150));
				expField.floatVar=startQuest.exp;
				OnCreate(expField);
				startQuest.expNode.ConnectTo(expField.InputNode);
				
			}
			
			if(state is Kill){
				Kill kill= state as Kill;
				
				StringField killDesc= new StringField(kill.Position+new Vector2(kill.Size.x+50,-50));
				killDesc.stringVar=kill.description;
				OnCreate(killDesc);
				kill.descriptionNode.ConnectTo(killDesc.InputNode);
				
				StringField killParam= new StringField(kill.Position+new Vector2(kill.Size.x+50,0));
				killParam.stringVar=kill.parameter;
				OnCreate(killParam);
				kill.parameterNode.ConnectTo(killParam.InputNode);
				
				IntField killAmount= new IntField(kill.Position+new Vector2(kill.Size.x+50,50));
				killAmount.intVar=kill.amount;
				OnCreate(killAmount);
				kill.amountNode.ConnectTo(killAmount.InputNode);
			}
			
			if( state is Say){
				Say say=state as Say;
				
				StringField sayDesc= new StringField(say.Position+new Vector2(say.Size.x+50,-50));
				sayDesc.stringVar=say.description;
				OnCreate(sayDesc);
				say.descriptionNode.ConnectTo(sayDesc.InputNode);
				
				TextField sayText=new TextField(say.Position+new Vector2(say.Size.x+50,50));
				sayText.stringVar=say.text;
				OnCreate(sayText);
				say.textNode.ConnectTo(sayText.InputNode);
				
			}
			
			if(state is TalkTo){
				TalkTo talkTo= state as TalkTo;
				
				StringField talkDesc= new StringField(talkTo.Position+new Vector2(talkTo.Size.x+50,-50));
				talkDesc.stringVar=talkTo.description;
				OnCreate(talkDesc);
				talkTo.descriptionNode.ConnectTo(talkDesc.InputNode);
				
				StringField talkParam= new StringField(talkTo.Position+new Vector2(talkTo.Size.x+50,0));
				talkParam.stringVar=talkTo.parameter;
				OnCreate(talkParam);
				talkTo.parameterNode.ConnectTo(talkParam.InputNode);
				
				TextField talkText=new TextField(talkTo.Position+new Vector2(talkTo.Size.x+50,100));
				talkText.stringVar=talkTo.text;
				OnCreate(talkText);
				talkTo.textNode.ConnectTo(talkText.InputNode);
			}
			
			if(state is TakeItem){
				TakeItem takeItem= state as TakeItem;
				
				StringField takeDesc= new StringField(takeItem.Position+new Vector2(takeItem.Size.x+50,-50));
				takeDesc.stringVar=takeItem.description;
				OnCreate(takeDesc);
				takeItem.descriptionNode.ConnectTo(takeDesc.InputNode);
				
				ItemField itemToTake= new ItemField(takeItem.Position+new Vector2(takeItem.Size.x+50,0));
				//Load Item
				itemToTake.item=GameManager.ItemDatabase.GetItem(takeItem.itemName);
				
				OnCreate(itemToTake);
				takeItem.itemNode.ConnectTo(itemToTake.InputNode);
			}
			
			if(state is EndQuest){
				EndQuest endQuest= state as EndQuest;
				
				StringField endDesc= new StringField(endQuest.Position+new Vector2(endQuest.Size.x+50,-50));
				endDesc.stringVar=endQuest.description;
				OnCreate(endDesc);
				endQuest.descriptionNode.ConnectTo(endDesc.InputNode);
			}
			
			foreach (BaseCondition condition in state.conditions) {
				condition.Init (new Vector2 (condition.x, condition.y));
				OnCreate (condition);
				state.conditionNode.ConnectTo (condition.InputNode);
				
				if(condition is LevelCondition){
					LevelCondition levelCondition= condition as LevelCondition;
					
					IntField minLevel=new IntField(levelCondition.Position+ new Vector2(levelCondition.Size.x+50,-50));
					minLevel.intVar=levelCondition.minValue;
					OnCreate(minLevel);
					levelCondition.minLevelNode.ConnectTo(minLevel.InputNode);
					
					IntField maxLevel=new IntField(levelCondition.Position+ new Vector2(levelCondition.Size.x+50,0));
					maxLevel.intVar=levelCondition.maxValue;
					OnCreate(maxLevel);
					levelCondition.maxLevelNode.ConnectTo(maxLevel.InputNode);
					
				}
				
				if(condition is AttributeCondition){
					AttributeCondition attrCondition= condition as AttributeCondition;
						
					StringField attr= new StringField(attrCondition.Position+new Vector2(attrCondition.Size.x+50,-50));
					attr.stringVar=attrCondition.attribute;
					OnCreate(attr);
					attrCondition.attributeNode.ConnectTo(attr.InputNode);
					
					IntField minAttr=new IntField(attrCondition.Position+ new Vector2(attrCondition.Size.x+50,0));
					minAttr.intVar=attrCondition.minValue;
					OnCreate(minAttr);
					attrCondition.minValueNode.ConnectTo(minAttr.InputNode);
					
					IntField maxAttr=new IntField(attrCondition.Position+ new Vector2(attrCondition.Size.x+50,50));
					maxAttr.intVar=attrCondition.maxValue;
					OnCreate(maxAttr);
					attrCondition.maxValueNode.ConnectTo(maxAttr.InputNode);
				}
				
				if(condition is ClassCondition){
					ClassCondition classCondition= condition as ClassCondition;
					
					StringField cClass= new StringField(classCondition.Position+new Vector2(classCondition.Size.x+50,-50));
					cClass.stringVar=classCondition.cClass;
					OnCreate(cClass);
					classCondition.classNode.ConnectTo(cClass.InputNode);
				}
				
				if(condition is QuestCompleteCondition){
					QuestCompleteCondition questCondition= condition as QuestCompleteCondition;
					
					StringField qName= new StringField(questCondition.Position+new Vector2(questCondition.Size.x+50,-50));
					qName.stringVar=questCondition.questName;
					OnCreate(qName);
					questCondition.questCompletedNode.ConnectTo(qName.InputNode);
				}
			}
		}
		
		quest.initialTaskNode.ConnectTo(GetBaseTask(initialTaskId).InputNode);
		
		for(int i=0; i< states.Count; i++){
			if(states[i] is BaseTaskState){
				BaseTaskState state= states[i] as BaseTaskState;
				BaseTaskState toTask= GetBaseTask(state.toTaskId);
				if(state.toTaskNode != null && toTask != null){
					state.toTaskNode.ConnectTo(toTask.InputNode);
				}
			}
		}
	}
	
	public BaseTaskState GetBaseTask(int id){
		foreach (AbstractState state in states) {
			if (state is BaseTaskState && (state as BaseTaskState).id.Equals (id)) {
				return state as BaseTaskState;
			}
		}
		return null;
	}
	
	private BaseState GetState (int id)
	{
		foreach (AbstractState state in states) {
			if (state is BaseState && (state as BaseState).id.Equals (id)) {
				return state as BaseState;
			}
		}
		return null;
	}
	
	private void OnCreate (object data)
	{
		AbstractState mState = data as AbstractState;
		states.Add (mState);
		AbstractState.LastSelection = mState;
	}
		
	private void OnClearAll ()
	{
		states.Clear ();
		AbstractState.LastSelection = null;
		
	}
	
	private void OnClearSelected ()
	{
		
		if (AbstractState.LastSelection == null) {
			return;
		}
		
		List<StateNode> remove = new List<StateNode> ();
		
		foreach (AbstractState state in states) {
			foreach (StateNode node in state.Nodes) {
				foreach (StateNode target in node.Targets) {
					if (target.state == AbstractState.LastSelection) {
						remove.Add (target);
					}
				}
			}
		}
		
		foreach (AbstractState state in states) {
			foreach (StateNode node in state.Nodes) {
				foreach (StateNode target in remove) {
					node.Disconnect (target);
				}

			}
		}

		states.Remove (AbstractState.LastSelection);
		AbstractState.LastSelection = null;
	}
	
	public void AutoPan (float speed)
	{

		if (Event.current.mousePosition.x > position.width + scrollPosition.x - 50) {
			scrollView.x += (speed + 1);
			scrollPosition.x += speed;
			AbstractState.Selection.Position += new Vector2 (speed, 0);
			Repaint ();
		}

		if ((Event.current.mousePosition.x < scrollPosition.x + 50) && scrollPosition.x > 0) {
			scrollPosition.x -= speed;
			AbstractState.Selection.Position -= new Vector2 (speed, 0);
			Repaint ();
		}

		if (Event.current.mousePosition.y > position.height + scrollPosition.y - 50) {
			scrollView.y += (speed + 1);
			scrollPosition.y += speed;
			AbstractState.Selection.Position += new Vector2 (0, speed);
			Repaint ();
		}

		if ((Event.current.mousePosition.y < scrollPosition.y + 50) && scrollPosition.y > 0) {
			scrollPosition.y -= speed;
			AbstractState.Selection.Position -= new Vector2 (0, speed);
			Repaint ();
		}
	}
	
	void OnFocus ()
	{
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
		SceneView.onSceneGUIDelegate += this.OnSceneGUI;
	}
	
	void OnDestroy ()
	{
		SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
	}
	
	void OnSceneGUI (SceneView sceneView)
	{
		foreach (AbstractState state in states) {
			state.OnSceneGUI ();
		}
	}
}
