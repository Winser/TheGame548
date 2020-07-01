using UnityEngine;
using System.Collections;

public class PlayerAttributeSlot : MonoBehaviour {
	[SerializeField]
	private UILabel displayName;
	[SerializeField]
	private UILabel attributeName;
	[SerializeField]
	private UILabel attributeValue;
	public PlayerAttribute attribute;
	[SerializeField]
	private GameObject raiseButton;
	
	public void Init(PlayerAttribute attr){
		attribute=attr;
		displayName.text=attr.displayName;
		attributeName.text=attr.attributeName;
		attributeName.color=attr.color;
		attributeValue.color=attr.color;
	}
	
	private void Update(){
		attributeValue.text=attribute.CurValue.ToString()+"/"+(attribute.BaseValue+ attribute.TempValue);
		if(GameManager.Player.FreeAttributePoints>0 && attribute.raisable){
			raiseButton.SetActive(true);
		}else{
			raiseButton.SetActive(false);
		}
	}
}
