using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class PlayerAttribute : ScriptableObject {
	
	public string displayName;
	public string attributeName;
	public int startValue;
	public float regenerationRate;
	public Color color=Color.white;
	public bool raisable=true;
	public bool fillOnStart=true;
	
	[System.NonSerialized]
	private UISprite bar;
	[System.NonSerialized]
	private float barLength;
	[System.NonSerialized]
	private float curValue;
	[System.NonSerialized]
	private float tempValue;
	[System.NonSerialized]
	private float baseValue;
	
	public void Init(UISprite attributeBar){
		if(attributeBar){
			this.bar=attributeBar;
			barLength=bar.transform.localScale.x;
		}
		BaseValue=startValue;
		if(fillOnStart){
			curValue=BaseValue+TempValue;
		}
		UpdateBar();
		UnityTools.StartCoroutine(Regenerate());
	}
	
	public void ApplyDamage(float val){
		curValue-=val;
		if(curValue<0){
			curValue=0;
		}
		UpdateBar();
	}
	
	public void HealDamage(float val){
		curValue+=val;
		if(curValue>baseValue+tempValue){
			curValue=baseValue+tempValue;
		}
		UpdateBar();
	}
	
	public void Refresh(){
		curValue=baseValue+tempValue;
		UpdateBar();
	}
	
	public float TempValue{
		get{return tempValue;}
		set{tempValue=value;
			HealDamage(0);
		}
	}
	
	public float BaseValue{
		get{return baseValue;}
		set{baseValue=value;
			UpdateBar();
		}
	}
	
	public float CurValue{
		get{return curValue;}
	}
	
	public UISprite BarWidget{
		get{return bar;}
		set{bar=value;}
	}
	
	public string Name{
		get{return attributeName;}
		set{attributeName=value;}
	}
	
	public void UpdateBar(){
		if(bar){
			float dx = (barLength * CurValue) / (BaseValue+TempValue)+0.01f;	
			bar.transform.localScale = new Vector3(dx, bar.transform.localScale.y, bar.transform.localScale.z);
		}
	}
	
	public void EnableBar(bool state){
		if(bar){
			bar.gameObject.SetActive(state);
		}
	}
	
	public IEnumerator Regenerate(){
		while(true){
			yield return new WaitForSeconds(regenerationRate);
			if(!GameManager.Player.Dead){
				HealDamage(1);
			}
		}
	}
	
	public IEnumerator Debuff(int val, float delay){
		yield return new WaitForSeconds(delay);
		if(TempValue-val < 0){
			TempValue=0;
		}else{
			TempValue-=val;
		}
	}
	
	public void Buff(int val){
		TempValue+=val;
	}
	
	#if UNITY_EDITOR
	public void OnGUI(){
		displayName=EditorGUILayout.TextField("Display Name",displayName);
		attributeName=EditorGUILayout.TextField("Name",attributeName);
		startValue=EditorGUILayout.IntField("Start Value",startValue);
		regenerationRate=EditorGUILayout.FloatField("Regeneration Rate",regenerationRate);
		color=EditorGUILayout.ColorField("Color",color);
		raisable=EditorGUILayout.Toggle("Raiseable",raisable);
	}
	#endif
}
