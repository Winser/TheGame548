using UnityEngine;
using System.Collections;

public class Mount : MonoBehaviour {
	public Character character;
	public Animation mountAnimation;
	public Transform bone;
	[HideInInspector]
	public ThirdPersonMovement movement;
	[HideInInspector]
	public Character playerCharacter;
	
	private void OnDestroy(){
		if(!PhotonNetwork.offlineMode && !PhotonView.Get(transform).isMine){
			movement.playerAnimation.transform.parent= movement.transform;
			movement.playerAnimation.transform.localPosition=Vector3.zero;
			movement.playerAnimation.transform.localRotation=Quaternion.Euler(Vector3.zero);
			movement.character=playerCharacter;
		}
	}
}
