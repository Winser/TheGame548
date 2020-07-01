using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Give target talent.This talent gives the player a visual Target.
/// </summary>
[System.Serializable]
public class GiveTargetTalent : AreaOfEffectTalent {
	//Can we walk with the target?
	public bool canWalk;
	
	/// <summary>
	///  Use this talent 
	/// </summary>
	public override bool Use ()
	{
		//Check if we can use this talent at all
		if(!base.Use()){
			return false;
		}
		//Set this talent to the visual Target, so we know in AOETarget.cs which talent to use.
		AOETarget.Instance.talent=this;
		//Enable the visual Target after instantiate delay.
		UnityTools.StartCoroutine(EnableTarget(instantiateDelay));
		return true;
	}
	
	/// <summary>
	/// Enables the visual target after delay
	/// </summary>
	/// <param name='delay'>
	/// Delay.
	/// </param>
	private IEnumerator EnableTarget(float delay){
		//Wait 
		yield return new WaitForSeconds(delay);
		//Enable target
		AOETarget.Instance.gameObject.SetActive(true);
	}
	
	#if UNITY_EDITOR	
	public override void OnGUI(){
		base.OnGUI();
		canWalk=EditorGUILayout.Toggle("Can Walk",canWalk);
	}
	#endif
}
