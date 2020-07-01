using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Base class for projectile talents
/// </summary>
[System.Serializable]
public class ProjectileTalent : DamageTalent {
	//Projectile prefab
	public GameObject projectile;
	//Instantiate offset
	public Vector3 projectileOffset= new Vector3(0,1.3f,0);
	//Should we instantiate after animation played?
	public bool instantiateAfterAnimation;
	//Or instantiate after seconds?
	public float instantiateAfterSeconds;
	
	//Variable to findout the instantiate time in derived classes.
	[System.NonSerialized]
	protected float instantiateDelay;
	
	/// <summary>
	/// Use this talent
	/// </summary>
	public override bool Use ()
	{
		//Can we use this talent at all?
		if(!base.Use ()){
			return false;
		}
		
		//Check the instantiate time
		instantiateDelay = instantiateAfterSeconds;
		if (instantiateAfterAnimation) {			
			instantiateDelay = animation.length;
		}
		
		//Should we look at our target?
		if(lookAtTarget && GameManager.Player.TargetBehaviour && Vector3.Distance(GameManager.Player.transform.position,GameManager.Player.TargetBehaviour.transform.position)< maxDistance && !GameManager.Player.TargetBehaviour.Dead){
			GameManager.Player.transform.LookAt (new Vector3 (GameManager.Player.TargetBehaviour.transform.position.x, GameManager.Player.transform.position.y, GameManager.Player.TargetBehaviour.transform.position.z));
		}
		return true;
	}
	
	/// <summary>
	/// Use this talent
	/// </summary>
	/// <param name='ai'>
	/// AiBehaviour
	/// </param>
	public override bool Use (AiBehaviour ai)
	{
		//Can we use this talent?
		if(!base.Use (ai)){
			return false;
		}
		
		//Find the instantiate time
		instantiateDelay = instantiateAfterSeconds;
		if (instantiateAfterAnimation) {			
			instantiateDelay = animation.length;
		}
		
		//Should we look at our target player?
		if(lookAtTarget){
			ai.transform.LookAt (new Vector3 (ai.target.position.x, ai.transform.position.y, ai.target.position.z));
		}
		return true;
	}
	
	/// <summary>
	/// Instantiates the projectile at player position + offset
	/// </summary>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	public virtual IEnumerator InstantiateProjectile (float delay){
		yield return new WaitForSeconds(delay);
		PhotonNetwork.Instantiate (projectile.name, GameManager.Player.transform.position+ projectileOffset, GameManager.Player.transform.rotation, 0);
	}
	
	/// <summary>
	/// Instantiates the projectile at position + offset
	/// </summary>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	/// <param name='position'>
	/// Position.
	/// </param>
	public virtual IEnumerator InstantiateProjectile (float delay, Vector3 position){
		yield return new WaitForSeconds(delay);
		PhotonNetwork.Instantiate (projectile.name, position+ projectileOffset, GameManager.Player.transform.rotation, 0);
	}
	
	/// <summary>
	/// Instantiates the projectile at AiBehaviour position + offset
	/// </summary>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	/// <param name='ai'>
	/// AiBehaviour
	/// </param>
	public virtual IEnumerator InstantiateProjectile(float delay, AiBehaviour ai){
		yield return new WaitForSeconds(delay);
		PhotonNetwork.Instantiate(projectile.name,ai.transform.position+ projectileOffset,ai.transform.rotation,0);
	}
	
	/// <summary>
	/// Raises the projectile hit event.
	/// </summary>
	/// <param name='hit'>
	/// Hit gameObject
	/// </param>
	public virtual void OnProjectileHit(GameObject hit){
		AiBehaviour behaviour= hit.GetComponent<AiBehaviour>();
		if(behaviour){
			//Apply damage to the behaviour
			UnityTools.StartCoroutine(ApplyDamage(0,behaviour));
		}
		//Is pvp allowed and our hit object is remote player?
		if(GameManager.GameSettings.allowPvp && hit.tag.Equals(GameManager.PlayerSettings.remotePlayerTag)){
			//Yes apply damage to the remote player
			UnityTools.StartCoroutine(ApplyDamage(0,hit.GetComponent<PhotonNetworkPlayer>(),(int)GameManager.Player.GetAttribute (damageAttributeModifier).CurValue));
		}
	}
	
	/// <summary>
	/// Raises the projectile hit event. Projectile was instantiated by npc
	/// </summary>
	/// <param name='hit'>
	/// Hit gameObjeact
	/// </param>
	/// <param name='behaviour'>
	/// Behaviour that instantiated the projectile.
	/// </param>
	public virtual void OnProjectileHit(GameObject hit, AiBehaviour behaviour){
		if(hit.tag.Equals(GameManager.PlayerSettings.playerTag) || hit.tag.Equals(GameManager.PlayerSettings.remotePlayerTag)){
			//Are we in multiplayer mode?
			if(!PhotonNetwork.offlineMode){
				PhotonView.Get(hit).RPC("ApplyDamage", PhotonView.Get(hit).owner,damageAttribute,damage,defenceAttribute);
			}else{
				// Apply damage to the local player
				if(GameManager.PlayerSettings.shakeCamera){
					PlayerCamera.Instance.Shake (0.2f, 0.03f);
				}
				UnityTools.StartCoroutine(ApplyDamage(0));
			}
			GameManager.Player.PlaySound(GameManager.Player.Character.getHitSound);
		}
	}
	
	#if UNITY_EDITOR	
	public override void OnGUI(){
		base.OnGUI();
		projectile=(GameObject)EditorGUILayout.ObjectField("Projectile",projectile,typeof(GameObject),false);
		projectileOffset=EditorGUILayout.Vector3Field("Projectile offset",projectileOffset);
		instantiateAfterAnimation=EditorGUILayout.Toggle("Instantiate after animation",instantiateAfterAnimation);
		if(!instantiateAfterAnimation){
			instantiateAfterSeconds=EditorGUILayout.FloatField("Instantiate after seconds", instantiateAfterSeconds);
		}
	}
	#endif
}
