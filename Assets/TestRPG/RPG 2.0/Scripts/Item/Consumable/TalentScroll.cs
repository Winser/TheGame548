using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class TalentScroll : ConsumableItem {
	public BaseTalent talent;
	
	public override bool Use ()
	{
		if(!base.Use ()){
			return false;
		}
		if(GameManager.Player.GetTalent(talent.talentName) == null){
			talent.spentPoints=1;
			GameManager.Player.AddTalent(talent);
			GameManager.Player.Character.talents.Add(talent);
		}else{
			GameManager.Player.GetTalent(talent.talentName).spentPoints+=1;
		}
		
		return true;
	}
	
	#if UNITY_EDITOR
	public override void OnGUI ()
	{
		base.OnGUI ();
		talent=(BaseTalent)EditorGUILayout.ObjectField("Talent",talent,typeof(BaseTalent),false);
		
	}
	#endif
}
