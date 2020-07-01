using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Class that contains all character assets
[System.Serializable]
public class CharacterDatabase : ScriptableObject {
	public List<Character> characters;
	
	/// <summary>
	/// Gets the character.
	/// </summary>
	/// <returns>
	/// The character.
	/// </returns>
	/// <param name='characterClass'>
	/// Character class.
	/// </param>
	public Character GetCharacter(string characterClass){
		return characters.Find(character=>character.characterClass==characterClass);
	}
	
	/// <summary>
	/// Adds the character. Used internal in editor.
	/// </summary>
	/// <param name='character'>
	/// Character.
	/// </param>
	public void AddCharacter(Character character){
		characters.Add(character);
	}
}
