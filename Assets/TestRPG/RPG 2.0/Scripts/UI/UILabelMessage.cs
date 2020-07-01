using UnityEngine;
using System.Collections;

public class UILabelMessage : MonoBehaviour {
	public GameObject target;
	public UILabel label;
	public string function;
	
	private void OnClick(){
		target.SendMessage(function,label.text,SendMessageOptions.DontRequireReceiver);
	}
}
