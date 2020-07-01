using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {
	public GameObject mySqlMenu;
	public GameObject binaryMenu;
	
	private void OnClick(){
		switch(GameManager.GameDatabase.databaseType){
		case DatabaseType.Binary:
			binaryMenu.SetActive(true);
			mySqlMenu.SetActive(false);
			break;
		case DatabaseType.MySql:
			mySqlMenu.SetActive(true);
			binaryMenu.SetActive(false);
			break;
		}
	}
}
