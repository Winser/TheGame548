using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class AiEditorWindow : EditorWindow
{
	[MenuItem("RPG Kit 2.0/Editor/Ai")]
	static void Init ()
	{
		editor = (AiEditorWindow)EditorWindow.GetWindow (typeof(AiEditorWindow));
		editor.scrollPosition = new Vector2 ();
		editor.scrollView = new Vector2 (10000, 10000);
		editor.states = new List<AbstractState> ();	
	}
	
	private static AiEditorWindow editor;
	private Vector2 scrollPosition;
	private Vector2 scrollView;
	private List<AbstractState> states;
	private bool hideVariables;
	
	private void OnGUI ()
	{
		GUILayout.BeginHorizontal (EditorStyles.toolbar);
		FileMenu ();
		GUILayout.EndHorizontal ();
		
		scrollPosition = GUI.BeginScrollView (new Rect (0, 20, this.position.width, this.position.height - 20), scrollPosition, new Rect (0, 20, scrollView.x, scrollView.y), true, true);
		GUILayout.BeginArea (new Rect (0, 20, scrollView.x, scrollView.y));
		
		foreach (AbstractState state in states) {
			if(hideVariables && IsVariable(state)){
				
			}else{
				
				if (!state.Equals (AbstractState.LastSelection)) {
					state.DrawState ();
				}
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
					if(hideVariables && node.type != null && node.type != typeof(BaseAttribute)){
						
					}else{
						StateNode.DrawConnection (node.position, target.position, false);
					}
				}
			}
		}
		if (AbstractState.Selection != null) {
			AutoPan (0.6f);
		}
		GUILayout.EndArea ();
		GUI.EndScrollView ();
	
		/*if(Event.current.clickCount==2 && AbstractState.LastSelection != null && AbstractState.LastSelection.AreaRect.Contains(Event.current.mousePosition)){
			AiPopupWindow.Show(position.center,AbstractState.LastSelection);
			Event.current.Use();
		}*/
		switch (Event.current.type) {
		case EventType.MouseUp:
			AbstractState.Selection = null;
			if(StateNode.Selection != null){
				if(StateNode.Selection.type != null){
					AbstractState mState= OnCreate(System.Activator.CreateInstance(StateNode.Selection.type,Event.current.mousePosition+scrollPosition),true);
					StateNode.Selection.ConnectTo(mState.InputNode);
				}
				
			}
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
			
	private bool IsVariable(AbstractState state){

		if(state is StringField ||
			state is AnimationField ||
			state is FloatField || 
			state is IntField ||
			state is TalentField ||
			state is ItemField || 
			state is ItemTableField){
			return true;
		
		}
		return false;
	}
	
	private void FileMenu ()
	{
		
		if (GUILayout.Button ("File", EditorStyles.toolbarDropDown, GUILayout.Width (60))) {
			GenericMenu toolsMenu = new GenericMenu ();
			Vector2 pos = new Vector2 (position.width / 2 + scrollPosition.x, position.height / 2 + scrollPosition.y);
			toolsMenu.AddItem (new GUIContent ("Create/Ai/New Behaviour"), false, OnCreate, new AiBehaviourState (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/State/Idle"), false, OnCreate, new IdleState (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/State/Follow"), false, OnCreate, new FollowState (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/State/Walk"), false, OnCreate, new WalkState (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/State/Attack"), false, OnCreate, new AttackState (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/State/Dead"), false, OnCreate, new DeadState (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/State/Respawn"), false, OnCreate, new RespawnState (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/State/Flee"), false, OnCreate, new FleeState (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/State/Help"), false, OnCreate, new HelpState (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/State/Patrol"), false, OnCreate, new PatrolState (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/State/Say"), false, OnCreate, new SayState (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/State/Talent"), false, OnCreate, new TalentState (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/State/OnHit"), false, OnCreate, new GetHitState (pos));
			
			toolsMenu.AddItem (new GUIContent ("Create/Attribute/Base Attribute"), false, OnCreate, new BaseAttribute (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Attribute/Get Hit"), false, OnCreate, new GetHitTransition (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Attribute/Comparer/Equal"), false, OnCreate, new EqualComparerTransition (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Attribute/Comparer/Greater Than"), false, OnCreate, new GreaterThanComparerTransition (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Attribute/Comparer/Less Than"), false, OnCreate, new LessThanComparerTransition (pos));
			
			toolsMenu.AddItem (new GUIContent ("Create/Ai/Transition/View Field"), false, OnCreate, new ViewFieldTransition (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/Transition/Within Distance"), false, OnCreate, new WithinDistanceTransition (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/Transition/Beyond Distance"), false, OnCreate, new BeyondDistanceTransition (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/Transition/Timed"), false, OnCreate, new TimedTransition (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Ai/Transition/Listener"), false, OnCreate, new ListenerTransition (pos));
			
//			toolsMenu.AddItem (new GUIContent ("Create/Character/Base Character"), false, OnCreate, new BaseCharacter (pos));
			//toolsMenu.AddItem (new GUIContent ("Create/Character/Motor"), false, OnCreate, new CharacterMotor (pos));
			
			
			toolsMenu.AddItem (new GUIContent ("Create/Variable/Animation"), false, OnCreate, new AnimationField (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Variable/Float"), false, OnCreate, new FloatField (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Variable/Int"), false, OnCreate, new IntField (pos));
			
			toolsMenu.AddItem (new GUIContent ("Create/Variable/String"), false, OnCreate, new StringField (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Variable/Item Table"), false, OnCreate, new ItemTableField (pos));
			toolsMenu.AddItem (new GUIContent ("Create/Variable/Talent"), false, OnCreate, new TalentField (pos));
			
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
		
		if(GUILayout.Button("Hide Variables",EditorStyles.toolbarButton,GUILayout.Width(100))){
			hideVariables=!hideVariables;
		}
		GUILayout.FlexibleSpace ();
	}
		
	private void OnSave ()
	{
		SaveAi();
	}
	
	private void SaveAi(){
		AiBehaviourState ai = null;
		foreach (AbstractState mState in states) {
			if (mState is AiBehaviourState) {
				ai = mState as AiBehaviourState;
			}
		}
		
		if (ai == null) {
			if (EditorUtility.DisplayDialog ("No Ai Behavior", "There is no Ai Behaviour, you should create one before save.", "Create", "Cancel")) {
				OnCreate (new AiBehaviourState (new Vector2 (position.width / 2 + scrollPosition.x, position.height / 2 + scrollPosition.y)));
			}
			return;
		}
		
		string mPath = EditorUtility.SaveFilePanelInProject (
         	       "Save Ai",
            	    "New Ai.bytes",
                	"bytes", "");
		
		if (mPath.Equals (string.Empty)) {
			return;
		}
		
		int cnt = 0;
		for (int i=0; i< states.Count; i++) {
			if (states [i] is BaseState) {
				((BaseState)states [i]).id = i;
				cnt++;
			}
		}
		
		FileStream fileStream = new FileStream (mPath, FileMode.Create); 
		BinaryFormatter formatter = new BinaryFormatter ();
		formatter.Binder = new VersionDeserializationBinder ();
		
		ai.Save (fileStream, formatter);
		formatter.Serialize (fileStream, cnt);
		
		for (int i=0; i< states.Count; i++) {
			if (states [i] is BaseState) {
				states [i].Save (fileStream, formatter);
			}
		}
		
		fileStream.Close ();
		AssetDatabase.Refresh ();
	}
	
	private void OnLoad ()
	{
		string mPath = EditorUtility.OpenFilePanel (
                "Open Ai",
                "",
                "bytes");
		
		FileStream fs = new FileStream (mPath, FileMode.Open);
		BinaryFormatter formatter = new BinaryFormatter ();
		formatter.Binder = new VersionDeserializationBinder ();
		
		List<string> tags = (List<string>)formatter.Deserialize (fs);
		int initialStateId = (int)formatter.Deserialize (fs);
		List<BaseAttribute> attributes = (List<BaseAttribute>)formatter.Deserialize (fs);
		
		AiBehaviourState ai = new AiBehaviourState (new Vector2 ((float)formatter.Deserialize (fs), (float)formatter.Deserialize (fs)));
		OnCreate (ai);
		
		int cnt = (int)formatter.Deserialize (fs);
		
		foreach (string tag in tags) {
			StringField tagField = new StringField (ai.Position + new Vector2 (ai.Size.x + 50, -50));
			tagField.stringVar = tag;
			OnCreate (tagField);
			ai.tagNode.ConnectTo (tagField.InputNode);
		}
		
		for (int i=0; i< cnt; i++) {
			BaseState state = (BaseState)formatter.Deserialize (fs);
			state.Init (new Vector2 (state.x, state.y));
			OnCreate (state);
			foreach (BaseTransition transition in state.transitions) {
				transition.Init (new Vector2 (transition.x, transition.y));
				OnCreate (transition);
				state.transitionNode.ConnectTo (transition.InputNode);
			}
			//if(!(state is TalentState)){ 
				StringField animationField = new StringField (state.Position + new Vector2 (state.Size.x + 50, -70));
				OnCreate (animationField);
				animationField.stringVar = state.animation;
				state.animationNode.ConnectTo (animationField.InputNode);
			//}
			
			if (state is FollowState) {
				FollowState followState = state as FollowState;
				
				FloatField followSpeedField = new FloatField (state.Position + new Vector2 (state.Size.x + 50, 30));
				OnCreate (followSpeedField);
				followSpeedField.floatVar = followState.followSpeed;
				followState.followSpeedNode.ConnectTo (followSpeedField.InputNode);
				
				FloatField followRotationSpeedField = new FloatField (state.Position + new Vector2 (state.Size.x + 50, 80));
				OnCreate (followRotationSpeedField);
				followRotationSpeedField.floatVar = followState.followRotationSpeed;
				followState.followRotationSpeedNode.ConnectTo (followRotationSpeedField.InputNode);
			}
			
			if (state is WalkState) {
				WalkState walkState = state as WalkState;
				
				FloatField walkSpeedField = new FloatField (state.Position + new Vector2 (state.Size.x + 50, -20));
				walkSpeedField.floatVar = walkState.walkSpeed;
				OnCreate (walkSpeedField);
				walkState.walkSpeedNode.ConnectTo (walkSpeedField.InputNode);
				
				
				FloatField walkRotationSpeedField = new FloatField (state.Position + new Vector2 (state.Size.x + 50, 30));
				walkRotationSpeedField.floatVar = walkState.walkRotation;
				OnCreate (walkRotationSpeedField);
				walkState.walkRotationNode.ConnectTo (walkRotationSpeedField.InputNode);
				
				FloatField walkRadiusField = new FloatField (state.Position + new Vector2 (state.Size.x + 50, 80));
				walkRadiusField.floatVar = walkState.walkRadius;
				OnCreate (walkRadiusField);
				walkState.walkRadiusNode.ConnectTo (walkRadiusField.InputNode);
				
			}
			
			if (state is AttackState) {
				AttackState attackState = state as AttackState;
				
				FloatField attackDamageField = new FloatField (state.Position + new Vector2 (state.Size.x + 50, -20));
				attackDamageField.floatVar = attackState.damage;
				OnCreate (attackDamageField);
				attackState.damageNode.ConnectTo (attackDamageField.InputNode);
				
				StringField attackAttributeNameField = new StringField (state.Position + new Vector2 (state.Size.x + 50, 30));
				attackAttributeNameField.stringVar = attackState.attributeName;
				OnCreate (attackAttributeNameField);
				attackState.attributeNameNode.ConnectTo (attackAttributeNameField.InputNode);
			}
			
			if (state is FleeState) {
				FleeState fleeState = state as FleeState;
				
				FloatField fleeSpeedField = new FloatField (state.Position + new Vector2 (state.Size.x + 50, -20));
				fleeSpeedField.floatVar = fleeState.fleeSpeed;
				OnCreate (fleeSpeedField);
				fleeState.fleeSpeedNode.ConnectTo (fleeSpeedField.InputNode);
			}
			
			if (state is HelpState) {
				HelpState helpState = state as HelpState;
				
				FloatField helpRadiusField = new FloatField (state.Position + new Vector2 (state.Size.x + 50, -20));
				helpRadiusField.floatVar = helpState.radius;
				OnCreate (helpRadiusField);
				helpState.radiusNode.ConnectTo (helpRadiusField.InputNode);
				
				StringField helpShoutField = new StringField (state.Position + new Vector2 (state.Size.x + 50, 30));
				helpShoutField.stringVar = helpState.shout;
				OnCreate (helpShoutField);
				helpState.shoutNode.ConnectTo (helpShoutField.InputNode);
			}
			
			if(state is DeadState){
				DeadState deadState= state as DeadState;
				
				FloatField expField = new FloatField (state.Position + new Vector2 (state.Size.x + 50, -20));
				expField.floatVar =deadState.exp;
				OnCreate (expField);
				deadState.expNode.ConnectTo (expField.InputNode);
				
				int l=0;
				foreach(string s in deadState.loot){
					StringField itemField= new StringField(state.Position + new Vector2 (state.Size.x + 50, 30+50*l));
					itemField.stringVar=s;
					OnCreate(itemField);
					deadState.lootNode.ConnectTo(itemField.InputNode);
					l++;
				}
			}
			
			if(state is PatrolState){
				PatrolState patrolState= state as PatrolState;
				
				IntField patrolId= new IntField(state.Position + new Vector2 (state.Size.x + 50, -20));
				patrolId.intVar=patrolState.patrolId;
				OnCreate(patrolId);
				patrolState.patrolIdNode.ConnectTo(patrolId.InputNode);

				FloatField patrolSpeedField= new FloatField(state.Position + new Vector2 (state.Size.x + 50, 30));
				patrolSpeedField.floatVar=patrolState.patrolSpeed;
				OnCreate(patrolSpeedField);
				patrolState.patrolSpeedNode.ConnectTo(patrolSpeedField.InputNode);
				
				FloatField patrolRotationField= new FloatField(state.Position + new Vector2 (state.Size.x + 50, 80));
				patrolRotationField.floatVar=patrolState.patrolRotation;
				OnCreate(patrolRotationField);
				patrolState.patrolRotationNode.ConnectTo(patrolRotationField.InputNode);
			}
			
			if(state is SayState){
				SayState sayState= state as SayState;
				
				int l=0;
				foreach(string s in sayState.sayText){
					StringField textField= new StringField(state.Position + new Vector2 (state.Size.x + 50, 30+50*l));
					textField.stringVar=s;
					OnCreate(textField);
					sayState.sayTextNode.ConnectTo(textField.InputNode);
					l++;
				}
			}
			
			if(state is TalentState){
				TalentState talentState= state as TalentState;
				
				TalentField talentField= new TalentField(state.Position + new Vector2 (state.Size.x + 50, -20));
				talentField.talent= GameManager.TalentDatabase.GetTalent(talentState.talent);
				OnCreate(talentField);
				talentState.talentNode.ConnectTo(talentField.InputNode);
			}
		}
		
		ai.initialStateNode.ConnectTo (GetState (initialStateId).InputNode);
		
		for (int i=0; i< states.Count; i++) {
			if (states [i] is BaseTransition) {
				(states [i] as BaseTransition).toStateNode.ConnectTo (GetState ((states [i] as BaseTransition).toStateId).InputNode);
				
				FloatField priorityField = new FloatField ((states [i] as BaseTransition).Position + new Vector2 ((states [i] as BaseTransition).Size.x + 40, -20));
				priorityField.floatVar = (states [i] as BaseTransition).priority;
				OnCreate (priorityField);
				(states [i] as BaseTransition).priorityNode.ConnectTo (priorityField.InputNode);
				
				
				if (states [i] is WithinDistanceTransition) {
					
					FloatField distanceField = new FloatField ((states [i] as BaseTransition).Position + new Vector2 ((states [i] as BaseTransition).Size.x + 40, 30));
					OnCreate (distanceField);
					(states [i] as WithinDistanceTransition).distanceNode.ConnectTo (distanceField.InputNode);
					distanceField.floatVar = (states [i] as WithinDistanceTransition).distance;
				}
				
				if (states [i] is BeyondDistanceTransition) {
					FloatField distanceField = new FloatField ((states [i] as BaseTransition).Position + new Vector2 ((states [i] as BaseTransition).Size.x + 40, 30));
					OnCreate (distanceField);
					(states [i] as BeyondDistanceTransition).distanceNode.ConnectTo (distanceField.InputNode);
					distanceField.floatVar = (states [i] as BeyondDistanceTransition).distance;
				}
				
				if (states [i] is TimedTransition) {
					FloatField timeField = new FloatField ((states [i] as BaseTransition).Position + new Vector2 ((states [i] as BaseTransition).Size.x + 40, 30));
					OnCreate (timeField);
					(states [i] as TimedTransition).timeNode.ConnectTo (timeField.InputNode);
					timeField.floatVar = (states [i] as TimedTransition).time;
				}
				
				if (states [i] is ViewFieldTransition) {
					FloatField distanceField = new FloatField ((states [i] as BaseTransition).Position + new Vector2 ((states [i] as BaseTransition).Size.x + 40, 30));
					OnCreate (distanceField);
					(states [i] as ViewFieldTransition).distanceNode.ConnectTo (distanceField.InputNode);
					distanceField.floatVar = (states [i] as ViewFieldTransition).distance;
					
					FloatField viewField = new FloatField ((states [i] as BaseTransition).Position + new Vector2 ((states [i] as BaseTransition).Size.x + 40, 80));
					OnCreate (viewField);
					(states [i] as ViewFieldTransition).viewFieldNode.ConnectTo (viewField.InputNode);
					viewField.floatVar = (states [i] as ViewFieldTransition).viewField;
					
					FloatField agentHeightField = new FloatField ((states [i] as BaseTransition).Position + new Vector2 ((states [i] as BaseTransition).Size.x + 40, 130));
					OnCreate (agentHeightField);
					(states [i] as ViewFieldTransition).agentHeightNode.ConnectTo (agentHeightField.InputNode);
					agentHeightField.floatVar = (states [i] as ViewFieldTransition).agentHeight;
				}
				
				if (states [i] is ListenerTransition) {
					StringField listnerField = new StringField ((states [i] as BaseTransition).Position + new Vector2 ((states [i] as BaseTransition).Size.x + 40, 30));
					OnCreate (listnerField);
					(states [i] as ListenerTransition).listenToNode.ConnectTo (listnerField.InputNode);
					listnerField.stringVar = (states [i] as ListenerTransition).listenTo;
				}
				
				if (states [i] is BaseComparerTransition) {
					FloatField comparerValueField = new FloatField ((states [i] as BaseComparerTransition).Position + new Vector2 ((states [i] as BaseComparerTransition).Size.x + 40, 30));
					OnCreate (comparerValueField);
					(states [i] as BaseComparerTransition).compareValueNode.ConnectTo (comparerValueField.InputNode);
					comparerValueField.floatVar = (states [i] as BaseComparerTransition).compareValue;
				}
			}
		}
		
		foreach (BaseAttribute attribute in attributes) {
			attribute.Init (new Vector2 (attribute.x, attribute.y));
			OnCreate (attribute);
			ai.attributeNode.ConnectTo (attribute.InputNode);
			
			StringField attributeNameField = new StringField (attribute.Position + new Vector2 (attribute.Size.x + 50, -80));
			attributeNameField.stringVar = attribute.name;
			OnCreate (attributeNameField);
			attribute.nameNode.ConnectTo (attributeNameField.InputNode);
			
			FloatField attributeBaseValueField = new FloatField (attribute.Position + new Vector2 (attribute.Size.x + 50, -30));
			attributeBaseValueField.floatVar = attribute.baseValue;
			OnCreate (attributeBaseValueField);
			attribute.baseValueNode.ConnectTo (attributeBaseValueField.InputNode);
			
			FloatField attributeRegenerationField = new FloatField (attribute.Position + new Vector2 (attribute.Size.x + 50, 20));
			attributeRegenerationField.floatVar = attribute.regenerationRate;
			OnCreate (attributeRegenerationField);
			attribute.regenerationRateNode.ConnectTo (attributeRegenerationField.InputNode);
			
			foreach (BaseComparerTransition attributeComparer in attribute.comparerTransitions) {
				attributeComparer.Init (new Vector2 (attributeComparer.x, attributeComparer.y));
				OnCreate (attributeComparer);
				attribute.curValueNode.ConnectTo (attributeComparer.InputNode);
				
				FloatField comparerPriorityField = new FloatField (attributeComparer.Position + new Vector2 (attributeComparer.Size.x + 40, -20));
				comparerPriorityField.floatVar = attributeComparer.priority;
				OnCreate (comparerPriorityField);
				attributeComparer.priorityNode.ConnectTo (comparerPriorityField.InputNode);
				
				if (! (attributeComparer is GetHitTransition)) {
					FloatField comparerValueField = new FloatField (attributeComparer.Position + new Vector2 (attributeComparer.Size.x + 40, 30));
					comparerValueField.floatVar = attributeComparer.compareValue;
					OnCreate (comparerValueField);
					attributeComparer.compareValueNode.ConnectTo (comparerValueField.InputNode);
				}
				attributeComparer.toStateNode.ConnectTo (GetState (attributeComparer.toStateId).InputNode);
			}
		}
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
	
	private AbstractState OnCreate (object data, bool r)
	{
		AbstractState mState = data as AbstractState;
		states.Add (mState);
		AbstractState.LastSelection = mState;
		return mState;
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
