using UnityEngine;
using System.Collections;

public class RaiseAttribute : MonoBehaviour {
	public PlayerAttributeSlot attributeSlot;
	
	private void OnClick(){
		if(GameManager.Player.FreeAttributePoints>0){
			attributeSlot.attribute.BaseValue+=1;
			GameManager.Player.FreeAttributePoints-=1;
		}
	}
}
