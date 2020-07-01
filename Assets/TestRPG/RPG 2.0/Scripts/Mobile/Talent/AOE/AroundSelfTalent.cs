using UnityEngine;
using System.Collections;

/// <summary>
/// Around self talent. Applies damage to all AiBehaviour arround the player.
/// </summary>
[System.Serializable]
public class AroundSelfTalent : AreaOfEffectTalent {
	
	/// <summary>
	///  Use this talent 
	/// </summary>
	public override bool Use ()
	{
		//Check if we can use the talent at all
		if(!base.Use ()){
			return false;
		}
		//Instantiate the particle effect
		UnityTools.StartCoroutine (InstantiateProjectile (instantiateDelay));
		//Search for all AiBehaviour around player
		AiBehaviour[] behaviours= (AiBehaviour[])UnityTools.FindObjectsOfType<AiBehaviour>(GameManager.Player.transform.position,maxDistance);
		
		//Apply damage to all AiBehaviours
		foreach (AiBehaviour ai in behaviours) {
			UnityTools.StartCoroutine (ApplyDamage (instantiateDelay, ai));
		}
		
		//Pvp mode on?
		if(GameManager.GameSettings.allowPvp){
			//Yes pvp mode is on, search for all players around self
			PhotonNetworkPlayer[] players=(PhotonNetworkPlayer[])UnityTools.FindObjectsOfType<PhotonNetworkPlayer>(GameManager.Player.transform.position,maxDistance);
			foreach(PhotonNetworkPlayer player in players){
				//player is not self
				if(!player.photonView.isMine){
					//Apply damage over Network
					UnityTools.StartCoroutine(ApplyDamage(0,player,(int)GameManager.Player.GetAttribute (damageAttributeModifier).CurValue));
				}
			}
		}
		return true;
	}
	
	public override bool Use (AiBehaviour ai)
	{
		if(!base.Use (ai)){
			return false;
		}
		//Instantiate the particle effect
		ai.StartCoroutine (InstantiateProjectile (instantiateDelay,ai));
		
		if(PhotonNetwork.offlineMode){
			ai.StartCoroutine(ApplyDamage(0.3f));
		}else{
			PhotonNetworkPlayer[] players=(PhotonNetworkPlayer[])UnityTools.FindObjectsOfType<PhotonNetworkPlayer>(ai.transform.position,maxDistance);
			foreach(PhotonNetworkPlayer player in players){
				UnityTools.StartCoroutine(ApplyDamage(0.3f,player,0));
			}
		}
		return true;
	}
}
