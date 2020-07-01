using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// This class contains data about the player
/// </summary>
[System.Serializable]
public class Player:ISerializable
{
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Player"/> class.
	/// </summary>
	/// <param name='mTransform'>
	/// Transform of the player
	/// </param>
	/// <param name='mCharacter'>
	/// Character asset
	/// </param>
	public Player (Transform mTransform, Character mCharacter, string name)
	{
		Initialize(mTransform);	
		this.Character=mCharacter;
		this.PlayerName = name;
		this.Level=1;
		this.Exp = new Exp (level);
		this.Inventory = new Inventory (mTransform.GetComponent<PlayerEquipment> ());
		
		//Load data from PlayerSettings
		this.Gold = GameManager.PlayerSettings.gold;
		this.FreeTalentPoints = GameManager.PlayerSettings.freeTalentPoints;
		this.FreeAttributePoints = GameManager.PlayerSettings.freeAttributePoints;
		
		
		UnityTools.StartCoroutine (SaveGame (),true);
	}
	
	public void Initialize(Transform mTransform){
		this.transform = mTransform;
	
		this.CharacterController = mTransform.GetComponent<CharacterController> ();
		this.Movement = mTransform.GetComponent<ThirdPersonMovement> ();
		if(GameManager.PlayerSettings.nameOverHead){
			this.transform.GetComponent<PhotonNetworkPlayer> ().SetName (playerName);
		}
		if(Inventory != null){
			Inventory.InitializeStartEquipment();
		}
	}
	
	private IEnumerator SaveGame ()
	{
		while (true) {
			yield return new WaitForSeconds(GameManager.GameDatabase.saveInterval);
			GameManager.GameDatabase.SaveGame ();
			
		}
	}
	
	/// <summary>
	/// The level of the player
	/// </summary>
	private int level;
	public int Level {
		get{ return level;}
		set { 
			if(value >= GameManager.PlayerSettings.maxLevel){
				return;
			}
			level = value;
			RefreshAttributes ();
			
			if (level > 1) {
				FreeTalentPoints++;
				FreeAttributePoints++;
				
				MessageManager.Instance.AddMessage (GameManager.GameMessages.levelUp.Replace ("@Level", level.ToString ()));
			}
			
		}
	}
	
	/// <summary>
	/// The name of the player
	/// </summary>
	private string playerName;
	public string PlayerName {
		get{ return playerName;}
		set {
			playerName = value;
			transform.GetComponent<PhotonNetworkPlayer> ().SetName (playerName);
		}
	}
	
	/// <summary>
	/// Free talent points that the player can spent on his talents.
	/// </summary>
	private int freeTalentPoints;
	public int FreeTalentPoints {
		get{ return freeTalentPoints;}
		set{ freeTalentPoints = value;}
	}
	
	/// <summary>
	/// Free attribute points that the player can spent on his attributes.
	/// </summary>
	private int freeAttributePoints;
	public int FreeAttributePoints {
		get{ return freeAttributePoints;}
		set{ freeAttributePoints = value;}
	}
	
	/// <summary>
	/// The player can purchase items with gold in a shop.
	/// </summary>
	private int gold;
	public int Gold {
		get{ return gold;}
		set{ gold = value;}
	}
	
	/// <summary>
	/// Base character information. This should not be changed at runtime.
	/// </summary>
	private Character character;
	public Character Character {
		get{ return character;}
		private set {
			character = (Character)ScriptableObject.Instantiate(value);
			for (int i=0; i<character.attributes.Count; i++) {
				switch (i) {
				case 0:
					AddAttribute (character.attributes [i], InterfaceContainer.Instance.redBar);
					break;
				case 1:
					AddAttribute (character.attributes [i], InterfaceContainer.Instance.blueBar);
					break;
				case 2:
					AddAttribute(character.attributes[i],InterfaceContainer.Instance.greenBar);
					break;
				default:
					AddAttribute (character.attributes [i]);
					break;
				}
			}
			
			for (int i=0; i<character.talents.Count; i++) {
				AddTalent (character.talents [i]);
			}
		}
		
	}
	
	/// <summary>
	/// The player experience points
	/// </summary>
	private Exp exp;
	public Exp Exp {
		get{ return exp;}
		private set{ exp = value;}
	}
	
	/// <summary>
	/// The player inventory.
	/// </summary>	
	private Inventory inventory;
	public Inventory Inventory {
		get{ return inventory;}
		private set{ inventory = value;}
	}
	
	/// <summary>
	/// Property to find out if the player is dead
	/// </summary>
	private bool dead;
	public bool Dead {
		get{ return dead;}
		set {
			if(dead == value){
				return;
			}
			
			dead = value;
			
			
			if (dead) {
				Dismount ();
				GameManager.Player.Movement.PlayAnimation (character.die.name, GameManager.PlayerSettings.respawnDelay);
				Target= null;
				TargetBehaviour= null;
				Movement.Stop();
				MessageManager.Instance.AddMessage (GameManager.GameMessages.dead);
				UnityTools.StartCoroutine (Respawn (GameManager.PlayerSettings.respawnDelay));//character.die.length - (character.die.length * 10 / 100))); //Respawn after death animation played - 10% of time
			}
			if (!PhotonNetwork.offlineMode) {
				PhotonView.Get (transform).RPC ("SetDead", PhotonTargets.All, dead);
			}
		}
	}
	
	/// <summary>
	/// Checkpoint Vector3 position. The player will be respawned at this position.
	/// </summary>
	private Vector3 checkpoint;
	public Vector3 Checkpoint {
		get{ return checkpoint;}
		set{ checkpoint = value;}
	}
	
	/// <summary>
	/// Reference to the transform of the player
	/// </summary>
	/// <value>
	/// Transform
	/// </value>
	private Transform _transform;
	public Transform transform { 
		get{ return _transform;}
		private set{ _transform = value;}
	}
	
	private AudioSource _audioSource;
	public AudioSource AudioSource{
		get{
			if(!_audioSource){
				_audioSource=transform.GetComponent<AudioSource>();
			}
			return _audioSource;
		}
		set{_audioSource=value;}
	}
	
	private AudioSource humenAudioSource;
	public void PlaySound(AudioClip clip){
		if(clip == null){
			return;
		}
		if(humenAudioSource == null){
			GameObject temp= new GameObject("HumenSounds");
			humenAudioSource=temp.AddComponent<AudioSource>();
		}
		humenAudioSource.transform.position=transform.position;
		if(!humenAudioSource.isPlaying){
			humenAudioSource.clip=clip;
			humenAudioSource.Play();
		}
	}
	
	/// <summary>
	/// Reference to the player CharacterController
	/// </summary>
	private CharacterController characterController;
	public CharacterController CharacterController {
		get{ return characterController;}
		private set{ characterController = value;}
	}
	
	private Animation _animation;
	public Animation animation{
		get{return _animation;}
		private set{_animation=value;}
	}
	
	/// <summary>
	/// The target behaviour. Instead of GetComponent we set this OnMouseUp in the AiBehaviour.
	/// This is more performance friendly. This is currently used only in the ThirdPersonMovement-> ClickToMove
	/// </summary>
	private AiBehaviour targetBehaviour;
	public AiBehaviour TargetBehaviour {
		get{ return targetBehaviour;}
		set{ targetBehaviour = value;}
	}
	
	/// <summary>
	/// The target of the player
	/// </summary>
	private Transform target;
	public Transform Target {
		get{ return target;}
		set{ target = value;}
	}
	
	/// <summary>
	/// Gets the attribute.
	/// </summary>
	/// <returns>
	/// The attribute
	/// </returns>
	/// <param name='attributeName'>
	/// Attribute name.
	/// </param>
	public PlayerAttribute GetAttribute (string attributeName)
	{
		return character.attributes.Find (attribute => attribute.Name == attributeName);
	}
	
	/// <summary>
	/// Adds the attribute to the character attributes collection and the interface
	/// </summary>
	/// <param name='attribute'>
	/// Attribute.
	/// </param>
	public void AddAttribute (PlayerAttribute attribute)
	{
		if (!character.attributes.Contains (attribute)) {
			character.attributes.Add (attribute);
		}
		
		GameObject go = NGUITools.AddChild (InterfaceContainer.Instance.attributeSlots.gameObject, GameManager.GamePrefabs.attribute);
		PlayerAttributeSlot slot = go.GetComponent<PlayerAttributeSlot> ();
		slot.Init (attribute);
		attribute.Init (null);
		InterfaceContainer.Instance.attributeSlots.Reposition ();
	}
	
	/// <summary>
	/// Adds the attribute with a bar
	/// </summary>
	/// <param name='attribute'>
	/// Attribute.
	/// </param>
	/// <param name='bar'>
	/// UISprite as bar
	/// </param>
	public void AddAttribute (PlayerAttribute attribute, UISprite bar)
	{
		if (!character.attributes.Contains (attribute)) {
			character.attributes.Add (attribute);
		}
		attribute.Init (bar);
		
		if(attribute.raisable){
			GameObject go = NGUITools.AddChild (InterfaceContainer.Instance.attributeSlots.gameObject, GameManager.GamePrefabs.attribute);
			PlayerAttributeSlot slot = go.GetComponent<PlayerAttributeSlot> ();
			slot.Init (attribute);
			InterfaceContainer.Instance.attributeSlots.Reposition ();
		}
	}
	
	/// <summary>
	/// Refreshs the attributes.
	/// </summary>
	public void RefreshAttributes ()
	{
		character.attributes.ForEach (attr => attr.Refresh ());
	}
	
	/// <summary>
	/// Gets the talent.
	/// </summary>
	/// <returns>
	/// The talent.
	/// </returns>
	/// <param name='talentName'>
	/// Talent name.
	/// </param>
	public BaseTalent GetTalent (string talentName)
	{
		return character.talents.Find (talent => talent.talentName == talentName);
	}
	
	/// <summary>
	/// Adds the talent to the character talent collection and the interface.
	/// </summary>
	/// <param name='talent'>
	/// Talent.
	/// </param>
	public void AddTalent (BaseTalent talent)
	{
		GameObject go = NGUITools.AddChild (InterfaceContainer.Instance.talentTable.gameObject, GameManager.GamePrefabs.talent);
		TalentSlot slot = go.GetComponent<TalentSlot> ();
		slot.talent = talent;
	}
	
	/// <summary>
	/// Respawn the the player after specified delay.
	/// </summary>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	public IEnumerator Respawn (float delay)
	{
		yield return new WaitForSeconds(delay);
		
		transform.position = UnityTools.RandomPointInArea (Checkpoint, 1) + Vector3.up;
		GameManager.Player.Movement.PlayAnimation (character.idle.name,1);
		GameManager.Player.Movement.SetAnimationLayer(character.idle.name,0);
		RefreshAttributes ();
		Target= null;
		TargetBehaviour= null;
		Movement.Stop();
		Dead = false;
	}
	
	/// <summary>
	/// Reference to the Movement script, be it horse or player.
	/// </summary>
	private ThirdPersonMovement movement;
	public ThirdPersonMovement Movement {
		get{ return movement;}
		private set{ movement = value;
			animation=movement.playerAnimation;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether the player is mounted.
	/// </summary>
	/// <value>
	/// <c>true</c> if the player is mounted; otherwise, <c>false</c>.
	/// </value>
	public bool IsMounted {
		get{ return mountTransform != null;}
	}
	
	/// <summary>
	/// Reference to the mount transform
	/// </summary>
	private Transform mountTransform;
	public Transform MountTransform {
		get{ return mountTransform;}
		set{ mountTransform = value;}
	}
	
	/// <summary>
	/// Mounts player.
	/// </summary>
	/// <param name='mount'>
	/// Mount transform.
	/// </param>
	public void Mount (Transform mount)
	{
		mount.parent = transform;
		Movement.playerAnimation.transform.parent = mount.GetComponent<Mount> ().bone;
		Movement.mountAnimation = mount.GetComponent<Mount> ().mountAnimation;
		Movement.character = mount.GetComponent<Mount> ().character;
		MountTransform = mount;
		
		PhotonView.Get (transform).RPC ("Mount", PhotonTargets.Others, PhotonView.Get (mount).viewID);
	}
	
	/// <summary>
	/// Dismounts the player and destroyes the mount transform
	/// </summary>
	public void Dismount ()
	{
		if (MountTransform != null) {
			Movement.playerAnimation.transform.parent = transform;
			Movement.playerAnimation.transform.localPosition = Vector3.zero;
			Movement.playerAnimation.transform.localRotation = Quaternion.Euler (Vector3.zero);
			Movement.character = Character;
			Movement.Stop ();
			PhotonNetwork.Destroy (MountTransform.gameObject);
		}
	}
	
	#region Serialization
	//Deserialization constructor.
	public Player (SerializationInfo info, StreamingContext ctxt)
	{
		//Get the values from info and assign them to the appropriate properties
		Character=GameManager.CharacterDatabase.GetCharacter((string)info.GetValue ("CharacterClass", typeof(string)));
		playerName = (string)info.GetValue ("PlayerName", typeof(string));
		level = (int)info.GetValue ("Level", typeof(int));
		FreeTalentPoints = (int)info.GetValue ("FreeTalentPoints", typeof(int));
		FreeAttributePoints = (int)info.GetValue ("FreeAttributePoints", typeof(int));
		Gold = (int)info.GetValue ("Gold", typeof(int));
		Checkpoint =new Vector3((float)info.GetValue ("CheckpointX", typeof(float)),(float)info.GetValue ("CheckpointY", typeof(float)),(float)info.GetValue ("CheckpointZ", typeof(float)));
		Exp=new Exp(level);
		Exp.ApplyExp((float)info.GetValue("Exp",typeof(float)));
		foreach(BaseTalent talent in Character.talents){
			if(info.ContainsValue(talent.talentName)){
				talent.spentPoints=(int)info.GetValue(talent.talentName,typeof(int));
			}
		}
		
		foreach(PlayerAttribute attribute in Character.attributes){
			if(info.ContainsValue(attribute.attributeName)){
				attribute.BaseValue=(float)info.GetValue(attribute.attributeName,typeof(float));
			}
		}
		Inventory= new Inventory();
		List<Pair<string,object[]>> inv= (List<Pair<string,object[]>>)info.GetValue("Inventory",typeof(List<Pair<string,object[]>>));
		
		foreach(Pair<string, object[]> kvp in inv){
			ItemSlot slot= Inventory.GetItemSlot((int)kvp.Value[0]);
			
			if(slot != null){
				BaseItem temp= (BaseItem)ScriptableObject.Instantiate(GameManager.ItemDatabase.GetItem(kvp.Key));
				temp.stack=(int)kvp.Value[1];
				if(temp is BonusItem && kvp.Value[2] != null){
					(temp as BonusItem).bonus=(List<Bonus>)kvp.Value[2];
				}
				Inventory.AddItem(temp,slot);
			}
		}
		
		List<Pair<string,object[]>> equipment= (List<Pair<string,object[]>>)info.GetValue("Equipment",typeof(List<Pair<string,object[]>>));
		foreach(Pair<string, object[]> kvp in equipment){
			EquipmentItem temp= (EquipmentItem)ScriptableObject.Instantiate(GameManager.ItemDatabase.GetItem(kvp.Key));
			temp.bonus=(List<Bonus>)kvp.Value[0];
			Inventory.AddStartEquipmentItem(temp);
		}
		
		List<Pair<string,object[]>> quests= (List<Pair<string,object[]>>)info.GetValue("Quest",typeof(List<Pair<string,object[]>>));
		Quest[] gameQuests=(Quest[])GameObject.FindObjectsOfType(typeof(Quest));
		foreach(Quest quest in gameQuests){
			foreach(Pair<string,object[]> kvp in quests){
				if(quest.questName.Equals(kvp.Key)){
					bool completed=(bool)kvp.Value[0];
					int taskId=(int)kvp.Value[1];
					if(completed){
						quest.completed=true;
						QuestManager.Instance.AddToQuestLog(quest);
					}else{
						quest.curTaskId=taskId;
						QuestManager.Instance.quests.Add(quest);
						QuestManager.Instance.AddToQuestLog(quest);
						QuestManager.Instance.AddToQuestBar(quest);
					}
				}
			}
		}
		
		List<Pair<KeyCode,string>> actionBar=(List<Pair<KeyCode,string>>)info.GetValue("ActionBar",typeof(List<Pair<KeyCode,string>>));
		
		foreach(Pair<KeyCode,string> kvp in actionBar){
			foreach(SlotContainer slotContainer in InterfaceContainer.Instance.actionBar.GetComponentsInChildren<SlotContainer>()){
				if(slotContainer.key==kvp.Key){
					slotContainer.Set(InterfaceContainer.Instance.GetTalentSlot(kvp.Value));
				}
			}
		}
		UnityTools.StartCoroutine (SaveGame (),true);
	}
	
	//Serialization function.
	public void GetObjectData (SerializationInfo info, StreamingContext ctxt)
	{
		//You can use any custom name for your name-value pair. 
		info.AddValue ("PlayerName", PlayerName);
		info.AddValue ("Level", Level);
		info.AddValue ("FreeTalentPoints", FreeTalentPoints);
		info.AddValue ("FreeAttributePoints", FreeTalentPoints);
		info.AddValue ("Gold", Gold);
		info.AddValue ("CheckpointX", Checkpoint.x);
		info.AddValue ("CheckpointY", Checkpoint.y);
		info.AddValue ("CheckpointZ", Checkpoint.z);
		info.AddValue("CharacterClass",Character.characterClass);
		info.AddValue("Exp", Exp.CurValue);
		EquipmentItem weapon= Inventory.GetEquipmentItem(EquipmentItem.Region.Hands);
		
		if(weapon && weapon.overrideMouseTalent != null){
			weapon.overrideMouseTalent.spentPoints-=1;
		}
		
		foreach(BaseTalent talent in Character.talents){

			info.AddValue(talent.talentName,talent.spentPoints );
		}
		
		if(weapon && weapon.overrideMouseTalent != null){
			weapon.overrideMouseTalent.spentPoints+=1;
		}
		
		foreach(PlayerAttribute attribute in Character.attributes){
			info.AddValue(attribute.attributeName,attribute.BaseValue);
		}
		
		List<Pair<string,object[]>> inv= new List<Pair<string, object[]>>();
		
		foreach(BaseItem item in Inventory.GetItems()){
			if(item != null){
				inv.Add( new Pair<string,object[]>(item.itemName,new object[]{item.itemSlotId ,item.stack,(item is BonusItem)?(item as BonusItem).bonus:null}));
			}
		}
		
		info.AddValue("Inventory",inv);
		
		
		List<Pair<string,object[]>> equip= new List<Pair<string, object[]>>();
		
		foreach(EquipmentItem item in Inventory.GetEquipmentItems()){
			if(item != null){
				equip.Add( new Pair<string,object[]>(item.itemName,new object[]{item.bonus}));
			}
		}
		
		info.AddValue("Equipment",equip);
		
		List<Pair<string, object[]>> quests= new List<Pair<string,object[]>>();
		foreach(Quest quest in QuestManager.Instance.quests){
			object[] data= new object[]{quest.completed,quest.curTaskId};
			quests.Add(new Pair<string,object[]>(quest.questName,data));
		}
		
		info.AddValue("Quest",quests);
		
		List<Pair<KeyCode,string>> actionBar= new List<Pair<KeyCode, string>>();
		foreach(SlotContainer slotContainer in InterfaceContainer.Instance.actionBar.GetComponentsInChildren<SlotContainer>()){
			if(slotContainer.slot != null){
				if(slotContainer.slot is TalentSlot){
					BaseTalent talent= (slotContainer.slot as TalentSlot).talent;
					actionBar.Add(new Pair<KeyCode,string>(slotContainer.key,talent.talentName));
				}
			}
		}
		
		info.AddValue("ActionBar",actionBar);
	}
	#endregion Serialization
}
