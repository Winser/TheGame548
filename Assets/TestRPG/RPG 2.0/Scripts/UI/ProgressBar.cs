using UnityEngine;
using System.Collections;

public delegate void ProgressCallback();

public class ProgressBar : MonoBehaviour {
	public Transform progressBar;
	private float initTime;
	private float perc;
	private bool start;
	private float duration = 5;
	private float barLength;
	public bool isDone=true;
	private GameObject receiver;
	private string message;
	private ProgressCallback progressCallback;
	
	private void Start(){
		barLength=progressBar.localScale.x;
	}
	
	private void Update(){
		if (Time.time - initTime <= duration && start == true) {
			isDone=false;
			perc =((Time.time - initTime) / duration);
			progressBar.localScale= new Vector3(barLength*perc+0.001f,progressBar.localScale.y,progressBar.localScale.z);
		} else {
			isDone=true;
			
			if(progressCallback != null){
				progressCallback();
				progressCallback=null;
			}else{
				receiver.SendMessage(message,SendMessageOptions.DontRequireReceiver);
			}
			gameObject.SetActive(false);
		}
	}
	
	public void StartProgress(float delay, GameObject receiver,string message)
	{
		gameObject.SetActive(true);
		this.receiver=receiver;
		this.message=message;
		duration = delay;
		start = true;
		initTime = Time.time;
	}
	
	public void StartProgress(float delay,ProgressCallback callback)
	{
		gameObject.SetActive(true);
		duration = delay;
		start = true;
		initTime = Time.time;
		progressCallback=callback;
	}
}
