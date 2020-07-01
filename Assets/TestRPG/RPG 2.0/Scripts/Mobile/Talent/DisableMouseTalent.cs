using UnityEngine;
using System.Collections;

public class DisableMouseTalent : MonoBehaviour {

	private void OnMouseEnter(){
		SlotContainer.disableMouseTalent=true;
	}
	
	private void OnMouseExit(){
		SlotContainer.disableMouseTalent=false;
	}
}
