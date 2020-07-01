using UnityEngine;
using System.Collections;

public class UITweenScale : MonoBehaviour
{
	public Vector3 scale;
	public float duration;
	public float delay;
	
	private void Start(){
		StartCoroutine(LerpScale(duration));
	}
	
	private IEnumerator LerpScale (float time)
	{
		yield return new WaitForSeconds(delay);
		
		Vector3 originalScale = transform.localScale;
		Vector3 targetScale = scale;
		float originalTime = time;
 
		while (time > 0.0f) {
			time -= Time.deltaTime;
 
			transform.localScale = Vector3.Lerp (targetScale, originalScale, time / originalTime);
 
			yield return null;
		}
	}
}
