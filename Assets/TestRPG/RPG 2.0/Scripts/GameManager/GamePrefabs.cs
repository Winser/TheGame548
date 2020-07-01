using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Storage class containing all game prefabs
/// </summary>
[System.Serializable]
public class GamePrefabs : ScriptableObject {
	public GameObject itemName;
	public GameObject talent;
	public GameObject damage;
	public GameObject bonus;
	public GameObject sellItem;
	public GameObject teleporter;
	public GameObject bloodParticle;
	public GameObject attribute;
	public GameObject craftingItem;
}
