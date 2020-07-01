using UnityEngine;
using System.Collections;

/// <summary>
/// This script should be attached to the projectile missle.So we can sync the position.
/// </summary>
public class Missle : Photon.MonoBehaviour
{
	//How fast and in which direction should the missle move?
	public Vector3 speed = new Vector3 (0, 0, 4);
	//The talent used to cast this projectile
	[HideInInspector]
	public MissleTalent talent;
	//Ai will be not null if the missle was instantiated by ai.
	[HideInInspector]
	public AiBehaviour ai;
	
	private void Start(){
		
		Missle[] missles=GetComponentsInChildren<Missle>();
		foreach(Missle missle in missles){
			missle.talent=talent;
			if(ai != null){
				missle.ai=ai;
			}else{
				Physics.IgnoreCollision(GameManager.Player.CharacterController,missle.GetComponent<Collider>());
				Physics.IgnoreCollision(GameManager.Player.transform.GetComponent<CapsuleCollider>(),missle.GetComponent<Collider>());
			}
			missle.transform.parent=null;
		}
	}
	
	void Update ()
	{
		//Move the projectile
		transform.Translate (speed * Time.deltaTime);

		//Check if projectile reaches the maximum distance --> destroy it.
		if (photonView.isMine && talent && Vector3.Distance (GameManager.Player.transform.position, transform.position) > talent.maxDistance) {
			PhotonNetwork.Destroy (gameObject);
		}
	}
	
	//Projectile hits something
	private void OnTriggerEnter (Collider other)
	{
		//Check if the missle does not hit a checkpoint trigger collider and if talent is not null
		if(!other.tag.Equals(GameManager.GameSettings.checkpointTag) && talent && !other.tag.Equals(tag)){
			//Check if missle was instantiated by ai 
			if(ai){
				//Call OnProjectileHit, because we hit something.
				talent.OnProjectileHit(other.gameObject,ai);
			}else{
				//Call OnProjectileHit, because we hit something.
				talent.OnProjectileHit(other.gameObject);
			}
			//Destroy our projectile
			PhotonNetwork.Destroy(gameObject);
		}
	}

}
