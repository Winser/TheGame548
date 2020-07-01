using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Simple item storage asset
/// </summary>
public class ItemTable : ScriptableObject {
	/// <summary>
	/// The items.
	/// </summary>
	public List<BaseItem> items;
	
	/// <summary>
	/// Gets a random item.
	/// </summary>
	/// <returns>
	/// The random item.
	/// </returns>
	public BaseItem GetRandomItem(){
		if(items.Count>0){
			//Clone this item
			BaseItem item= (BaseItem)Instantiate(items[Random.Range(0,items.Count)]);
			if(item is BonusItem){
				//Randomize bonus
				(item as BonusItem).RandomizeBonus();
			}
			return item;
		}
		return null;
	}
	
	#if UNITY_EDITOR
	public virtual void OnGUI(){
		int index=-1;
		for(int i=0; i< items.Count; i++){
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("X", GUILayout.Width(20))){
				index=i;
			}
			items[i]=(BaseItem)EditorGUILayout.ObjectField(items[i],typeof(BaseItem), false);
			GUILayout.EndHorizontal();
		}
		
		if(index != -1){
			items.RemoveAt(index);
		}
	}
	#endif
}
