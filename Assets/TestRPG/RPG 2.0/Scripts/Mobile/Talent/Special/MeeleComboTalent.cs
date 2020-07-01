using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MeeleComboTalent : MeeleTalent {
	public List<AnimationClip> animations;
	
	public override bool Use ()
	{
		if(!base.Use()){
			return false;
		}
		
		UnityTools.StartCoroutine(GameManager.Player.Movement.PlayAnimation(animations,animation.length-animation.length*0.1f,killInput,animationSpeed));
		//If we have not targeted any AiBehaviour or if our target does not fullfill the requirements--> search for AiBehaviour.
		AiBehaviour behaviour = (GameManager.Player.TargetBehaviour != null && Vector3.Distance(GameManager.Player.TargetBehaviour.transform.position,GameManager.Player.transform.position)<maxDistance && !GameManager.Player.TargetBehaviour.Dead)?GameManager.Player.TargetBehaviour: UnityTools.FindObjectOfType<AiBehaviour>(GameManager.Player.transform,maxDistance,viewAngle,GameManager.Player.CharacterController.height*0.8f); 
		if(behaviour){
			float t=animation.length;
			for(int i=0;i<animations.Count;i++){
				t+=animations[i].length/2;
				UnityTools.StartCoroutine(ApplyDamage(t,behaviour));
				t+=animations[i].length/2;
			}
		}
		return true;
	}
}
