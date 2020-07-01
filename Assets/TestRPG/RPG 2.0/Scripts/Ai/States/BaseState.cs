using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class BaseState:AbstractState
{
	public int id;
	public string animation;
	public List<BaseTransition> transitions;
	[System.NonSerialized]
	private bool startedTimedTransition;
	[System.NonSerialized]
	public bool breakTimedTransition;
	[System.NonSerialized]
	public string listenTo;
	
	public virtual void HandleState (AiBehaviour ai)
	{
		if(animation != string.Empty){
			ai.GetComponent<Animation>().CrossFade (animation);
		}
		CheckTransition (ai);
	}
	
	public void CheckTransition (AiBehaviour ai)
	{
		if(GameManager.Player == null){
			return;
		}
		
		float priority = -100;
		int id = -1;

		foreach (BaseTransition transition in transitions) {
			if(transition is ListenerTransition){
				ListenerTransition listenerTransition= transition as ListenerTransition;
				if(listenTo != null && listenTo.Equals(listenerTransition.listenTo)){
					if(listenerTransition.priority>priority){
						priority=listenerTransition.priority;
						id=listenerTransition.toStateId;
						listenTo=string.Empty;
					}
				}
			}
			
			if (transition is WithinDistanceTransition) {
				WithinDistanceTransition withinDistance = transition as WithinDistanceTransition;
				if (ai.targetDistance < withinDistance.distance) {
					if (withinDistance.priority > priority) {
						priority = withinDistance.priority;
						id = withinDistance.toStateId;
					}
				}
			}
			
			if (transition is BeyondDistanceTransition) {
				BeyondDistanceTransition beyondDistance = transition as BeyondDistanceTransition;
				if (ai.targetDistance > beyondDistance.distance) {
					if (beyondDistance.priority > priority) {
						priority = beyondDistance.priority;
						id = beyondDistance.toStateId;
					}
				}
			}
			
			if (transition is ViewFieldTransition) {
				ViewFieldTransition viewField = transition as ViewFieldTransition;
				if (ai.targetDistance < viewField.distance) { 
					if (ai.target != null && ai.CanSee (ai.target, viewField.viewField, viewField.agentHeight)) {
						if (viewField.priority > priority) {
							priority = viewField.priority;
							id = viewField.toStateId;
						}
					}
				}
			}
			
			if (transition is TimedTransition) {
				if (!startedTimedTransition) {
					TimedTransition timed = transition as TimedTransition;
					if (timed.priority > priority && timed.toStateId != ai.CurStateId) {
						id = -1;
						
						foreach (KeyValuePair<int,BaseState> kvp in ai.GetStates()) {
							kvp.Value.startedTimedTransition = false;
							kvp.Value.breakTimedTransition=true;
						}
						//Debug.Log("Started Timed "+ai.GetStates()[timed.toStateId].GetType().ToString()+" "+ai.GetStates()[timed.toStateId].animation);
						this.breakTimedTransition=false;
						ai.StartCoroutine (StartTimedTransition (ai, timed.time, timed.toStateId));
					}
				}
			}
		}
		
		if(PhotonNetwork.offlineMode){
			if(GameManager.Player.Dead && !ai.Dead ){
				id=ai.initialStateId;
			}
		}else{
			if(ai.target != null && ai.target.GetComponent<PhotonNetworkPlayer>().dead && !ai.Dead){
				id=ai.initialStateId;
			}
		}
		
		if (id != -1 && id != ai.CurStateId) {
			//Debug.Log("Switch "+ ai.GetStates()[id].GetType().ToString());
			ai.CurStateId = id;
		}
	}
	
	private IEnumerator StartTimedTransition (AiBehaviour ai, float time, int toState)
	{
		startedTimedTransition = true;
		yield return new WaitForSeconds(time);
		if(!breakTimedTransition){
//			Debug.Log("Switch Timed "+ai.GetStates()[toState].GetType().ToString()+" "+ai.GetStates()[toState].animation);
			ai.CurStateId = toState;
		}
		startedTimedTransition = false;
	}
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode animationNode;
	[System.NonSerialized]
	public StateNode transitionNode;
	
	public BaseState(Vector2 position){
		this.Position=position;
		animationNode= new StateNode("Animation",this,typeof(AnimationField));
		transitionNode= new StateNode("Transition",this);
		this.Nodes.Add(animationNode);
		this.Nodes.Add(transitionNode);
	}
	
	public override void Init(Vector2 position){
		base.Init(position);
		this.Position=position;
		animationNode= new StateNode("Animation",this,typeof(AnimationField));
		transitionNode= new StateNode("Transition",this);
		this.Nodes.Add(animationNode);
		this.Nodes.Add(transitionNode);
	}
	
	public override void Init ()
	{
		base.Init ();
		AnimationClip animationClip= animationNode.GetAnimationClip();
		if(animationClip != null){
			animation= animationNode.GetAnimationClip().name;
		}else{
			animation=animationNode.GetString();
		}
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		animation=EditorGUILayout.TextField("Animation",animation);
	}
	
	public override void Save (System.IO.FileStream fileStream, System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter)
	{
		AnimationClip animationClip= animationNode.GetAnimationClip();
		if(animationClip != null){
			animation= animationNode.GetAnimationClip().name;
		}else{
			animation=animationNode.GetString();
		}
		transitions= transitionNode.GetBaseTransition();
		foreach(BaseTransition transition in transitions){
			transition.Setup();
		}
		x = Position.x;
		y = Position.y;
		formatter.Serialize(fileStream,this);
	}

#endif
}
