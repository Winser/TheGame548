using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleporterManager : MonoBehaviour {
	private static TeleporterManager instance;
	public static TeleporterManager Instance{
		get{return instance;}
	}
	public List <Teleporter> startTeleporters;
	
	public Dictionary<TeleporterSlot, Vector3> teleporters= new Dictionary<TeleporterSlot, Vector3>();
	
	[HideInInspector]
	public bool teleported;
	
	private void Awake(){
		instance=this;
	}
	
	public void Start(){
		TeleporterTrigger[] triggers= (TeleporterTrigger[])GameObject.FindObjectsOfType(typeof(TeleporterTrigger));
		foreach(TeleporterTrigger trigger in triggers){
			if(startTeleporters.Contains(trigger.teleporter) && !ContainsLocation(trigger.teleporter)){
				AddToTable(trigger.teleporter,trigger.transform.position);
			}
		}
	}
	
	public void AddToTable(Teleporter teleporter, Vector3 position){
		GameObject go=NGUITools.AddChild(InterfaceContainer.Instance.teleporterTable.gameObject,GameManager.GamePrefabs.teleporter);
		TeleporterSlot slot=go.GetComponent<TeleporterSlot>();
		slot.locationName.text=teleporter.teleporterName;
		InterfaceContainer.Instance.teleporterTable.Reposition();
		teleporters.Add(slot,position);
		
	}
	
	public bool ContainsLocation(Teleporter teleporter){
		foreach(KeyValuePair<TeleporterSlot,Vector3> kvp in teleporters){
			if(kvp.Key.locationName.text.Equals(teleporter.teleporterName)){
				return true;
			}
		}
		return false;
	}
	
	public void SetCurrentLocation(Teleporter teleporter){
		foreach(KeyValuePair<TeleporterSlot,Vector3> kvp in teleporters){
			if(kvp.Key.locationName.text.Equals(teleporter.teleporterName)){
				kvp.Key.currentLocationSprite.gameObject.SetActive(true);
			}else{
				kvp.Key.currentLocationSprite.gameObject.SetActive(false);
			}
		}
	}
	
	public Vector3 GetLocation(string locationName){
		foreach(KeyValuePair<TeleporterSlot,Vector3> kvp in teleporters){
			if(kvp.Key.locationName.text.Equals(locationName)){
				return kvp.Value;
			}
		}
		return Vector3.zero;
	}
	
	public Vector3 GetLocation(TeleporterSlot slot){
		if(teleporters.ContainsKey(slot)){
			return teleporters[slot];
		}
		return Vector3.zero;
	}
	
	
	public void Teleport(TeleporterSlot slot){
		StartCoroutine(StartTeleport(slot));
	}
	
	private IEnumerator StartTeleport(TeleporterSlot slot){
		Vector3 loc=GetLocation(slot);
		RaycastHit hit;
		if(Physics.Raycast(loc+Vector3.up,Vector3.down,out hit)){
			teleported=true;
			GameManager.Player.transform.position=hit.point;
			GameManager.Player.Movement.Stop();
		}
		
		yield return new WaitForSeconds(0.1f);
		teleported=false;
	}
}
