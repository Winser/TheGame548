using UnityEngine;
using System.Collections;

public class RepeatCharsInput : MonoBehaviour {
	public string text;
	public float delay;
	public bool repeat;
	public UILabel label;
	
	private void Start(){
		StartCoroutine(StartInput());
	}
	
	private IEnumerator StartInput(){
		char[] chars= text.ToCharArray();
		label.text="";
		foreach(char c in chars){
			yield return new WaitForSeconds(delay);
			label.text=label.text+ c.ToString();
		}
		yield return new WaitForSeconds(delay);
		if(repeat){
			StartCoroutine(StartInput());
		}
	}
}
