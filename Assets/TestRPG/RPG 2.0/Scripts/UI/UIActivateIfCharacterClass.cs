using UnityEngine;
using System.Collections;

public class UIActivateIfCharacterClass : MonoBehaviour {
	public string characterClass;
	
	private void OnPlayerSpawn(){
		if( GameManager.Player.Character.characterClass.Equals(characterClass)){
			gameObject.GetComponent<UISprite>().enabled=true;
		}else{
			gameObject.GetComponent<UISprite>().enabled=false;
		}
	}
	
	
}
