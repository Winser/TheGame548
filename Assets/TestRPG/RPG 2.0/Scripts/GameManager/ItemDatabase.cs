using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class that contains all items 
/// </summary>
[System.Serializable]
public class ItemDatabase : ScriptableObject {
	public List<BaseItem> items;
	
	/// <summary>
	/// Finds the item by name
	/// </summary>
	/// <returns>
	/// The item asset.
	/// </returns>
	/// <param name='itemName'>
	/// Item name.
	/// </param>
	public BaseItem GetItem(string itemName){
		return items.Find(item => item.itemName == itemName);
	}
	
	/// <summary>
	/// Adds the item to the database asset. This method should be used only in editor mode.
	/// </summary>
	/// <param name='item'>
	/// Item asset
	/// </param>
	public void AddItem(BaseItem item){
		if(!items.Contains(item)){
			items.Add(item);
		}
	}
}
