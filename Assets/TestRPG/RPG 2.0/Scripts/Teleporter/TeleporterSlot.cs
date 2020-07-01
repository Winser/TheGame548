using UnityEngine;
using System.Collections;

public class TeleporterSlot : MonoBehaviour {
	public UISprite currentLocationSprite;
	public UILabel locationName;
	
	
	private void OnClick(){
		TeleporterManager.Instance.Teleport(this);
	}
}
