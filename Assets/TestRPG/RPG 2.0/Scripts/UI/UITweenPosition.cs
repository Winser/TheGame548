using UnityEngine;
using System.Collections;

public class UITweenPosition : MonoBehaviour {
	public Vector3 direction;
	public float duration;
	public float delay;
	
	private void Start(){
		StartCoroutine(LerpPosition(duration));
	}
	
	private IEnumerator LerpPosition (float time)
	{
		yield return new WaitForSeconds(delay);
		
		Vector3 originalPosition = transform.position;
		Vector3 targetPosition = transform.position+direction;
		float originalTime = time;
 
		while (time > 0.0f) {
			time -= Time.deltaTime;
 
			transform.position = Vector3.Lerp (targetPosition, originalPosition, time / originalTime);
 
			yield return null;
		}
	}
}
