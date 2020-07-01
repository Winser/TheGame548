using UnityEngine;
using System.Collections;

/// <summary>
/// Helper class containing reference to general game assets
/// </summary>
public static class GameManager{
	/// <summary>
	/// Reference to GameSettings asset
	/// </summary>
	private static GameSettings gameSettings;
	public static GameSettings GameSettings{
		get{
			if(!gameSettings){
				gameSettings= (GameSettings)Resources.Load("Internal/GameSettings", typeof(GameSettings));
			}
			return gameSettings;
		}
	}
	
	/// <summary>
	/// Reference to GamePefabs asset
	/// </summary>
	private static GamePrefabs gamePrefabs;
	public static GamePrefabs GamePrefabs{
		get{
			if(!gamePrefabs){
				gamePrefabs=(GamePrefabs)Resources.Load("Internal/GamePrefabs",typeof(GamePrefabs));
			}
			return gamePrefabs;
		}
	}
	
	/// <summary>
	/// Reference to ItemDatabase asset
	/// </summary>
	private static ItemDatabase itemDatabase;
	public static ItemDatabase ItemDatabase{
		get{
			if(!itemDatabase){
				itemDatabase=(ItemDatabase)Resources.Load("Internal/ItemDatabase",typeof(ItemDatabase));
			}
			return itemDatabase;
		}
	}
	
	/// <summary>
	/// Referece to TalentDatabase asset
	/// </summary>
	private static TalentDatabase talentDatabase;
	public static TalentDatabase TalentDatabase{
		get{
			if(!talentDatabase){
				talentDatabase= (TalentDatabase)Resources.Load("Internal/TalentDatabase",typeof(TalentDatabase));
			}
			return talentDatabase;
		}
	}
	
	/// <summary>
	/// Referece to the InputSettings asset
	/// </summary>
	private static InputSettings inputSettings;
	public static InputSettings InputSettings{
		get{
			if(!inputSettings){
				inputSettings= (InputSettings)Resources.Load("Internal/InputSettings",typeof(InputSettings));
			}
			return inputSettings;
		}
	}
	
	/// <summary>
	/// Referece to the PlayerSettings asset
	/// </summary>
	private static PlayerSettings playerSettings;
	public static PlayerSettings PlayerSettings{
		get{
			if(!playerSettings){
				playerSettings= (PlayerSettings)Resources.Load("Internal/PlayerSettings",typeof(PlayerSettings));
			}
			return playerSettings;
		}
	}
	
	/// <summary>
	/// Reference to the GameMessages asset
	/// </summary>
	private static GameMessages gameMessages;
	public static GameMessages GameMessages{
		get{
			if(!gameMessages){
				gameMessages= (GameMessages)Resources.Load("Internal/GameMessages",typeof(GameMessages));
			}
			return gameMessages;
		}
	}
	
	/// <summary>
	/// Reference to the GameDatabase asset
	/// </summary>
	private static GameDatabase gameDatabase;
	public static GameDatabase GameDatabase{
		get{
			if(!gameDatabase){
				gameDatabase= (GameDatabase)Resources.Load("Internal/GameDatabase",typeof(GameDatabase));
			}
			return gameDatabase;
		}
	}
	
	/// <summary>
	/// Reference to the CharacterDatabase asset
	/// </summary>
	private static CharacterDatabase characterDatabase;
	public static CharacterDatabase CharacterDatabase{
		get{
			if(!characterDatabase){
				characterDatabase= (CharacterDatabase)Resources.Load("Internal/CharacterDatabase",typeof(CharacterDatabase));
			}
			return characterDatabase;
		}
	}
	
	/// <summary>
	/// The player data which is only availible at runtime.
	/// </summary>
	private static Player player;
	public static Player Player{
		get{return player;}
		set{player=value;}
	}
	
}
