using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("RPG Kit 2.0/InterfaceContainer")]
public class InterfaceContainer : MonoBehaviour
{
	private static InterfaceContainer instance;

	public static InterfaceContainer Instance {
		get{ return instance;}
	}
	
	private void Awake ()
	{
		instance = this;
	}

	public UISprite dragIcon;
	public GameObject profileWindow;
	public UITable equipmentTable;
	public UIGrid inventoryGrid;
	public UITable talentTable;
	public GameObject talentWindow;
	public UILabel freeTalentPoints;
	public UILabel playerLevel;
	public UISprite redBar;
	public UISprite blueBar;
	public UISprite greenBar;
	public UILabel gold;
	public UILabel playerName;
	public AOETarget aoeTarget;
	public GameObject settingsWindow;

	public GameObject inventoryToolTipWindow;
	public UILabel inventoryToolTipItemName;
	public UITable inventoryToolTipTable;
	public GameObject equipmentToolTipWindow;
	public UILabel equipmentToolTipItemName;
	public UITable equipmentToolTipTable;
	public GameObject worldToolTipWindow;
	public UILabel worldToolTipItemName;
	public UITable worldToolTipTable;
	public GameObject shopWindow;
	public UIGrid shopTable;
	public UITable purchaseTable;
	public UITable sellingTable;
	public UILabel totalCost;
	public UILabel totalPayout;
	public GameObject talentDescriptionWindow;
	public UILabel talentDescriptionName;
	public UILabel talentDescriptionText;
	public UISprite expBar;
	public GameObject checkpointWindow;
	public GameObject teleporterWindow;
	public UITable teleporterTable;
	public GameObject containerWindow;
	public UITable containerTable;
	public UILabel messageStorageLabel;
	public GameObject npcBarWindow;
	public UISprite npcBar;
	public UILabel npcName;
	public GameObject dismountButton;
	public GameObject toolTipWindow;
	public UILabel toolTipItemName;
	public UITable toolTipTable;
	public UITable attributeSlots;
	public UILabel freeAttributePoints;
	
	[HideInInspector]
	public Slot draggingSlot;
	[HideInInspector]
	public bool isDragging;
	public GameObject actionBar;
	public ProgressBar progressBar;
	
	public GameObject craftingMenu;
	public UITable craftingTable;
	public GameObject craftingDetailsWindow;
	public GameObject interfaceWindow;
	
	public bool UIClick ()
	{
		if (UICamera.mainCamera != null) {
			Ray inputRay = UICamera.mainCamera.ScreenPointToRay (Input.mousePosition);    
			RaycastHit widgetHit;

			if (Physics.Raycast (inputRay.origin, inputRay.direction, out widgetHit, Mathf.Infinity, 1 << LayerMask.NameToLayer ("Interface"))) {

				return true;
			}
		}
		return false;
	}
	
	
	private void Update(){
		
		UpdatePlayerInterface();
		
		if(Input.GetKeyDown(GameManager.InputSettings.talent) && !ChatManager.Instance.chatInput.gameObject.activeSelf){
			talentWindow.gameObject.SetActive(!talentWindow.activeSelf);
		}
		
		if(Input.GetKeyDown(GameManager.InputSettings.profile)&& !ChatManager.Instance.chatInput.gameObject.activeSelf){
			profileWindow.gameObject.SetActive(!profileWindow.activeSelf);
			if(profileWindow.activeSelf){
				QuestManager.Instance.questBarWindow.SetActive(false);
			}else{
				QuestManager.Instance.questBarWindow.SetActive(true);
			}
		}
	
		if(Input.GetKeyDown(GameManager.InputSettings.settings) && !ChatManager.Instance.chatInput.gameObject.activeSelf){
			settingsWindow.gameObject.SetActive(!settingsWindow.activeSelf);
		}
		
		if(Input.GetKeyDown(GameManager.InputSettings.close)){
			profileWindow.SetActive(false);
			talentWindow.SetActive(false);
			shopWindow.SetActive(false);
			settingsWindow.SetActive(false);
		}
	}
	
	public void ShowToolTip (BaseItem item, Vector2 offset)
	{
		if (item is EquipmentItem && (item as EquipmentItem).bonus != null && (item as EquipmentItem).bonus.Count>0) {
			toolTipWindow.SetActive (true);
			toolTipItemName.text = item.itemName;
			toolTipTable.Reposition ();
			foreach (ToolTip toolTip in toolTipTable.GetComponentsInChildren<ToolTip>()) {
				Destroy (toolTip.gameObject);
			}
		
			toolTipWindow.transform.position = new Vector3 (UICamera.lastHit.point.x + offset.x, UICamera.lastHit.point.y + offset.y, toolTipWindow.transform.position.z);
				
			EquipmentItem equipment = item as EquipmentItem;
			foreach (Bonus b in equipment.bonus) {
				GameObject bonus = NGUITools.AddChild (toolTipTable.gameObject, GameManager.GamePrefabs.bonus);
				ToolTip toolTip = bonus.GetComponent<ToolTip> ();
				toolTip.bonusName.text = b.attribute;
				toolTip.bonusValue.text = "+" + b.bonusValue;
				toolTip.bonusName.color = b.color;
				toolTip.bonusValue.color = b.color;
			}
			toolTipTable.Reposition ();
		}
	}
	
	private void UpdatePlayerInterface(){
		if( GameManager.Player != null){
			freeTalentPoints.text= GameManager.Player.FreeTalentPoints.ToString();
			freeAttributePoints.text= GameManager.Player.FreeAttributePoints.ToString();
			playerLevel.text= GameManager.Player.Level.ToString();
			gold.text= GameManager.Player.Gold.ToString();
			playerName.text=GameManager.Player.PlayerName;
			dismountButton.SetActive(GameManager.Player.IsMounted);
		}
	}
	
	public TalentSlot GetTalentSlot(string talentName){
		talentWindow.SetActive(true);
		foreach(TalentSlot slot in talentTable.GetComponentsInChildren<TalentSlot>()){
			if(slot.talent != null && slot.talent.talentName.Equals(talentName)){
				talentWindow.SetActive(false);
				return slot;
			}
		}
		talentWindow.SetActive(false);
		return null;
	}
}
