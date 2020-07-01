using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// Usable item class.
/// </summary>
[System.Serializable]
public class UsableItem : SellableItem {
	//Using cool down
	public float coolDown;
	//Item is only usable by this class
	public List<string> characterClasses;
	
	/// <summary>
	/// Can we use the item again after the delay.
	/// </summary>
	[System.NonSerialized]
	protected bool canUse=true;
	
	/// <summary>
	/// Use this Item.
	/// </summary>
	public virtual bool Use(){
		//Do not use item if player is dead or if the item is cooling down.
		if(GameManager.Player.Dead || !canUse ){
			return false;
		}
		
		if(!characterClasses.Contains(GameManager.Player.Character.characterClass)){
			MessageManager.Instance.AddMessage(GameManager.GameMessages.canNotUse);
			return false;
		}
		
		//Start the usage delay
		UnityTools.StartCoroutine(Delay());
		return true;
	}
	
	/// <summary>
	/// Delay this Item.
	/// </summary>
	public virtual IEnumerator Delay(){
		canUse=false;
		yield return new WaitForSeconds(coolDown);
		canUse=true;
		
	}
	
	#if UNITY_EDITOR	
	public override void OnGUI(){
		base.OnGUI();
		coolDown=EditorGUILayout.FloatField("Cool Down", coolDown);

		if(GUILayout.Button("Add Character Class")){
			characterClasses.Add("");
		}
		int index=-1;
		if(characterClasses.Count<1){
			characterClasses.Add("");
		}
		for(int i=0; i< characterClasses.Count; i++){
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("X",GUILayout.Width(20))){
				index=i;
			}
			characterClasses[i]=EditorGUILayout.TextField("Class",characterClasses[i]);
			GUILayout.EndHorizontal();
		}
		
		if(index != -1){
			characterClasses.RemoveAt(index);
		}
	}
	#endif
}
