using UnityEngine;
using System.Collections;

public class CloseContainer : MonoBehaviour {
	private void OnClick(){
		ItemContainer.lastContainer.Close();
	}
}
