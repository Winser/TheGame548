using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Missle talent.
/// </summary>
[System.Serializable]
public class MissleTalent : ProjectileTalent {
	/// <summary>
	/// The speed and diretion of missle
	/// </summary>
	public Vector3 speed=new Vector3(0,0,8);
	
	/// <summary>
	///  Use this talent 
	/// </summary>
	public override bool Use ()
	{
		//Can we use this talent at all?
		if(!base.Use ()){
			return false;
		}
		
		//Instantiate the projectile after instantiateDelay
		UnityTools.StartCoroutine (InstantiateProjectile (instantiateDelay));
		return true;
	}
	
	public override bool Use (AiBehaviour ai)
	{
		if(!base.Use(ai)){
			return false;
		}
		ai.StartCoroutine(InstantiateProjectile(instantiateDelay,ai));
		return true;
	}
	
	/// <summary>
	/// Instantiates the projectile. We override this method to setup the Missle script.
	/// </summary>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	public override IEnumerator InstantiateProjectile (float delay)
	{
		yield return new WaitForSeconds(delay);
		GameObject go=PhotonNetwork.Instantiate (projectile.name, GameManager.Player.transform.position+ projectileOffset, GameManager.Player.transform.rotation, 0);
		Missle missle= go.GetComponent<Missle>();
		if(!missle){
			missle= go.AddComponent<Missle>();
		}
		Physics.IgnoreCollision(GameManager.Player.transform.GetComponent<Collider>(),missle.GetComponent<Collider>());
		missle.talent=this;
		missle.speed=speed;
	}
	
	/// <summary>
	/// Instantiates the projectile.
	/// </summary>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	/// <param name='ai'>
	/// AiBehaviour
	/// </param>
	public override IEnumerator InstantiateProjectile (float delay, AiBehaviour ai)
	{
		yield return new WaitForSeconds(delay);
		GameObject go=PhotonNetwork.Instantiate(projectile.name,ai.transform.position+ projectileOffset,ai.transform.rotation,0);
		Missle missle= go.GetComponent<Missle>();
		if(!missle){
			missle= go.AddComponent<Missle>();
		}
		Physics.IgnoreCollision(ai.GetComponent<Collider>(),missle.GetComponent<Collider>());
		missle.talent=this;
		missle.speed=speed;
		missle.ai= ai;
	}
	
	#if UNITY_EDITOR	
	public override void OnGUI(){
		base.OnGUI();
		speed=EditorGUILayout.Vector3Field("Speed",speed);
	}
	#endif
}
