using UnityEngine;
using System.Collections;

public class LoginAccount : MonoBehaviour {
	public UIInput username;
	public UIInput password;
	public UILabel error;
	public GameObject loginButtonBackground;
	public GameObject loginLabel;
	public GameObject backButton;
	public UILabel verifyingUser;
	
	private void OnClick(){
		if(username.text.Equals(string.Empty) || password.Equals(string.Empty)){
			error.text="You need to complete all fields!";
			verifyingUser.text="";
		}else{
			error.text="";
			StartCoroutine(GameManager.GameDatabase.Login(username.text,password.text,gameObject));
			loginButtonBackground.SetActive(false);
			loginLabel.SetActive(false);
			backButton.SetActive(false);
			verifyingUser.text="Verifying user...";
		}
	}
	
	private void OnSuccess(){
		StartCoroutine(GameManager.GameDatabase.LoadPlayer(username.text,OnLoadPlayer));
	}
			
	private void OnLoadPlayer(byte[] data){
		if(data.Length < 10){
			Application.LoadLevel("Create");
		}else{
			if(PhotonNetwork.offlineMode){
				Application.LoadLevel("PreLoad");
			}else{
				Application.LoadLevel("Lobby");
			}
		}
	
	}
	
	private void OnFail(){
		verifyingUser.text="";
		error.text="Your username or password is wrong!";
		loginButtonBackground.SetActive(true);
		loginLabel.SetActive(true);
		backButton.SetActive(true);
		
	}
}
