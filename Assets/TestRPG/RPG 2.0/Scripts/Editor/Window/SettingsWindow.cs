using UnityEngine;
using System.Collections;
using UnityEditor;

public class SettingsWindow : EditorWindow {
	[MenuItem ("RPG Kit 2.0/Settings")]
	public static void Init ()
	{
		editor=(SettingsWindow) EditorWindow.GetWindow (typeof(SettingsWindow));
		editor.minSize=new Vector2(300,400);
	}
	
	private static SettingsWindow editor;
	private Vector2 scroll;
	private void OnGUI(){
		if(editor == null){
			editor=(SettingsWindow) EditorWindow.GetWindow (typeof(SettingsWindow));
		}
		
		scroll= GUILayout.BeginScrollView(scroll);
		
		//Base game settings
		GameManager.GameSettings.OnGUI();
		//Base player settings
		GameManager.PlayerSettings.OnGUI();
		//Input settings
		GameManager.InputSettings.OnGUI();
		//Game messages
		GameManager.GameMessages.OnGUI();
		//Database
		GameManager.GameDatabase.OnGUI();
		
		GUILayout.EndScrollView();
	}
}
