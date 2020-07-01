using UnityEngine;
using System.Collections;

public class CharacterSlot : MonoBehaviour {
	public bool defaultCharacter;
	public Character character;
	
	private void Start(){
		if(defaultCharacter){
			OnClick();
			GetComponent<UIActivate>().OnClick();
			GetComponent<UIMassActivate>().OnClick();
		}
	}
	
	private void OnClick(){
		DataStorage.Instance.character=character;
	}
}
