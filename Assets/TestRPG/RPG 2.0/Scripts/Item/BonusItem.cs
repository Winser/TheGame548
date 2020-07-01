using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Bonus item.
/// </summary>
[System.Serializable]
public class BonusItem : UsableItem {
	//Bonuses that this item has
	public List<Bonus> bonus;
	
	/// <summary>
	/// Use this Item.
	/// </summary>
	public override bool Use (){
		return base.Use ();
	}
	
	/// <summary>
	/// Randomizes the bonus.
	/// </summary>
	public void RandomizeBonus(){
		for(int i=0; i< bonus.Count; i++){
			bonus[i].bonusValue=Random.Range(bonus[i].minValue,bonus[i].maxValue);
		}
	}
	
	public Bonus GetBonus(string attribute){
		return bonus.Find(b=>b.attribute== attribute);
	}
	
	#if UNITY_EDITOR	
	public override void OnGUI(){
		base.OnGUI();
		
		if(GUILayout.Button("Add Bonus")){
			bonus.Add(new Bonus());
		}
		int index=-1;
		for(int i=0; i< bonus.Count; i++){
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("X",GUILayout.Width(20))){
				index=i;
			}
			bonus[i].attribute=EditorGUILayout.TextField("Attribute",bonus[i].attribute);
			GUILayout.EndHorizontal();
			bonus[i].minValue=EditorGUILayout.IntField("Min Value",bonus[i].minValue);
			bonus[i].maxValue=EditorGUILayout.IntField("Max Value",bonus[i].maxValue);
			bonus[i].color=EditorGUILayout.ColorField("Color",bonus[i].color);
		}
		
		if(index != -1){
			bonus.RemoveAt(index);
		}
	}
	#endif
}
