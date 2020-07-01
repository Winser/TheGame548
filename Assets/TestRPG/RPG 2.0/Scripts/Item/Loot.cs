using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Base npc loot class.
/// </summary>
[System.Serializable]
public class Loot
{
	/// <summary>
	/// ItemTable to spawn from.
	/// </summary>
	public ItemTable table;
	/// <summary>
	/// Number of objects to instantiate
	/// </summary>
	public int numberOfObjects = 1;
	/// <summary>
	/// GameObjects will be instantiated in this range
	/// </summary>
	public float range = 5.0f;
	
	/// <summary>
	/// Spawns a random item from ItemTable at a given position + random offset
	/// </summary>
	/// <param name='pos'>
	/// Reference position.
	/// </param>
	public void Spawn (Vector3 pos)
	{
		for (int i=0; i< numberOfObjects; i++) {
			BaseItem mItem = table.GetRandomItem ();

			GameObject go = (GameObject)PhotonNetwork.Instantiate (mItem.prefab.name, UnityTools.RandomPointInArea (pos, range), UnityTools.RandomQuaternion (Vector3.up, 0, 360),0);
			Lootable loot = go.GetComponent<Lootable> ();
			if (loot != null) {
				loot.item = mItem;
			}
		}
	}
}
