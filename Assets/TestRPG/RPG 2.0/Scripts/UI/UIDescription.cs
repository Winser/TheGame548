using UnityEngine;
using System.Collections;

public class UIDescription : MonoBehaviour {
		public UILabel target;
		public string desc;
		public Color color;
	
		private void OnHover (bool isOver){
		if(isOver){
			target.gameObject.SetActive (true);
			target.text=desc;
			target.color=color;
			target.transform.position = new Vector3 (UICamera.lastHit.point.x, UICamera.lastHit.point.y, target.transform.position.z);
		}else{
			target.color= Color.white;
			target.gameObject.SetActive (false);
		}
	}
	
}
