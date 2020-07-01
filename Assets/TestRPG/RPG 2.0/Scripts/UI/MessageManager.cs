using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageManager : MonoBehaviour
{
	private static MessageManager instance;
	
	public static MessageManager Instance {
		get{ return instance;}
	}
	
	private void Awake ()
	{
		instance = this;
	}
	
	private void Start ()
	{
		AddMessage (GameManager.GameMessages.welcome);
	}
	
	public void AddMessage (string message)
	{
		InterfaceContainer.Instance.messageStorageLabel.color = Color.white;
		InterfaceContainer.Instance.messageStorageLabel.text += "\n" + message;
		time = -2;
	}
	
	float time;

	private void Update ()
	{
		InterfaceContainer.Instance.messageStorageLabel.color = Color.Lerp (Color.white, new Color (1, 1, 1, 0), time);
		
		if (time < 1) { 
			time += Time.deltaTime / 2;
		}
		
		if(InterfaceContainer.Instance.messageStorageLabel.color.a <0.1f){
			InterfaceContainer.Instance.messageStorageLabel.text="";
		}
		
	}
}
