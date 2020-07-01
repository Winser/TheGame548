using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Base class for mount talents
/// </summary>
[System.Serializable]
public class MountTalent : BaseTalent {
	//Particle effect prefab, instantiated before instantiating the mount
	public GameObject projectile;
	//Projectile offset
	public Vector3 projectileOffset= new Vector3(0,1.3f,0);
	//Instantiate Projectile after animation played?
	public bool instantiateAfterAnimation;
	//Or after some seconds?
	public float instantiateAfterSeconds;
	//Mount prefab
	public GameObject mount;
	
	/// <summary>
	/// Use this talent.
	/// </summary>
	public override bool Use ()
	{
		//Is the player already mounted and can we use this talent at all?
		if(GameManager.Player.IsMounted || !base.Use ()){
			return false;
		}
		
		//Find the instantiate delay
		float instantiateDelay = instantiateAfterSeconds;
		if (instantiateAfterAnimation) {			
			instantiateDelay = animation.length;
		}
		
		//Start instantiating projectile
		UnityTools.StartCoroutine (InstantiateProjectile (instantiateDelay, GameManager.Player.transform.position));
		//Instantiate mount after the animation has played.
		UnityTools.StartCoroutine (InstantiateMount (animation.length));
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
	
	/// <summary>
	/// Instantiates the mount.
	/// </summary>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	public IEnumerator InstantiateMount(float delay)
	{
		yield return new WaitForSeconds(delay);
		GameObject go= PhotonNetwork.Instantiate(mount.name,GameManager.Player.transform.position,GameManager.Player.transform.rotation,0);
		//Mount the player.
		GameManager.Player.Mount(go.transform);
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
		mount=(GameObject)EditorGUILayout.ObjectField("Mount prefab",mount,typeof(GameObject),false);
	}
	#endif
}
