using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Base talent class
/// </summary>
[System.Serializable]
public class BaseTalent : ScriptableObject {
	#region GeneralSettings
	/// <summary>
	/// The name of the talent.
	/// </summary>
	public string talentName;
	/// <summary>
	/// The description of the talent. Shown in the talent window.
	/// </summary>
	public string description;
	/// <summary>
	/// The animation we should play on usage.
	/// </summary>
	public AnimationClip animation;
	/// <summary>
	/// How fast should the animation be played.
	/// </summary>
	public float animationSpeed=1.0f;
	/// <summary>
	/// The icon of the talent, icons should be placed in the Interface Atlas 
	/// </summary>
	public string icon;
	/// <summary>
	/// The override talent attribute.
	/// </summary>
	public string overrideTalentAttribute="Power";
	/// <summary>
	/// If the overrideTalentAttribute is full we will use this talent instead.
	/// </summary>
	public BaseTalent overrideOnFullAttribute;
	/// <summary>
	/// This value will be added to the attribute after using the talent.
	/// </summary>
	public float addOverrideTalentValue=5;
	/// <summary>
	/// The sound.
	/// </summary>
	public AudioClip sound;
	/// <summary>
	/// The sound delay.
	/// </summary>
	public float soundDelay;
	#endregion GeneralSettings
	
	#region Delay
	/// <summary>
	/// Kill player input in seconds
	/// </summary>
	public float killInput=1.0f;
	/// <summary>
	/// The cool down of the talent
	/// </summary>
	public float coolDown=1.0f;
	#endregion Delay
	
	#region Requirements
	/// <summary>
	/// The minimum level required to use this talent.
	/// </summary>
	public int minLevel;
	/// <summary>
	/// The required Attribute.
	/// </summary>
	public string attribute="Mana";
	/// <summary>
	/// The attribute value to substract from the player on usage.
	/// </summary>
	public int attributeValue;
	/// <summary>
	/// The view angle to find AiBehaviour.
	/// </summary>
	public float viewAngle=180;
	#endregion Requirements
	
	#region Runtime
	/// <summary>
	/// The spent points on this talent
	/// </summary>
	[System.NonSerialized]
	public int spentPoints;
	/// <summary>
	/// Can we use the talent again after the delay.
	/// </summary>
	[System.NonSerialized]
	protected bool canUse=true;
	
	/// <summary>
	/// Use this talent.
	/// </summary>
	public virtual bool Use(){
		//Check if player is dead, spent points on this talent is less or equal null or if the delay is not over. 
		//If one of those conditions fail, we return and can not use the talent.
		if(GameManager.Player.Dead || spentPoints <= 0 || !canUse || GameManager.Player.IsMounted){
			return false;
		}
		
		PlayerAttribute power= GameManager.Player.GetAttribute(overrideTalentAttribute);
		if(power != null && power.CurValue >= power.BaseValue && overrideOnFullAttribute != null){
			power.ApplyDamage(power.CurValue);
			overrideOnFullAttribute.spentPoints=1;
			overrideOnFullAttribute.Use();
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
		//Play the talent animation
		GameManager.Player.Movement.PlayAnimation (animation.name, killInput,animationSpeed);
		
		//coolDown should not be lower then animation length
		if(coolDown<animation.length){
			coolDown=animation.length;
		}
		//Start the usage delay
		UnityTools.StartCoroutine(Delay());
		
		if(power != null){
			power.HealDamage(addOverrideTalentValue);
		}
		UnityTools.StartCoroutine(PlaySound());
		return true;
	}
	
	/// <summary>
	/// Use this talent
	/// </summary>
	/// <param name='ai'>
	/// Aibehavior
	/// </param>
	public virtual bool Use(AiBehaviour ai){
		if(ai.Dead || !canUse){
			return false;
		}
		ai.StopAgent();
		ai.GetComponent<Animation>()[animation.name].speed=animationSpeed;
		ai.GetComponent<Animation>().CrossFade(animation.name);
		if(!PhotonNetwork.offlineMode){
			ai.photonView.RPC("PlayAnimation",PhotonTargets.Others,animation.name);
		}
		ai.StartCoroutine(Delay());
		return true;		
	}
	
	/// <summary>
	/// Delay this talent.
	/// </summary>
	public IEnumerator Delay(){
		canUse=false;
		yield return new WaitForSeconds(coolDown);
		canUse=true;
		
	}
	
	public IEnumerator PlaySound(){
		yield return new WaitForSeconds(soundDelay);
		
		if(GameManager.Player.AudioSource){
			GameManager.Player.AudioSource.clip=sound;
			GameManager.Player.AudioSource.Play();
		}
	}
	#endregion Runtime
	
	#if UNITY_EDITOR	
	public virtual void OnGUI(){
		talentName = EditorGUILayout.TextField("Name",talentName);
		description = EditorGUILayout.TextField("Description",description);
		animation= (AnimationClip)EditorGUILayout.ObjectField("Animation",animation,typeof(AnimationClip),false);
		animationSpeed=EditorGUILayout.FloatField("Animation Speed",animationSpeed);
		icon=EditorGUILayout.TextField("Icon",icon);
		overrideTalentAttribute=EditorGUILayout.TextField("Override Talent Attribute",overrideTalentAttribute);
		overrideOnFullAttribute=(BaseTalent)EditorGUILayout.ObjectField("Override On Full Attribute", overrideOnFullAttribute,typeof(BaseTalent),false);
		addOverrideTalentValue=EditorGUILayout.FloatField("Add Override Talent Value",addOverrideTalentValue);
		sound=(AudioClip)EditorGUILayout.ObjectField("Sound", sound, typeof(AudioClip),false);
	}
	#endif
}
