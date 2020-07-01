using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Base class for damage talents
/// </summary>
[System.Serializable]
public class DamageTalent : BaseTalent
{
	//Attribute at which we should substract the damage
	public string damageAttribute = "Health";
	//Damage modifier attribute.The current value of this attribute will be added to the damage.
	public string damageAttributeModifier = "Damage";
	//Damage of the talent.
	public int damage;
	//Spent points modifier
	public float damageMod = 0.02f;
	//Defence attribute. Currect value of this attribute will be substracted from damage. 
	public string defenceAttribute = "Defence";
	//Maximum distance to the defender(AiBehaviour or Player).
	public float maxDistance = 2.0f;
	//Should we look at our target on usage?
	public bool lookAtTarget;
	
	/// <summary>
	/// Applies the damage over Network
	/// </summary>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	/// <param name='player'>
	/// Player to apply the damage.
	/// </param>
	public IEnumerator ApplyDamage (float delay, PhotonNetworkPlayer player, int extraDamage)
	{
		if(!player.dead && Vector3.Distance(player.transform.position,GameManager.Player.transform.position)<maxDistance){
			LookAtTarget(player.transform);
		}
		yield return new WaitForSeconds(delay);
		if(!player.dead && Vector3.Distance(player.transform.position,GameManager.Player.transform.position)<maxDistance){
			player.photonView.RPC ("ApplyDamage", player.photonView.owner, damageAttribute, damage + extraDamage, defenceAttribute);
		}
	}
	
	/// <summary>
	/// Applies the damage to Behaviour
	/// </summary>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	/// <param name='behaviour'>
	/// Behaviour.
	/// </param>
	public IEnumerator ApplyDamage (float delay, AiBehaviour behaviour)
	{
		yield return new WaitForSeconds(delay);
		//Is the ai still in range?
		if (Vector3.Distance (GameManager.Player.transform.position, behaviour.transform.position) < maxDistance) {
		
		
			//Are we in Multiplayer or Singleplayer instance?
			if (PhotonNetwork.offlineMode) {
				//Get the defender Attribute
				BaseAttribute defenderAttribute = behaviour.GetBaseAttribute (damageAttribute);
				//Apply damage to it
				defenderAttribute.ApplyDamage (behaviour, damage + GameManager.Player.GetAttribute (damageAttributeModifier).CurValue);
				//Defender dead?
				if (!behaviour.Dead) {
					//Instantiate the damage numbers.
					GameObject damagePanel = (GameObject)Instantiate (GameManager.GamePrefabs.damage, behaviour.transform.position + Vector3.up * behaviour.agentHeight, Quaternion.identity);
					damagePanel.GetComponentInChildren<UILabel> ().text = (damage + GameManager.Player.GetAttribute (damageAttributeModifier).CurValue).ToString ();
					damagePanel.transform.parent = behaviour.transform;
				}
			} else {
				//We are in Multiplayeer mode and apply damage over Network.
				behaviour.photonView.RPC ("ApplyDamageOverNetwork", PhotonTargets.All, damageAttribute, damage + GameManager.Player.GetAttribute (damageAttributeModifier).CurValue);
			}
		}
	}
	
	/// <summary>
	/// Applies damage to the local player
	/// </summary>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	public IEnumerator ApplyDamage (float delay)
	{
		yield return new WaitForSeconds(delay);
		//Is our player dead?
		if (!GameManager.Player.Dead) {
			//Get the player attribute, at which we will substract damage
			PlayerAttribute playerAttribute = GameManager.Player.GetAttribute (damageAttribute);
			//Get the player defence attribute, the current value will be substracted from the damage
			PlayerAttribute defencePlayerAttribute = GameManager.Player.GetAttribute (defenceAttribute);
			//Apply damage to the player attribute(often health)
			playerAttribute.ApplyDamage (damage - defencePlayerAttribute.CurValue);

		
			//No our player is still alive -> instantiate the damage numbers.
			GameObject damagePanel = (GameObject)Instantiate (GameManager.GamePrefabs.damage, GameManager.Player.transform.position + Vector3.up * GameManager.Player.CharacterController.height * 0.9f, Quaternion.identity);
			damagePanel.GetComponentInChildren<UILabel> ().text = "-" + (damage - defencePlayerAttribute.CurValue).ToString ();
			damagePanel.transform.parent = GameManager.Player.transform;
			if (GameManager.Player.Character.getHit != null) {
				GameManager.Player.Movement.PlayAnimation (GameManager.Player.Character.getHit.name, GameManager.Player.Character.getHit.length, 2);
			}
			
			//Check if the player attributes value is less or equal then zero, if yes set player dead property to true
			if (playerAttribute.CurValue <= 0) {
				GameManager.Player.Dead = true;
			}
			
		}
	}
	
	public void LookAtTarget(Transform target){
		if(lookAtTarget){
			//Look at target
			GameManager.Player.transform.LookAt (new Vector3 (target.position.x, GameManager.Player.transform.position.y, target.position.z));
		}
	}
	
	#if UNITY_EDITOR	
	public override void OnGUI(){
		base.OnGUI();
		damageAttribute=EditorGUILayout.TextField("Damage Attribute",damageAttribute);
		damageAttributeModifier=EditorGUILayout.TextField("Damage Attribute Modifier",damageAttributeModifier);
		damage=EditorGUILayout.IntField("Damage", damage);
		damageMod=EditorGUILayout.FloatField("Damage Modifier",damageMod);
		defenceAttribute=EditorGUILayout.TextField("Defence Attribute",defenceAttribute);
		maxDistance=EditorGUILayout.FloatField("Maximum Distance", maxDistance);
		lookAtTarget=EditorGUILayout.Toggle("Look At Target", lookAtTarget);
	}
	#endif
}
