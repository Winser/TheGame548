using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Base Item class
/// </summary>
[System.Serializable]
public class BaseItem : ScriptableObject {
	//Item name
	public string itemName;
	//Item description
	public string description;
	//Item prefab
	public GameObject prefab;
	//Is item stackable?
	public bool stackable;
	//Item icon
	public string icon;
	/// <summary>
	/// Items that are needed to craft this item
	/// </summary>
	public ItemTable neededCraftingItems;
	/// <summary>
	/// Sound that will be played when an item is droped down by player.
	/// </summary>
	public AudioClip dropSound;
	
	
	[System.NonSerialized]
	public int stack = 1;
	/// <summary>
	/// ItemSlot identifier, used to save and load inventory items
	/// </summary>
	//
	[System.NonSerialized]
	public int itemSlotId=-1;
	
	#if UNITY_EDITOR	
	public virtual void OnGUI(){
		itemName = EditorGUILayout.TextField("Name",itemName);
		description = EditorGUILayout.TextField("Description",description);
		prefab= (GameObject)EditorGUILayout.ObjectField("Prefab",prefab,typeof(GameObject),false);
		icon=EditorGUILayout.TextField("Icon",icon);
		stackable=EditorGUILayout.Toggle("Stackable",stackable);
		neededCraftingItems=(ItemTable)EditorGUILayout.ObjectField("Crafting Table",neededCraftingItems,typeof(ItemTable),false);
		dropSound=(AudioClip)EditorGUILayout.ObjectField("Drop Sound",dropSound,typeof(AudioClip),false);
	}
	#endif
}

[System.Serializable]
public struct Pair<K, V>
{
	public Pair(K k, V v) : this() { Key = k; Value = v; }
  
	public K Key { get; set; }
	public V Value { get; set; }
}
