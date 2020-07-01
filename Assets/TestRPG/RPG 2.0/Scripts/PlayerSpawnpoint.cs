using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Simple script to spawn the player or to reset his position to this transforms
/// position after loading a new scene.
/// </summary>
public class PlayerSpawnpoint : MonoBehaviour {
	//Range from this transform to spawn the player
	[Range (0.0f, 100.0f)]
	public float range = 5.0f;
	
	void Start () {
		PhotonNetwork.isMessageQueueRunning = true;

		if(GameManager.Player == null){	
			if(!GameManager.GameDatabase.LoadGame()){
				GameObject player=PhotonNetwork.Instantiate(DataStorage.Instance.character.prefab.name,UnityTools.RandomPointInArea(transform.position,range)+Vector3.up,UnityTools.RandomQuaternion(Vector3.up,0,360),0);
				GameManager.Player= new Player(player.transform,DataStorage.Instance.character, DataStorage.Instance.playerName);	
				GameManager.Player.Checkpoint = UnityTools.RandomPointInArea(transform.position,range)+Vector3.up;
			}
			InterfaceContainer.Instance.interfaceWindow.BroadcastMessage("OnPlayerSpawn",SendMessageOptions.DontRequireReceiver);
		}else{
			//We switch the scene so no need to spawn the player, we only want to repositio him to this spawnpoint.
			GameManager.Player.transform.position = UnityTools.RandomPointInArea(transform.position,range)+ Vector3.up;
			//Set first checkpoint to the player spawnpoint.
			GameManager.Player.Checkpoint = UnityTools.RandomPointInArea(transform.position,range)+Vector3.up;
		}
	}
	
}
