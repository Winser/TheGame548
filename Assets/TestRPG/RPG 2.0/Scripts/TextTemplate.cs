using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TextTemplate : ScriptableObject {
	public List<string> text;
	
	public string GetRandomText(){
		string t=string.Empty;
		if(text.Count>0){
			t=text[Random.Range( 0,text.Count)];
		}
		return t;
	}
}
