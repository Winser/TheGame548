using UnityEngine;
using System.Collections;

public class UITweenColor : MonoBehaviour {

	public Color color;
	public float duration;
	public float delay;
	public UIWidget target;
	
	private void Start(){
		if(target != null){
			backupColor=target.color;
		}
		StartCoroutine(LerpColor(duration));
		
	}
	
	public IEnumerator LerpColor(float time)
	{
		yield return new WaitForSeconds(delay);
		
		Color originalColor = target.color;
		Color targetColor = color;
		float originalTime = time;
 
		while (time > 0.0f) {
			time -= Time.deltaTime;
 
			target.color = Color.Lerp (targetColor, originalColor, time / originalTime);
 
			yield return null;
		}
	}
	
	private Color backupColor;
	public void Reset(){
		target.color=backupColor;
		StartCoroutine(LerpColor(duration));
	}
}
