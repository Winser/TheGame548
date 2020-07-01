using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Storage class to setup all inputs at one place
/// </summary>
[System.Serializable]
public class InputSettings : ScriptableObject {
	public KeyCode profile= KeyCode.P;
	public KeyCode talent=KeyCode.T;
	public KeyCode settings=KeyCode.Z;
	public KeyCode chat= KeyCode.Return;
	public KeyCode quest= KeyCode.Q;
	public KeyCode close= KeyCode.Escape;
	
	#if UNITY_EDITOR
	public void OnGUI(){
		GUI.changed=false;
		GUILayout.BeginVertical("Input Settings","box");
		GUILayoutUtility.GetRect(0,15);
		profile=(KeyCode)EditorGUILayout.EnumPopup("Profile",profile);
		settings=(KeyCode)EditorGUILayout.EnumPopup("Settings",settings);
		quest=(KeyCode)EditorGUILayout.EnumPopup("Quest",quest);
		talent=(KeyCode)EditorGUILayout.EnumPopup("Talent",talent);
		chat=(KeyCode)EditorGUILayout.EnumPopup("Chat",chat);
		close=(KeyCode)EditorGUILayout.EnumPopup("Close",close);
		GUILayout.EndVertical();
		if(GUI.changed){
			EditorUtility.SetDirty(this);
		}
	}
	#endif
}


