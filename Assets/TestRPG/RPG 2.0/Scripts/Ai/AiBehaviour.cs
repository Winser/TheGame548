using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class AiBehaviour : Photon.MonoBehaviour
{
	public TextAsset file;
	public float agentHeight;
	private Dictionary<int,BaseState> states;
	private List<string> playerTags;
	private List<BaseAttribute> attributes;
	[HideInInspector]
	private int curStateId = -1;
	[HideInInspector]
	public Transform target;
	[HideInInspector]
	public float targetDistance;
	[HideInInspector]
	public Vector3 startPosition;
	[HideInInspector]
	public UnityEngine.AI.NavMeshAgent navMeshAgent;
	[HideInInspector]
	public int layerMask;
	[HideInInspector]
	public int initialStateId;
	public UISprite attributeBar;
	
	public int CurStateId {
		get{ return curStateId;}
		set {
			if (curStateId != value) {
				if (onStateChange != null) {
					onStateChange (states [value]);
				}
			}
			curStateId = value;
		}
	}
	
	public delegate void ExternalMove (Vector3 point,float moveSpeed,float rotationSpeed);

	public ExternalMove externalMove;
	
	public delegate void ExternalStopMove ();

	public ExternalStopMove externalStopMove;
	
	public delegate void OnApplyDamage (string attribute,float damage,float curValue);

	public OnApplyDamage onApplyDamage;
	
	public delegate void OnDeath (float exp,List<string> loot);

	public OnDeath onDeath;
	
	public delegate void OnRespawn ();

	public OnRespawn onRespawn;
	
	public delegate void OnStateChange (BaseState state);

	public OnStateChange onStateChange;
	
	public delegate void OnSay (string msg,float t);

	public OnSay onSay;
	
	private void Start ()
	{
		targetDistance = Mathf.Infinity;
		layerMask = 1 << gameObject.layer;
		layerMask = ~layerMask;
		states = new Dictionary<int,BaseState> ();
		playerTags = new List<string> ();
		attributes = new List<BaseAttribute> ();
		
		MemoryStream stream = new MemoryStream (file.bytes);
		BinaryFormatter formatter = new BinaryFormatter ();
		formatter.Binder = new VersionDeserializationBinder ();
		
		playerTags = (List<string>)formatter.Deserialize (stream);
		initialStateId = (int)formatter.Deserialize (stream);
		curStateId = initialStateId;
		attributes = (List<BaseAttribute>)formatter.Deserialize (stream);
		
		foreach (BaseAttribute attribute in attributes) {

			attribute.Init (attributeBar);
			StartCoroutine (attribute.Regenerate ());	
		}
		
		#pragma warning disable 0219
		float x = (float)formatter.Deserialize (stream);//unused
		#pragma warning disable 0219
		float y = (float)formatter.Deserialize (stream);//unused
		
		int cnt = (int)formatter.Deserialize (stream);
		
		for (int i=0; i< cnt; i++) {
			BaseState state = (BaseState)formatter.Deserialize (stream);
			if (state is WalkState) {
				(state as WalkState).nextPoint = UnityTools.RandomPointInArea (transform.position, 0, ~(1 << gameObject.layer));
			}
			
			if (state is PatrolState) {
				(state as PatrolState).path = WaypointManager.Instance.GetPath ((state as PatrolState).patrolId);
			}
			
			states.Add (state.id, state);
		}
		
		if (PhotonNetwork.isMasterClient) {
			StartCoroutine (FindClosestTarget ());
			StartCoroutine (UpdaterTargetDistance ());
		}
		startPosition = transform.position;
		navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		if (navMeshAgent) {
			navMeshAgent.stoppingDistance = 0;
		}
		
		onDeath += OnNpcDeath;
		onRespawn += OnNpcRespawn;
		onApplyDamage += OnGetHit;
		onSay += OnNpcSay;
	}
	
	private void Update ()
	{
		
		if (!PhotonNetwork.isMasterClient) {
			transform.position = Vector3.Lerp (transform.position, correctPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp (transform.rotation, correctRot, Time.deltaTime * 5);
			if (states.ContainsKey (CurStateId) && !(states [CurStateId] is TalentState)) {
				GetComponent<Animation>().CrossFade (states [CurStateId].animation);
			}
		} else {
			if (states.ContainsKey (CurStateId)) {
				//Debug.Log(states[CurStateId]);
				states [CurStateId].HandleState (this);
			}
		}
	}
	
	private void ListenTo (string shout)
	{
		states [CurStateId].listenTo = shout;
	}
	
	public IEnumerator FindClosestTarget ()
	{
		List<GameObject> targets = new List<GameObject> ();
		while (true) {
			yield return new WaitForSeconds(0.6f);
			targets.Clear ();
			foreach (string tag in playerTags) {
				
				GameObject[] gos = GameObject.FindGameObjectsWithTag (tag);
				targets.AddRange (gos);
				yield return new WaitForSeconds(0.1f);
			}

			GameObject closest = null;
			float distance = Mathf.Infinity;
			Vector3 position = transform.position;
			foreach (GameObject go in targets) {
				if (go != null && !go.GetComponent<PhotonNetworkPlayer>().dead) {
					Vector3 diff = go.transform.position - position;
					float curDistance = diff.sqrMagnitude;
					if (curDistance < distance) {
						closest = go;
						distance = curDistance;
					}
				}
			}
			if (closest != null) {
				target = closest.transform;
			}
		}
	}
	
	public List<Transform> AllPlayrsInViewField (float viewAngle, float maxDistance)
	{
		Collider[] gos = Physics.OverlapSphere (transform.position, maxDistance);
		List<Transform> list = new List<Transform> ();
		
		foreach (Collider col in gos) {
			
			if (playerTags.Contains (col.tag)) {
				float targetAngle = Vector3.Angle (col.transform.position - transform.position, transform.forward);
				if (Mathf.Abs (targetAngle) < (viewAngle / 2)) {
					RaycastHit hit; 
					if (Physics.Linecast (transform.position + Vector3.up * agentHeight, col.transform.position + Vector3.up * agentHeight, out hit)) { 	
						if (hit.transform == col.transform) {
							list.Add (col.transform);
						}
					}
				}
			}
		}
		return list;
	}
	
	private IEnumerator UpdaterTargetDistance ()
	{
		while (true) {
			yield return new WaitForSeconds(0.1f);
			if (target) {
				targetDistance = Vector3.Distance (target.position, transform.position);
			}
		}
	}
	
	public bool CanSee (Transform targ, float viewAngle, float agentHeight)
	{ 
		float targetAngle = Vector3.Angle (targ.position - transform.position, transform.forward);
		Debug.DrawLine (transform.position + Vector3.up * agentHeight, targ.position);
		if (Mathf.Abs (targetAngle) < (viewAngle / 2)) {
			RaycastHit hit; 
			
			if (Physics.Linecast (transform.position + Vector3.up * agentHeight, targ.position + Vector3.up, out hit, layerMask)) { 
				return hit.transform == targ; 
			}
		}
		return false; 
	}
	
	public BaseAttribute GetBaseAttribute (string name)
	{
		foreach (BaseAttribute attribute in attributes) {
			if (attribute.name.Equals (name)) {
				return attribute;
			}
		}
		return null;
	}
	
	public List<BaseAttribute> GetBaseAttributes ()
	{
		return attributes;
	}
	
	public BaseState GetCurrentState ()
	{
		return states [CurStateId];
	}
	
	public Dictionary<int,BaseState> GetStates ()
	{
		return states;
	}
	
	public void MoveAgent (Vector3 point, float moveSpeed, float rotationSpeed)
	{
		if (!PhotonNetwork.isMasterClient) {
			return;
		}
		
		if (navMeshAgent != null && navMeshAgent.enabled) {
			navMeshAgent.speed = moveSpeed;
			navMeshAgent.SetDestination (point);
		}
		
		if (externalMove != null) {
			externalMove (point, moveSpeed, rotationSpeed);
		}
	}
	
	public void StopAgent ()
	{
		if (!PhotonNetwork.isMasterClient) {
			return;
		}
		if (navMeshAgent != null && navMeshAgent.enabled) {
			navMeshAgent.Stop ();
		}
		if (externalStopMove != null) {
			externalStopMove ();
		}
	}

	public void StartApplyPlayerDamage (string attribute, float damage)
	{

		if (PhotonNetwork.offlineMode) {
			
			if (states [CurStateId] is AttackState || states [CurStateId] is TalentState) {
				PlayerAttribute playerAttribute = GameManager.Player.GetAttribute (attribute);
				playerAttribute.ApplyDamage (damage);
				if (playerAttribute.CurValue <= 0 && !GameManager.Player.Dead) {
					GameManager.Player.Dead = true;
				} else {
					GameObject damagePanel = (GameObject)Instantiate (GameManager.GamePrefabs.damage, GameManager.Player.transform.position + Vector3.up * GameManager.Player.CharacterController.height * 0.9f, Quaternion.identity);
					damagePanel.GetComponentInChildren<UILabel> ().text = "-" + (damage).ToString ();
					damagePanel.transform.parent = GameManager.Player.transform;
					if (GameManager.Player.Character.getHit != null) {
						GameManager.Player.Movement.PlayAnimation (GameManager.Player.Character.getHit.name, 0.5f, 2);
						GameManager.Player.PlaySound(GameManager.Player.Character.getHitSound);
						if(GameManager.PlayerSettings.shakeCamera){
							PlayerCamera.Instance.Shake (0.2f, 0.03f);
						}
					}
				}
			}
		} else {
			PhotonView.Get(target).RPC("ApplyDamage",PhotonView.Get(target).owner,attribute,(int)damage,"Defence");
		}
	}

	//[RPC]
	private void ApplyDamageOverNetwork (string attribute, float damage)
	{
		BaseAttribute defenderAttribute = GetBaseAttribute (attribute);
		defenderAttribute.ApplyDamage (this, damage);
		if (!Dead) {
			GameObject damagePanel = (GameObject)Instantiate (GameManager.GamePrefabs.damage, transform.position + Vector3.up * agentHeight, Quaternion.identity);//+ Vector3.up * behaviour.controller.height, Quaternion.identity);
			damagePanel.GetComponentInChildren<UILabel> ().text = damage.ToString ();
			damagePanel.transform.parent = transform;
		}
	}
	
	//[RPC]
	private void PlayAnimation (string anim)
	{
		/*if (!(states [CurStateId] is TalentState)) {
			animation.CrossFade (states [CurStateId].animation);
			return;
		}*/
		GetComponent<Animation>().CrossFade (anim);
	}
	
	private bool dead;

	public bool Dead {
		get{ return dead;}
	}
	
	private void OnNpcDeath (float exp, List<string> loot)
	{
		if (!dead) {
			dead = true;
			GetComponent<Collider>().enabled=false;
			InterfaceContainer.Instance.npcBarWindow.SetActive (false);
			if (!PhotonNetwork.offlineMode) {
				if (PhotonNetwork.isMasterClient) {
					SpawnLoot (loot.ToArray ());
				}
				PhotonNetworkPlayer[] players = UnityTools.FindObjectsOfType<PhotonNetworkPlayer> (transform.position, 30);
				for (int i=0; i< players.Length; i++) {
					PhotonView.Get (players [i]).RPC ("ApplyExp", PhotonView.Get (players [i]).owner, exp);
				}
				
			} else {
				SpawnLoot (loot.ToArray ());
				GameManager.Player.Exp.ApplyExp (exp);
			}
		}
	}
	
	private void SpawnLoot (string[] loot)
	{
		foreach (string itemName in loot) {
			BaseItem item = GameManager.ItemDatabase.GetItem (itemName);

			GameObject go = PhotonNetwork.Instantiate (item.prefab.name, UnityTools.RandomPointInArea (transform.position, 3), UnityTools.RandomQuaternion (Vector3.up, 0, 360), 0);
			Lootable lootable = go.GetComponent<Lootable> ();
				
			if (lootable) {
				lootable.item = (BaseItem)Instantiate (lootable.item);
				if (lootable.item is BonusItem) {
					(lootable.item as BonusItem).RandomizeBonus ();
				}
				/*GameObject itemNamePanel = (GameObject)Instantiate (GameManager.GamePrefabs.itemName, go.transform.position, Quaternion.identity);
				itemNamePanel.GetComponentInChildren<UILabel> ().text = "[" + lootable.item.stack.ToString () + "] " + lootable.item.itemName;
				itemNamePanel.transform.parent = go.transform;
				itemNamePanel.transform.localPosition += Vector3.up * 0.02f;	*/
			}	
		}
	}
	
	private void OnNpcRespawn ()
	{
		dead = false;
		GetComponent<Collider>().enabled=true;
	}
	
	private void OnGetHit (string attribute, float damage, float curValue)
	{
		int rnd = Random.Range (0, 100);
		if (rnd > 50) {
			GameObject go = PhotonNetwork.Instantiate (GameManager.GamePrefabs.bloodParticle.name, transform.position + Vector3.up * (agentHeight / 2), Quaternion.identity, 0);
			go.transform.parent = transform;
		}
	}
	
	private float sayTime;
	
	private void OnNpcSay (string msg, float t)
	{
		if (Time.time > sayTime) {
			transform.LookAt (new Vector3 (target.position.x, transform.position.y, target.position.z));
			GameObject go = (GameObject)Instantiate (GameManager.GamePrefabs.damage, transform.position + Vector3.up * agentHeight, Quaternion.identity);
			go.transform.parent = transform;
			go.GetComponentInChildren<UILabel> ().text = msg;
			sayTime = Time.time + t + 0.5f;
		}
	}
	
	private void OnMouseDown ()
	{
		GameManager.Player.TargetBehaviour = this;
		GameManager.Player.Movement.autoAttack = true;
	}
	
	public void OnMouseUp ()
	{
		if (attributeBar == null && !dead) {
			foreach (BaseAttribute attr in attributes) {
				attr.UpdateBar ();
			}
			InterfaceContainer.Instance.npcBarWindow.SetActive (true);
			DisplayName displayName = GetComponent<DisplayName> ();
			if (displayName != null) {
				InterfaceContainer.Instance.npcName.text = displayName.nameLabel.text;
				InterfaceContainer.Instance.npcName.color = displayName.nameLabel.color;
			} else {
				InterfaceContainer.Instance.npcName.text = "";
			}
		} else {
			InterfaceContainer.Instance.npcBarWindow.SetActive (false);
		}
	}
	
	public void OnMasterClientSwitched (PhotonPlayer player)
	{
		if (PhotonNetwork.isMasterClient) {
			StartCoroutine (FindClosestTarget ());
			StartCoroutine (UpdaterTargetDistance ());
		}
	}

	private Vector3 correctPos = Vector3.zero; //We lerp towards this
	private Quaternion correctRot = Quaternion.identity; //We lerp towards this
	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			//We own this player: send the others our data
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
			stream.SendNext (CurStateId);
		} else {
			//Network player, receive data
			correctPos = (Vector3)stream.ReceiveNext ();
			correctRot = (Quaternion)stream.ReceiveNext ();
			CurStateId = (int)stream.ReceiveNext ();
		}
	}
}
