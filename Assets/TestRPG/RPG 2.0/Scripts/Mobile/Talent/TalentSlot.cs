using UnityEngine;
using System.Collections;

/// <summary>
/// Talent ui slot.
/// </summary>
public class TalentSlot : Slot
{
	[HideInInspector]
	public GameObject raiseButton;
	[SerializeField]
	private UISprite overlay;
	[SerializeField]
	private UILabel spentPointsLabel;
	public BaseTalent talent;
	
	public override void Start ()
	{
		base.Start ();
		//Set the icon of this slot to the talent icon
		icon.spriteName = talent.icon;
		//Set the coolDown
		coolDown = talent.coolDown;
	}
	
	//We check the requirements for this talent, when we enable the the slot.
	private void OnEnable(){
		CheckSlot();
	}
	
	/// <summary>
	/// Checks the talent if the requirements are fullfilled.
	/// </summary>
	private void CheckSlot ()
	{
		//Player is null --> return
		if(GameManager.Player == null){
			return;
		}
		
		if (GameManager.Player.Level >= talent.minLevel || talent.spentPoints > 0) {
			overlay.gameObject.SetActive (false);
			GetComponent<Collider>().enabled = true;
			if (GameManager.Player.FreeTalentPoints > 0) {
				raiseButton.gameObject.SetActive (true);
			}
		} else {
			overlay.gameObject.SetActive (true);
			raiseButton.SetActive (false);
			GetComponent<Collider>().enabled = false;
		}
		
	}
	
	/// <summary>
	/// Use this talent.
	/// </summary>
	public override bool Use ()
	{
		return talent.Use ();
	}
	
	public override void Update ()
	{
		base.Update ();
		if(talent.spentPoints != 0){
			spentPointsLabel.text = talent.spentPoints.ToString ();
		}
	}
	
	/// <summary>
	/// Raises the talent.
	/// </summary>
	public void RaiseTalent ()
	{
		if (GameManager.Player.FreeTalentPoints > 0) {
			talent.spentPoints += 1;
			MessageManager.Instance.AddMessage (GameManager.GameMessages.talentRaise.Replace("@TalentName",talent.talentName).Replace("@SpentPoints",talent.spentPoints.ToString()));
			spentPointsLabel.text = talent.spentPoints.ToString ();
			GameManager.Player.FreeTalentPoints -= 1;
			
			if (GameManager.Player.FreeTalentPoints < 1) {
				foreach (TalentSlot slot in InterfaceContainer.Instance.talentTable.GetComponentsInChildren<TalentSlot>()) {
					slot.raiseButton.gameObject.SetActive (false);
				}
			}
		}
	}
	
	/// <summary>
	/// Raises the hover event. Used to show the talent description.
	/// </summary>
	/// <param name='isOver'>
	/// Is over.
	/// </param>
	private void OnHover (bool isOver)
	{
		if (isOver) {
			InterfaceContainer.Instance.talentDescriptionWindow.gameObject.SetActive (true);
			InterfaceContainer.Instance.talentDescriptionName.text = talent.talentName;
			InterfaceContainer.Instance.talentDescriptionText.text = talent.description;
			InterfaceContainer.Instance.talentDescriptionWindow.transform.position = new Vector3 (UICamera.lastHit.point.x, UICamera.lastHit.point.y, 0);
		} else {
			InterfaceContainer.Instance.talentDescriptionWindow.gameObject.SetActive (false);
		}
	}
}
