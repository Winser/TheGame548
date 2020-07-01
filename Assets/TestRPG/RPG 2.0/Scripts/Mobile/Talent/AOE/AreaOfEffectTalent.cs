using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Base class for AOE talents
/// </summary>
[System.Serializable]
public class AreaOfEffectTalent : ProjectileTalent {
	//Range to apply damage around
	public float aoeRange;
	
	/// <summary>
	///  Use this talent 
	/// </summary>
	public override bool Use ()
	{
		return base.Use ();
	}
	
	#if UNITY_EDITOR	
	public override void OnGUI(){
		base.OnGUI();
		aoeRange=EditorGUILayout.FloatField("AOE Range",aoeRange);
	}
	#endif
}
