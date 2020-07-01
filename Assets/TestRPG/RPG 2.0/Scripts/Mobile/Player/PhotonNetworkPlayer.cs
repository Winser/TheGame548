using UnityEngine;
using System.Collections;

/// <summary>
/// Class to sync player actions
/// </summary>
public class PhotonNetworkPlayer : Photon.MonoBehaviour {
	public UILabel nameLabel;
	private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
	private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this
	private CharacterState curState=CharacterState.Idle;
	private ThirdPersonMovement movement;
	[HideInInspector]
	public bool dead=false;
	private float characterHeight;
	
	private void Awake(){
		characterHeight=GetComponent<CharacterController>().height;
		if (!photonView.isMine) {
			transform.tag = GameManager.PlayerSettings.remotePlayerTag;
			gameObject.layer=0;
			Destroy (GetComponent<DontDestroyOnLevelChange> ());
			movement=GetComponent<ThirdPersonMovement>();
			movement.enabled=false;
			movement.SetupEvadeAnimations();
			movement.playerAnimation[movement.character.die.name].wrapMode=WrapMode.ClampForever;
			movement.playerAnimation[movement.character.die.name].layer=6;
		} 
	}
	
	private void Update(){
		if (!photonView.isMine) {
			transform.position = Vector3.Lerp (transform.position, correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp (transform.rotation, correctPlayerRot, Time.deltaTime * 5);
			movement.HandleCharacterState (curState);
		}
	}
	
	private void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			//We own this player: send the others our data
			stream.SendNext (GameManager.Player.transform.position);
			stream.SendNext (Quaternion.Euler(0,GameManager.Player.transform.rotation.eulerAngles.y,0));
			stream.SendNext ((int)GameManager.Player.Movement.curState);
		} else {
			//Network player, receive data
			correctPlayerPos = (Vector3)stream.ReceiveNext ();
			correctPlayerRot = (Quaternion)stream.ReceiveNext ();
			curState = (CharacterState)(int)stream.ReceiveNext ();
		}
	}
	
	//[RPC]
	public void SetName(string playerName){
		nameLabel.text=playerName;
		if(!PhotonNetwork.offlineMode){
			photonView.RPC("SetName",PhotonTargets.Others,playerName);
		}
	}
	
	public void SetName(string playerName, Color color){
		nameLabel.color=color;
		SetName(playerName);
	}
	
	private void OnPhotonInstantiate(PhotonMessageInfo info)
    {
		if(!photonView.isMine){
			PhotonView.Get(GameManager.Player.transform).RPC("SetName",photonView.owner,GameManager.Player.PlayerName);
		}
	}
	
	/// <summary>
	/// Mounts the remote player
	/// </summary>
	/// <param name='viewID'>
	/// ViewID
	/// </param>
	//[RPC]
	private void Mount (int viewID)
	{
		PhotonView mountView = PhotonView.Find (viewID);
		Mount mount= mountView.GetComponent<Mount>();
		mount.transform.parent= transform;
		
		movement.mountAnimation = mount.mountAnimation;
		mount.playerCharacter=movement.character;
		movement.character= mount.character;
		movement.playerAnimation.transform.parent=mount.bone;
		mount.movement=movement;
		
	}
	
	/// <summary>
	/// Plays the animation over network
	/// </summary>
	/// <param name='anim'>
	/// Animation.
	/// </param>
	//[RPC]
	private void PlayAnimation(string anim)
	{
		movement.playerAnimation [anim].layer = 3;
		movement.playerAnimation.CrossFade (anim);
		
	}
	
	//[RPC]
	public void SetAnimationLayer(string anim, int layer){
		movement.playerAnimation[anim].layer=layer;
	}
	
	//[RPC]
	private void PlayAnimation(string anim, float animationSpeed)
	{
		movement.playerAnimation[anim].speed=animationSpeed;
		movement.playerAnimation [anim].layer = 3;
		movement.playerAnimation.CrossFade (anim);
	}
	
	/// <summary>
	/// Sets the player dead.
	/// </summary>
	/// <param name='state'>
	/// State.
	/// </param>
	//[RPC]
	private void SetDead(bool state){
		dead=state;
	}
	
	/// <summary>
	/// Spawns the damage text.
	/// </summary>
	/// <param name='damage'>
	/// Damage.
	/// </param>
	//[RPC]
	private void SpawnDamagePanel (int damage)
	{
		GameObject damagePanel = (GameObject)Instantiate (GameManager.GamePrefabs.damage, transform.position + Vector3.up * characterHeight, Quaternion.identity);
		damagePanel.GetComponentInChildren<UILabel> ().text = damage.ToString ();
		damagePanel.transform.parent = transform;
	}
	
	/// <summary>
	/// Applies the damage over Network
	/// </summary>
	/// <param name='attribute'>
	/// Attribute.
	/// </param>
	/// <param name='damage'>
	/// Damage.
	/// </param>
	/// <param name='defenceAttribute'>
	/// Defence attribute.
	/// </param>
	//[RPC]
	public void ApplyDamage(string attribute, int damage, string defenceAttribute){
		if(dead){
			return;
		}
		PlayerAttribute attr= GameManager.Player.GetAttribute(attribute);
		PlayerAttribute defAttr=GameManager.Player.GetAttribute(defenceAttribute);
		if(defAttr != null){
			damage-=(int)defAttr.CurValue;
			if(damage<0){
				damage=0;
			}
		}
		photonView.RPC("SpawnDamagePanel",PhotonTargets.All,damage);
		if(attr!= null){
			attr.ApplyDamage(damage);
		}
		
		if (GameManager.Player.Character.getHit != null && !dead) {
			GameManager.Player.Movement.PlayAnimation (GameManager.Player.Character.getHit.name, 0.5f, 2);
			GameManager.Player.PlaySound(GameManager.Player.Character.getHitSound);
			if(GameManager.PlayerSettings.shakeCamera){
				PlayerCamera.Instance.Shake (0.2f, 0.03f);
			}
		}
		
		if (attr.CurValue <= 0) {
			GameManager.Player.Dead = true;
		}
	}
	
	/// <summary>
	/// Applies the exp.
	/// </summary>
	/// <param name='val'>
	/// Exp Value.
	/// </param>
	//[RPC]
	private void ApplyExp(float val){
		GameManager.Player.Exp.ApplyExp(val);
	}
	
	/// <summary>
	/// Loads the level for all players in room
	/// </summary>
	/// <param name='level'>
	/// Level.
	/// </param>
	//[RPC]
	private void LoadLevel(string level){

		Hashtable table = new Hashtable();
		table.Add("scene",level);
		
		PhotonNetwork.room.SetCustomProperties(table);
		
		PhotonNetwork.isMessageQueueRunning=false;
		
		Application.LoadLevel(level);
	}
	
	private void OnMouseUp(){
		Debug.Log("OnMouseUp");
		if(GameManager.GameSettings.allowPvp){
			GameManager.Player.Target=transform;
		}
	}
}
