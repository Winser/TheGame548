using UnityEngine;
using System.Collections;

public class RegisterAccount : MonoBehaviour {
	public UIInput username;
	public UIInput password;
	public UIInput confirmPassword;
	public UIInput email;
	public UILabel error;
	public GameObject registerWindow;
	public GameObject startMenuWindow;
	
	private void OnClick(){
		if(username.text.Equals(string.Empty) || password.text.Equals(string.Empty) || confirmPassword.text.Equals(string.Empty) || email.text.Equals(string.Empty)){
			error.text="You need to complete all fields!";
		}else if(!password.text.Equals(confirmPassword.text)){
			error.text="Password does not match the confirm password!";
		}else if(!email.text.Contains("@")){
			error.text="Please enter correct email!";
		}else{
			error.text="";
			StartCoroutine(GameManager.GameDatabase.RegisterProfile(username.text,password.text,email.text,gameObject));
		}
		
	}
	
	private void OnSuccess(){
		registerWindow.SetActive(false);
		startMenuWindow.SetActive(true);
	}
	
	private void OnFail(){
		error.text="Acount already exists!";
	}
}
