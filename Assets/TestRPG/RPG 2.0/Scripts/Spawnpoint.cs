using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Simple class to spawn enemies, items...
/// </summary>
public class Spawnpoint : MonoBehaviour{
	//Range from this transform to spawn the objects
	public float range = 5.0f;
	//Which gameObjects should we instantiate
	public List<GameObject> objects;
	//Number of gameObjects to instantiate
	public int numberOfObjects = 1;
	//Should we instantiate on start or call the spawn method from another script
	public bool spawnOnStart=true;
	
	private void Start (){
		//We spawn only if we are the master client, it will spawn the game objects to all clients. 
		//Please note that to spawn a game object, it needs to be located in the main resources folder 
		//and has the photon view attached
		if (spawnOnStart && PhotonNetwork.isMasterClient) {
			Spawn ();
			//StartCoroutine(Spawn(2.0f));
		}
	}
	
	public void Spawn ()
	{
		for (int i=0; i< numberOfObjects; i++) {
			//Get random prefab from the object list
			GameObject prefab = objects [Random.Range (0, objects.Count)];
			
			GameObject go= null;
			//This will instantiate the game objects to all clients.
			if(!prefab.GetComponent<Lootable>()){ 
				go = PhotonNetwork.InstantiateSceneObject (prefab.name, UnityTools.RandomPointInArea(transform.position,range)+Vector3.up, UnityTools.RandomQuaternion(Vector3.up,0,360), 0, null);
			}else{
				 go = PhotonNetwork.Instantiate (prefab.name, UnityTools.RandomPointInArea(transform.position,range)+Vector3.up, UnityTools.RandomQuaternion(Vector3.up,0,360), 0);
			}
			Lootable lootable = go.GetComponent<Lootable> ();
			if (lootable) {
				//Clone the item asset
				lootable.item = (BaseItem)Instantiate(lootable.item);
				if(lootable.item is BonusItem){
					(lootable.item as BonusItem).RandomizeBonus();
				}
				GameObject itemName = (GameObject)Instantiate (GameManager.GamePrefabs.itemName, go.transform.position, Quaternion.identity);
				itemName.GetComponentInChildren<UILabel> ().text = "[" + lootable.item.stack.ToString () + "] " + lootable.item.itemName;
				
				itemName.transform.position = UnityTools.RandomPointInArea(go.transform.position,0);
				itemName.transform.parent = go.transform;
					
			}
			
		}
	}
	
	public IEnumerator Spawn (float delayBetweenSpawn)
	{
		for (int i=0; i< numberOfObjects; i++) {
			//Get random prefab from the object list
			GameObject prefab = objects [Random.Range (0, objects.Count)];
			
			GameObject go= null;
			//This will instantiate the game objects to all clients.
			if(!prefab.GetComponent<Lootable>()){ 
				go = PhotonNetwork.InstantiateSceneObject (prefab.name, UnityTools.RandomPointInArea(transform.position,range)+Vector3.up, UnityTools.RandomQuaternion(Vector3.up,0,360), 0, null);
			}else{
				 go = PhotonNetwork.Instantiate (prefab.name, UnityTools.RandomPointInArea(transform.position,range)+Vector3.up, UnityTools.RandomQuaternion(Vector3.up,0,360), 0);
			}
			Lootable lootable = go.GetComponent<Lootable> ();
			if (lootable) {
				//Clone the item asset
				lootable.item = (BaseItem)Instantiate(lootable.item);
				if(lootable.item is BonusItem){
					(lootable.item as BonusItem).RandomizeBonus();
				}
				GameObject itemName = (GameObject)Instantiate (GameManager.GamePrefabs.itemName, go.transform.position, Quaternion.identity);
				itemName.GetComponentInChildren<UILabel> ().text = "[" + lootable.item.stack.ToString () + "] " + lootable.item.itemName;
				
				itemName.transform.position = UnityTools.RandomPointInArea(go.transform.position,0);
				itemName.transform.parent = go.transform;
					
			}
			yield return new WaitForSeconds(delayBetweenSpawn);
		}
	}
}
