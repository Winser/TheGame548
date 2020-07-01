using UnityEngine;
using System.Collections;
using UnityEditor;

public class CustomAsset{
	[MenuItem("Assets/Create/Text Template")]
    public static void CreateTextAsset ()
    {
        CustomAssetUtility.CreateAsset<TextTemplate>();
    }
	
	[MenuItem("Assets/Create/GameSettings")]
    public static void CreateGameSettingsAsset ()
    {
        CustomAssetUtility.CreateAsset<GameSettings>();
    }
	
	[MenuItem("Assets/Create/GamePrefabs")]
    public static void CreateGamePrefabsAsset ()
    {
        CustomAssetUtility.CreateAsset<GamePrefabs>();
    }
	
	[MenuItem("Assets/Create/GameMessages")]
    public static void CreateGameMessagesAsset ()
    {
        CustomAssetUtility.CreateAsset<GameMessages>();
    }
	
	[MenuItem("Assets/Create/Teleporter")]
    public static void CreateTeleporterAsset ()
    {
        CustomAssetUtility.CreateAsset<Teleporter>();
    }
	
	[MenuItem("Assets/Create/ItemDatabase")]
    public static void CreateItemDatabaseAsset ()
    {
        CustomAssetUtility.CreateAsset<ItemDatabase>();
    }
	
	[MenuItem("Assets/Create/TalentDatabase")]
    public static void CreateTalentDatabaseAsset ()
    {
        CustomAssetUtility.CreateAsset<TalentDatabase>();
    }
	
	[MenuItem("Assets/Create/InputSettings")]
    public static void CreateInputSettingsAsset ()
    {
        CustomAssetUtility.CreateAsset<InputSettings>();
    }
	
	[MenuItem("Assets/Create/PlayerSettings")]
    public static void CreatePlayerSettingsAsset ()
    {
        CustomAssetUtility.CreateAsset<PlayerSettings>();
    }
	
	[MenuItem("Assets/Create/Attribute")]
    public static void CreateAttributeAsset ()
    {
        CustomAssetUtility.CreateAsset<PlayerAttribute>();
    }
	
	[MenuItem("Assets/Create/Talent/Meele")]
    public static void CreateMeeleTalentAsset ()
    {
        CustomAssetUtility.CreateAsset<MeeleTalent>();
    }
	
	[MenuItem("Assets/Create/Talent/MeeleCombo")]
    public static void CreateMeeleComboTalentAsset ()
    {
        CustomAssetUtility.CreateAsset<MeeleComboTalent>();
    }
	
	[MenuItem("Assets/Create/Talent/Missle")]
    public static void CreateMissleTalentAsset ()
    {
        CustomAssetUtility.CreateAsset<MissleTalent>();
    }
	
	[MenuItem("Assets/Create/Talent/AOE/GiveTarget")]
    public static void CreateGiveTargetTalentAsset ()
    {
        CustomAssetUtility.CreateAsset<GiveTargetTalent>();
    }
	
	[MenuItem("Assets/Create/Talent/AOE/Target")]
    public static void CreateTargetTalentAsset ()
    {
        CustomAssetUtility.CreateAsset<TargetTalent>();
    }
	
	[MenuItem("Assets/Create/Talent/AOE/AroundSelf")]
    public static void CreateAroundSelfTalentAsset ()
    {
        CustomAssetUtility.CreateAsset<AroundSelfTalent>();
    }
	
	[MenuItem("Assets/Create/Talent/Buff")]
    public static void CreateBuffTalentAsset ()
    {
        CustomAssetUtility.CreateAsset<BuffTalent>();
    }
	
	[MenuItem("Assets/Create/Talent/Mount")]
    public static void CreateMountTalentAsset ()
    {
        CustomAssetUtility.CreateAsset<MountTalent>();
    }
	
	[MenuItem("Assets/Create/Talent/Move")]
    public static void CreateMoveTalentAsset ()
    {
        CustomAssetUtility.CreateAsset<MoveTalent>();
    }
	
	[MenuItem("Assets/Create/Item/Potion")]
    public static void CreatePotionAsset ()
    {
        CustomAssetUtility.CreateAsset<Potion>();
    }
	
	[MenuItem("Assets/Create/Item/BaseItem")]
    public static void CreateBaseItemAsset ()
    {
        CustomAssetUtility.CreateAsset<BaseItem>();
    }
	
	[MenuItem("Assets/Create/Item/SellableItem")]
    public static void CreateSellableItemAsset ()
    {
        CustomAssetUtility.CreateAsset<SellableItem>();
    }
	
	[MenuItem("Assets/Create/Item/TalentScroll")]
    public static void CreateTalentScrollAsset ()
    {
        CustomAssetUtility.CreateAsset<TalentScroll>();
    }
	
	[MenuItem("Assets/Create/Item/CollectableItem")]
    public static void CreateCollectableItemAsset ()
    {
        CustomAssetUtility.CreateAsset<CollectableItem>();
    }
	
	[MenuItem("Assets/Create/Item/EquipmentItem")]
    public static void CreateEquipmentItemAsset ()
    {
        CustomAssetUtility.CreateAsset<EquipmentItem>();
    }
	
	[MenuItem("Assets/Create/GameDatabase")]
    public static void CreateGameDatabaseAsset ()
    {
        CustomAssetUtility.CreateAsset<GameDatabase>();
    }
	
	[MenuItem("Assets/Create/CharacterDatabase")]
    public static void CreateCharacterDatabaseAsset ()
    {
        CustomAssetUtility.CreateAsset<CharacterDatabase>();
    }
	
	[MenuItem("Assets/Create/BugReporterInformation")]
    public static void CreateBugReporterInformationAsset ()
    {
        CustomAssetUtility.CreateAsset<BugReporterInformation>();
    }
}
