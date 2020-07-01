using UnityEngine;
using System.Collections;

public enum ShowTrigger{
	Always,
	OnMouseOver,
	OnClick,
}

public class DisplayName : MonoBehaviour {
	public static DisplayName lastName;
	
	public string nameToDisplay;
	public bool random;
	public TextTemplate nameTemplate;
	public Color color=Color.white;
	public UILabel nameLabel;
	public ShowTrigger showTrigger=ShowTrigger.Always;
	
	private void Start(){
		if(random && nameTemplate != null){
			nameLabel.text=nameTemplate.GetRandomText();
		}else{
			nameLabel.text=nameToDisplay;
		}
		nameLabel.color=color;
		
		if( showTrigger != ShowTrigger.Always){
			nameLabel.gameObject.SetActive(false);
		}
	}
	
	private void OnMouseEnter(){
		if(showTrigger == ShowTrigger.OnMouseOver){
			nameLabel.gameObject.SetActive(true);
		}
	}
	
	private void OnMouseExit(){
		if(showTrigger == ShowTrigger.OnMouseOver){
			nameLabel.gameObject.SetActive(false);
		}
	}
	
	private void OnMouseUp(){
		if(showTrigger== ShowTrigger.OnClick){
			if(DisplayName.lastName != null){
				lastName.nameLabel.gameObject.SetActive(false);
			}
			lastName=this;
			nameLabel.gameObject.SetActive(true);
		}
	}
}
