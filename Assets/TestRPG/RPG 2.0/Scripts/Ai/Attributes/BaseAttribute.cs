using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class BaseAttribute : AbstractState
{
	public string name;
	public float baseValue;
	public float regenerationRate;
	public float curValue;
	public List<BaseComparerTransition> comparerTransitions;
	[System.NonSerialized]
	public UISprite bar;
	[System.NonSerialized]
	public float barLength;
	
	public BaseAttribute(){}
	
	public void Init (UISprite attributeBar)
	{
		curValue = baseValue;
		if(attributeBar){
			bar=attributeBar;
		}else{
			bar=InterfaceContainer.Instance.npcBar;
		}
		
		barLength=bar.transform.localScale.x;
		UpdateBar();
	}
	
	public virtual IEnumerator Regenerate ()
	{
		while (true) {
			yield return new WaitForSeconds(regenerationRate);
			if (curValue < baseValue && curValue > 0) {
				curValue++;
			}
		}
	}
	
	public virtual void Refresh ()
	{
		curValue = baseValue;
		UpdateBar();
	}
	
	public void ApplyDamage (AiBehaviour ai,float damage)
	{
		curValue-=damage;
		UpdateBar();
		
		if(PhotonNetwork.isMasterClient){
			CheckTransition(ai);
			if(ai.onApplyDamage != null){
				ai.onApplyDamage(name,damage,curValue);
			}
		}
	}
	
	public virtual void UpdateBar(){
		if(bar){
			float dx = (barLength * curValue) / (baseValue)+0.00001f;
			if(dx<0){
				dx=0.00001f;
			}
			bar.transform.localScale = new Vector3(dx, bar.transform.localScale.y, bar.transform.localScale.z);
		}
	}
	
	public void CheckTransition(AiBehaviour ai){
		float priority = -100;
		int id = -1;
		foreach (BaseComparerTransition comparerTransition in comparerTransitions) {
			if (comparerTransition is EqualComparerTransition && curValue.Equals (comparerTransition.compareValue)) {
				if (comparerTransition.priority > priority) {
					priority = comparerTransition.priority;
					id = comparerTransition.toStateId;
				}
			}
				
			if (comparerTransition is GreaterThanComparerTransition && curValue > comparerTransition.compareValue) {
				if (comparerTransition.priority > priority) {
					priority = comparerTransition.priority;
					id = comparerTransition.toStateId;
				}
			}
			
			if(comparerTransition is GetHitTransition){
				if (comparerTransition.priority > priority) {
					priority = comparerTransition.priority;
					id = comparerTransition.toStateId;
				}
			}

			if (comparerTransition is LessThanComparerTransition && curValue < comparerTransition.compareValue) {
				if (comparerTransition.priority > priority) {
					priority = comparerTransition.priority;
					id = comparerTransition.toStateId;
				}
			}
		}
		
		if(id != -1 && id != ai.CurStateId){
			foreach(KeyValuePair<int,BaseState> kvp in ai.GetStates()){
				kvp.Value.breakTimedTransition=true;
			}
			//Debug.Log("Switch "+ ai.GetStates()[id].GetType().ToString());
			ai.CurStateId=id;
		}
	}
	
#if UNITY_EDITOR
	[System.NonSerialized]
	public StateNode nameNode;
	[System.NonSerialized]
	public StateNode baseValueNode;
	[System.NonSerialized]
	public StateNode regenerationRateNode;
	[System.NonSerialized]
	public StateNode curValueNode;
	
	public BaseAttribute(Vector2 position){
		this.Position=position;
		this.Size= new Vector2(140,100);
		this.Title="Base Attribute";
		nameNode= new StateNode("Name",this,typeof(StringField));
		baseValueNode= new StateNode("Base Value",this,typeof(FloatField));
		regenerationRateNode= new StateNode("Regeneration Rate",this,typeof(FloatField));
		curValueNode=new StateNode("Current Value",this);
		
		this.Nodes.Add(nameNode);
		this.Nodes.Add(baseValueNode);
		this.Nodes.Add(regenerationRateNode);
		this.Nodes.Add(curValueNode);
	}
	
	public override void Init(Vector2 position){
		base.Init(position);
		this.Position=position;
		this.Size= new Vector2(140,100);
		this.Title="Base Attribute";
		nameNode= new StateNode("Name",this,typeof(StringField));
		baseValueNode= new StateNode("Base Value",this,typeof(FloatField));
		regenerationRateNode= new StateNode("Regeneration Rate",this,typeof(FloatField));
		curValueNode= new StateNode("Current Value",this);
			
		this.Nodes.Add(nameNode);
		this.Nodes.Add(baseValueNode);
		this.Nodes.Add(regenerationRateNode);
		this.Nodes.Add(curValueNode);
	}
	public override void Init ()
	{
		base.Init ();
		name= nameNode.GetString();
		baseValue=baseValueNode.GetFloat();
		regenerationRate=regenerationRateNode.GetFloat();
	}
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		name=EditorGUILayout.TextField("Name", name);
		baseValue=EditorGUILayout.FloatField("Base Value",baseValue);
		regenerationRate=EditorGUILayout.FloatField("Regeneration Rate",regenerationRate);
	}
	
	public void Setup(){
		name= nameNode.GetString();
		baseValue=baseValueNode.GetFloat();
		regenerationRate=regenerationRateNode.GetFloat();
		
		comparerTransitions = curValueNode.GetBaseComparerTransition();
		foreach(BaseComparerTransition transition in comparerTransitions){
			transition.Setup();
		}
		
		x = Position.x;
		y = Position.y;
	}
#endif
}
