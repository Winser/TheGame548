using UnityEngine;
using System.Collections;

/// <summary>
/// Potion class
/// </summary>
[System.Serializable]
public class Potion : ConsumableItem {

	/// <summary>
	///  Use this Item. 
	/// </summary>
	public override bool Use ()
	{
		if(!base.Use ()){
			return false;
		}
		
		for(int i=0; i< bonus.Count; i++){
			PlayerAttribute attribute= GameManager.Player.GetAttribute(bonus[i].attribute);
			if(attribute){
				attribute.HealDamage(bonus[i].bonusValue);
			}
		}
		return true;
	}
}
