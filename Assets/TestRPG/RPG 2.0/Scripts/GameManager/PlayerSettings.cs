using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Player settings.
/// </summary>
[System.Serializable]
public class PlayerSettings : ScriptableObject {
	/// <summary>
	/// How many gold does the player has on start
	/// </summary>
	public int gold=0;
	/// <summary>
	/// How many free talent points does the player has on start 
	/// </summary>
	public int freeTalentPoints=1;
	/// <summary>
	/// How many free attribute points does the player has on start 
	/// </summary>
	public int freeAttributePoints=5;
	/// <summary>
	/// The player tag.
	/// </summary>
	public string playerTag="Player";
	/// <summary>
	/// The remote player tag.
	/// </summary>
	public string remotePlayerTag="RemotePlayer";
	/// <summary>
	/// Should we display the player name over head
	/// </summary>
	public bool nameOverHead;
	/// <summary>
	/// If you do not want to use a mouse click talent, just set this to null in the inspector.
	/// </summary>
	public DamageTalent mouseTalent;
	/// <summary>
	/// The respawn delay.
	/// </summary>
	public float respawnDelay=5;
	/// <summary>
	/// Maximum level that a player can reach.
	/// </summary>
	public int maxLevel=99;
	/// <summary>
	/// Should the camera shake on player hit
	/// </summary>
	public bool shakeCamera=true;
	
	#if UNITY_EDITOR
	public void OnGUI(){
		GUI.changed=false;
		GUILayout.BeginVertical("Player Settings","box");
		GUILayoutUtility.GetRect(0,15);
		playerTag=EditorGUILayout.TextField("Player Tag",playerTag);
		remotePlayerTag=EditorGUILayout.TextField("Remote Player Tag", remotePlayerTag);
		respawnDelay=EditorGUILayout.FloatField("Respawn Delay",respawnDelay);
		gold=EditorGUILayout.IntField("Start Gold",gold);
		freeTalentPoints=EditorGUILayout.IntField("Start Talent Points", freeTalentPoints);
		freeAttributePoints=EditorGUILayout.IntField("Start Attribute Points",freeAttributePoints);
		nameOverHead=EditorGUILayout.Toggle("Display Name Over Head",nameOverHead);
		mouseTalent=(DamageTalent)EditorGUILayout.ObjectField("Mouse Talent",mouseTalent, typeof(DamageTalent),false);
		maxLevel=EditorGUILayout.IntField("Max Level",maxLevel);
		shakeCamera=EditorGUILayout.Toggle("Shake Camera",shakeCamera);
		
		GUILayout.EndVertical();
		if(GUI.changed){
			EditorUtility.SetDirty(this);
		}
	}
	#endif
}
