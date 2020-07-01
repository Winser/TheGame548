using UnityEngine;
using System.Collections;

/// <summary>
/// Target talent. Instantiates a projectile and applies damage to all AiBehaviour in view
/// </summary>
[System.Serializable]
public class TargetTalent : AreaOfEffectTalent {
	
	/// <summary>
	///  Use this talent 
	/// </summary>
	public override bool Use ()
	{
		//Can we use this talent at all?
		if(!base.Use ()){
			return false;
		}
		
		//Search for all AiBehaviour in view 
		AiBehaviour[] behaviours=(AiBehaviour[]) UnityTools.FindObjectsOfType<AiBehaviour>(GameManager.Player.transform,maxDistance,viewAngle,GameManager.Player.CharacterController.height*0.9f);
		foreach (AiBehaviour ai in behaviours) {
			//Check if the AiBehaviour is not dead
			if(!ai.Dead){
				//Instantiate the particle effect--> projectile at ai position
				UnityTools.StartCoroutine (InstantiateProjectile (instantiateDelay, ai.transform.position));
				//Apply damage to the AiBahaviour.
				UnityTools.StartCoroutine (ApplyDamage (instantiateDelay, ai));
			}
		}
		
		//Pvp mode on?
		if(GameManager.GameSettings.allowPvp){
			//Find all player in view
			PhotonNetworkPlayer[] players= (PhotonNetworkPlayer[]) UnityTools.FindObjectsOfType<PhotonNetworkPlayer>(GameManager.Player.transform,maxDistance,viewAngle,GameManager.Player.CharacterController.height*0.9f);
			//Apply damage to the found players over Network
			foreach(PhotonNetworkPlayer player in players){
				player.photonView.RPC ("ApplyDamage", player.photonView.owner, damageAttribute, (damage+GameManager.Player.GetAttribute (damageAttributeModifier).CurValue),defenceAttribute);
			}
		}
		return true;
	}
}
