using UnityEngine;
using System.Collections;

public class TeleporterTrigger : MonoBehaviour
{
	public Teleporter teleporter;
	
	private void OnTriggerEnter (Collider other)
	{
		if (other.tag.Equals ("Player")) {
			InterfaceContainer.Instance.teleporterWindow.SetActive (true);
			if (!TeleporterManager.Instance.ContainsLocation (teleporter)) {
				TeleporterManager.Instance.AddToTable (teleporter, transform.position);
			}
		
			TeleporterManager.Instance.SetCurrentLocation (teleporter);
		}
	}
	
	
	private void OnTriggerExit (Collider other)
	{
		if (other.tag.Equals ("Player") && !TeleporterManager.Instance.teleported) {
			StartCoroutine(DisableWindow());
		}
	}
	
	private IEnumerator DisableWindow(){
		yield return new WaitForSeconds(0.1f);
		InterfaceContainer.Instance.teleporterWindow.SetActive (false);
	}
}
