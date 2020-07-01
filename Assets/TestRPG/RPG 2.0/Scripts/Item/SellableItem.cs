using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Base sellable item
/// </summary>
[System.Serializable]
public class SellableItem : BaseItem {
	//Buy price in a shop
	public int buyPrice;
	//Sell price in a shop
	public int sellPrice;
	
	#if UNITY_EDITOR	
	public override void OnGUI(){
		base.OnGUI();
		buyPrice=EditorGUILayout.IntField("Buy Price", buyPrice);
		sellPrice=EditorGUILayout.IntField("Sell Price", sellPrice);
	}
	#endif
}
