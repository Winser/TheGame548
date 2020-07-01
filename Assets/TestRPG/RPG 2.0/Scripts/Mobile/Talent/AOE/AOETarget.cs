using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AOETarget : MonoBehaviour
{
	private static AOETarget instance;
	public static AOETarget Instance{
		get{return instance;}
	}
	
	public LayerMask mask;
	[HideInInspector]
	public GiveTargetTalent talent;

	private void Awake(){
		instance=this;
	}
	
	private void Start ()
	{
		gameObject.SetActive (false);
	}
	
	void Update ()
	{
		if (GameManager.Player.Dead) {
			gameObject.SetActive (false);
			return;
		}
		
		if(GameManager.Player.Movement.controllerType== ThirdPersonMovement.ControllerType.ClickToMove){
			GameManager.Player.Movement.Stop();
		}
		
		transform.localScale = new Vector3 (talent.aoeRange / 10, 1, talent.aoeRange / 10);
		RaycastHit hit;
		Vector3 pos = transform.position;
		if (Physics.Raycast (PlayerCamera.Instance.GetComponent<Camera>().ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity, mask)) {
			pos = hit.point;
		}
		Vector3 diff = pos - GameManager.Player.transform.position;
		float distance = diff.magnitude;
		if (distance > (talent.maxDistance - (transform.localScale.x / 2))) {
			transform.position = GameManager.Player.transform.position + (diff / distance) * talent.maxDistance;
		} else {
			transform.position = pos;
		}
		
		
		transform.Rotate (Vector3.up);
		
		if(!PlayerCamera.Instance.isInFirstPersonView){
			if(GameManager.Player.IsMounted){
				GameManager.Player.MountTransform.LookAt (new Vector3 (transform.position.x, GameManager.Player.MountTransform.position.y, transform.position.z));
			}else{
				GameManager.Player.transform.LookAt (new Vector3 (transform.position.x, GameManager.Player.transform.position.y, transform.position.z));
			}
		}
		if (!talent.canWalk) {
			GameManager.Player.Movement.killInput = true;
		}
		if (Input.GetMouseButtonDown (0)) {
			GameManager.Player.Movement.PlayAnimation (talent.animation.name, talent.killInput);
			
			AiBehaviour[] inRangeBehaviour = (AiBehaviour[])UnityTools.FindObjectsOfType<AiBehaviour>(transform.position,talent.aoeRange);
			
			foreach (AiBehaviour behaviour in inRangeBehaviour) {
				UnityTools.StartCoroutine(talent.InstantiateProjectile(0,behaviour.transform.position));
				UnityTools.StartCoroutine (talent.ApplyDamage (0, behaviour));
			}
			
			if (inRangeBehaviour.Length == 0) {
				UnityTools.StartCoroutine(talent.InstantiateProjectile(0,transform.position));
			}
			
			if (GameManager.GameSettings.allowPvp) {
				GameObject[] remotePlayers = UnityTools.FindGameObjectsWithTag(transform.position,talent.aoeRange,GameManager.PlayerSettings.remotePlayerTag);
				float damage=talent.damage + GameManager.Player.GetAttribute (talent.damageAttributeModifier).CurValue;
				foreach (GameObject go in remotePlayers) {
					PhotonView.Get(go).RPC ("ApplyDamage", PhotonView.Get (go).owner, talent.damageAttribute, (int)damage,talent.defenceAttribute);
				}
			
			}
			
			gameObject.SetActive (false);
			
		}

		if (Input.GetMouseButtonDown (1)) {
			gameObject.SetActive (false);
		}
	}
	
	private void OnEnable(){
		SlotContainer.disableMouseTalent=true;
	}
	
	private void OnDisable(){
		SlotContainer.disableMouseTalent=false;
	}
}
