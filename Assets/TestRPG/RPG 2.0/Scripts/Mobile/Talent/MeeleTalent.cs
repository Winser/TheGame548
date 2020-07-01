using UnityEngine;
using System.Collections;

/// <summary>
/// Meele talent.
/// </summary>
[System.Serializable]
public class MeeleTalent : DamageTalent
{
	/// <summary>
	/// Use this talent
	/// </summary>
	public override bool Use ()
	{
		//Can we use this talent at all?
		if (!base.Use ()) {
			return false;
		}
		
		//Is pvp allowed?
		if (GameManager.GameSettings.allowPvp) {
			//Find the player
			PhotonNetworkPlayer remotePlayer = GameManager.Player.Target != null?GameManager.Player.Target.GetComponent<PhotonNetworkPlayer>():null;
			if(remotePlayer== null){
				remotePlayer = UnityTools.FindObjectOfType<PhotonNetworkPlayer>(GameManager.Player.transform,maxDistance,viewAngle,GameManager.Player.CharacterController.height*0.9f);
			}
			if (remotePlayer != null && !remotePlayer.photonView.isMine) {
				//Apply damage 
				UnityTools.StartCoroutine (ApplyDamage (animation.length / 2, remotePlayer,(int)GameManager.Player.GetAttribute (damageAttributeModifier).CurValue));
			}
		} 
		
		//If we have not targeted any AiBehaviour or if our target does not fullfill the requirements--> search for AiBehaviour.
		AiBehaviour behaviour = (GameManager.Player.TargetBehaviour != null && Vector3.Distance(GameManager.Player.TargetBehaviour.transform.position,GameManager.Player.transform.position)<maxDistance && !GameManager.Player.TargetBehaviour.Dead)?GameManager.Player.TargetBehaviour: UnityTools.FindObjectOfType<AiBehaviour>(GameManager.Player.transform,maxDistance,viewAngle,GameManager.Player.CharacterController.height*0.8f); 
		
		//We have found one Aibehaviour
		if (behaviour) {
			//Look at target
			LookAtTarget(behaviour.transform);
			//Apply damage to the AiBehaviour
			UnityTools.StartCoroutine (ApplyDamage (animation.length / 2, behaviour));	
		}
		return true;
	}
	
	/// <summary>
	/// Use this talent.
	/// </summary>
	/// <param name='ai'>
	/// AiBehaviour that uses this talent
	/// </param>
	public override bool Use (AiBehaviour ai)
	{
		//Can we use this talent?
		if(!base.Use (ai)){
			return false;
		}
		return true;
	}

}
