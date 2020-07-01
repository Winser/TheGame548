using UnityEngine;
using System.Collections;
using System.IO;

public class ContinueGame : MonoBehaviour {
	public string level="PreLoad";
	
	private void OnClick(){
		if (!File.Exists(Application.dataPath + "/" + GameManager.GameDatabase.sceneDataFile + ".bytes") || !File.Exists(Application.dataPath + "/" + GameManager.GameDatabase.playerDataFile + ".bytes")) {
			Debug.Log("There is no saved data!");
			return;
		}
		
		Application.LoadLevel(level);
	}
}
