using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class that contains all talents.
/// </summary>
[System.Serializable]
public class TalentDatabase : ScriptableObject {
	public List<BaseTalent> talents;
	
	/// <summary>
	/// Finds the talent by name
	/// </summary>
	/// <returns>
	/// The talent asset.
	/// </returns>
	/// <param name='talentName'>
	/// Talent name.
	/// </param>
	public BaseTalent GetTalent(string talentName){
		return talents.Find(talent => talent.talentName == talentName);
	}
	
	/// <summary>
	/// Adds the talent to the this collection. Should be used only in editor.
	/// </summary>
	/// <param name='talent'>
	/// Talent asset
	/// </param>
	public void AddTalent(BaseTalent talent){
		if(!talents.Contains(talent)){
			talents.Add(talent);
		}
	}
}
