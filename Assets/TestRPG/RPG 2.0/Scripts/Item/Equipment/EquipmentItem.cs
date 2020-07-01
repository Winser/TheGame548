using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Equipment item.
/// </summary>
[System.Serializable]
public class EquipmentItem : BonusItem
{
	public enum Region
	{
		Head,
		Torso,
		Shoulders,
		Hands,
		Legs,
		Foot,
		Ring,
		Amulet
	}
	
	//Region to equip
	public Region equipmentRegion;
	public DamageTalent overrideMouseTalent;
	public AnimationClip overrideIdle;
	
	/// <summary>
	///  Use this Item. 
	/// </summary>
	public override bool Use ()
	{
		if(!base.Use ()){
			return false;
		}
		
		if(GameManager.Player.Inventory.RemoveItem(this)){
			GameManager.Player.Inventory.Equip(this);
		}
		return true;
	}
	
	#if UNITY_EDITOR
	public override void OnGUI ()
	{
		base.OnGUI ();
		equipmentRegion= (Region)EditorGUILayout.EnumPopup("Region",equipmentRegion);
		switch(equipmentRegion){
		case Region.Hands:
			overrideMouseTalent=(DamageTalent) EditorGUILayout.ObjectField("Override Talent",overrideMouseTalent,typeof(DamageTalent),false);
			overrideIdle=(AnimationClip)EditorGUILayout.ObjectField("Override Idle",overrideIdle,typeof(AnimationClip),false);
			break;
		}
	}
	#endif
}

