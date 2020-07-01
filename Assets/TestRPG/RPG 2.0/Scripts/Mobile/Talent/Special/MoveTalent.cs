using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class MoveTalent : DamageTalent {
	public AnimationClip moveAnimation;
	public float moveAnimationSpeed=1.0f;
	public float moveSpeed=5.0f;
	public float maxMoveDistance=15.0f;
	
	public override bool Use ()
	{
		//Check if player is dead, spent points on this talent is less or equal null or if the delay is not over. 
		//If one of those conditions fail, we return and can not use the talent.
		if(GameManager.Player.Dead || spentPoints <= 0 || !canUse || GameManager.Player.IsMounted){
			return false;
		}
		
		//Get the required Attribute to use this talent
		PlayerAttribute requiredAttribute= GameManager.Player.GetAttribute(attribute);
		
		//Attribute value is less then needed -> return
		if(requiredAttribute.CurValue < attributeValue){
			return false;
		}
		//Substract the required value from the Attribute
		requiredAttribute.ApplyDamage(attributeValue);
		//Start the usage delay
		UnityTools.StartCoroutine(Delay());
		
		//If we have not targeted any AiBehaviour or if our target does not fullfill the requirements--> search for AiBehaviour.
		AiBehaviour behaviour = (GameManager.Player.TargetBehaviour != null && Vector3.Distance(GameManager.Player.TargetBehaviour.transform.position,GameManager.Player.transform.position)<maxDistance && !GameManager.Player.TargetBehaviour.Dead)?GameManager.Player.TargetBehaviour: UnityTools.FindObjectOfType<AiBehaviour>(GameManager.Player.transform,maxDistance,viewAngle,GameManager.Player.CharacterController.height*0.8f); 
		
		//We have found one Aibehaviour
		if (behaviour) {
			//Should we look at our target?
			if(lookAtTarget){
				//Look at target
				GameManager.Player.transform.LookAt (new Vector3 (behaviour.transform.position.x, GameManager.Player.transform.position.y, behaviour.transform.position.z));
			}
		}
		
		GameObject handler= new GameObject("MoveTalentHandler");
		MoveTalentInstance instance=handler.AddComponent<MoveTalentInstance>();
		instance.Initialize(this);
		GameManager.Player.Movement.StopCoroutine("KillInput");
		GameManager.Player.Movement.StartCoroutine ("KillInput" ,killInput);
		
		return true;
	}
	
	#if UNITY_EDITOR
	public override void OnGUI ()
	{
		base.OnGUI ();
		moveAnimation=(AnimationClip)EditorGUILayout.ObjectField("Move Animation",moveAnimation,typeof(AnimationClip),false);
		moveAnimationSpeed=EditorGUILayout.FloatField("Move Animation Speed",moveAnimationSpeed);
		moveSpeed=EditorGUILayout.FloatField("Move Speed",moveSpeed);
	}
	#endif
}

public class MoveTalentInstance: MonoBehaviour{
	private CharacterController controller;
	private Animation playerAnimation;
	private MoveTalent talent;
	private AiBehaviour ai;
	private Vector3 startPos;
	
	public void Initialize(MoveTalent talent){
		this.controller=GameManager.Player.CharacterController;
		this.startPos=controller.transform.position;
		this.playerAnimation=GameManager.Player.animation;
		this.talent=talent;
	
		this.playerAnimation[talent.moveAnimation.name].speed=talent.moveAnimationSpeed;
		
		//If we have not targeted any AiBehaviour or if our target does not fullfill the requirements--> search for AiBehaviour.
		ai = (GameManager.Player.TargetBehaviour != null && Vector3.Distance(GameManager.Player.TargetBehaviour.transform.position,GameManager.Player.transform.position)<talent.maxMoveDistance && !GameManager.Player.TargetBehaviour.Dead)?GameManager.Player.TargetBehaviour: UnityTools.FindObjectOfType<AiBehaviour>(GameManager.Player.transform,talent.maxMoveDistance,talent.viewAngle,GameManager.Player.CharacterController.height*0.8f); 
		
	}
	
	
	private void Update(){
		if(Vector3.Distance(controller.transform.position,startPos)> talent.maxMoveDistance){
			playerAnimation[talent.moveAnimation.name].layer=0;
			GameManager.Player.Movement.PlayAnimation(talent.animation.name,talent.animation.length-talent.animation.length*0.1f,talent.animationSpeed);
			Destroy(gameObject);
			return;
		}
		
		//We have found one Aibehaviour
		if (ai) {
			//Should we look at our target?
			if(talent.lookAtTarget){
				//Look at target
				GameManager.Player.transform.LookAt (new Vector3 (ai.transform.position.x, GameManager.Player.transform.position.y, ai.transform.position.z));
			}
			
			if(Vector3.Distance(ai.transform.position,controller.transform.position)< talent.maxDistance){
				playerAnimation[talent.moveAnimation.name].layer=0;
				GameManager.Player.Movement.PlayAnimation(talent.animation.name,talent.animation.length-talent.animation.length*0.1f,talent.animationSpeed);
				
				UnityTools.StartCoroutine( talent.ApplyDamage(talent.animation.length*0.5f,ai));
				Destroy(gameObject);
				return;
			}
		}
		
		Vector3 direction = controller.transform.TransformDirection (Vector3.forward);
		playerAnimation[talent.moveAnimation.name].layer=4;
		playerAnimation.CrossFade(talent.moveAnimation.name);
		controller.SimpleMove(direction*talent.moveSpeed);
	}
}