using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Buff talent. Buffs the players attributes.
/// </summary>
[System.Serializable]
public class BuffTalent : BaseTalent {
	/// <summary>
	/// Projectile prefab
	/// </summary>
	public GameObject projectile;
	// Instantiate offset.
	public Vector3 projectileOffset= new Vector3(0,1.3f,0);
	// Should we instantiate after animation played?
	public bool instantiateAfterAnimation;
	//Or instantiate after seconds?
	public float instantiateAfterSeconds;
	/// <summary>
	/// Attribute to buff
	/// </summary>
	public string buffAttribute="Health";
	/// <summary>
	/// The buff value
	/// </summary>
	public float buff;
	/// <summary>
	/// Debuff after seconds.
	/// </summary>
	public float debuff;
	
	/// <summary>
	/// Use this talent.
	/// </summary>
	public override bool Use ()
	{
		//Can we use the talent at all?
		if(!base.Use ()){
			return false;
		}
		
		//Check the instantiate delay
		float instantiateDelay = instantiateAfterSeconds;
		if (instantiateAfterAnimation) {			
			instantiateDelay = animation.length;
		}
		//Get the attribute to buff.
		PlayerAttribute toBuff = GameManager.Player.GetAttribute (buffAttribute);
		//Instantiate projectile
		UnityTools.StartCoroutine (InstantiateProjectile (instantiateDelay, GameManager.Player.transform.position));
		//Buff the attribute
		toBuff.Buff ((int)buff);
		//Start the debuff
		UnityTools.StartCoroutine (toBuff.Debuff ((int)buff, debuff));
		
		return true;
	}
	
	/// <summary>
	/// Instantiates the projectile.
	/// </summary>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	/// <param name='position'>
	/// Position.
	/// </param>
	public IEnumerator InstantiateProjectile (float delay, Vector3 position)
	{
		yield return new WaitForSeconds(delay);
		PhotonNetwork.Instantiate (projectile.name, position + projectileOffset, GameManager.Player.transform.rotation, 0);
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
		buffAttribute=EditorGUILayout.TextField("Buff Attribute",buffAttribute);
		buff=EditorGUILayout.FloatField("Buff Value", buff);
		debuff=EditorGUILayout.FloatField("Debuff Delay",debuff);
	}
	#endif
}
