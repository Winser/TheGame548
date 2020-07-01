using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Game settings.
/// </summary>
[System.Serializable]
public class GameSettings : ScriptableObject {
	public bool allowPvp;
	public string checkpointTag="Trigger";
	
	#if UNITY_EDITOR
	public void OnGUI(){
		GUI.changed=false;
		GUILayout.BeginVertical("Game Settings","box");
		GUILayoutUtility.GetRect(0,15);
		allowPvp=EditorGUILayout.Toggle("Allow PvP",allowPvp);
		checkpointTag=EditorGUILayout.TextField("Checkpoint Tag", checkpointTag);
		
		GUILayout.EndVertical();
		if(GUI.changed){
			EditorUtility.SetDirty(this);
		}
	}
	#endif
}
