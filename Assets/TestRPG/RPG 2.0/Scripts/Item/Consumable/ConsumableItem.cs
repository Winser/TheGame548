using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Consumable item class
/// </summary>
[System.Serializable]
public class ConsumableItem : BonusItem {
	
	/// <summary>
	///  Use this Item. 
	/// </summary>
	public override bool Use ()
	{
		if(!base.Use ()){
			return false;
		}
		stack--;
		if(stack<=0){
			GameManager.Player.Inventory.RemoveItem(this);
		}
		return true;
	}
	
	#if UNITY_EDITOR
	public override void OnGUI ()
	{
		base.OnGUI ();
	}
	#endif
}
