using UnityEngine;
using System.Collections;
using UnityEditor;

public class CharacterEditorWindow : EditorWindow {
	[MenuItem ("RPG Kit 2.0/Editor/Character")]
	public static void Init ()
	{
		editor = (CharacterEditorWindow)EditorWindow.GetWindow (typeof(CharacterEditorWindow));
	}
	
	private Character character;
	private Vector2 scroll;
	private static CharacterEditorWindow editor;
	
	private void OnGUI(){
		if(editor == null){
			editor = (CharacterEditorWindow)EditorWindow.GetWindow (typeof(CharacterEditorWindow));
		}
		
		GUILayout.BeginHorizontal (EditorStyles.toolbar);
		DrawToolStrip ();
		GUILayout.EndHorizontal ();
		
		scroll = EditorGUILayout.BeginScrollView (scroll);
		GUI.changed=false;
		if(character != null){
			character.OnGUI();
		}
		if(GUI.changed){
			EditorUtility.SetDirty(character);
		}
		EditorGUILayout.EndScrollView();
	}
	
	private void DrawToolStrip ()
	{
		if (GUILayout.Button ("File", EditorStyles.toolbarDropDown, GUILayout.Width (60))) {
			GenericMenu toolsMenu = new GenericMenu ();
			toolsMenu.AddItem (new GUIContent ("Create"), false, Create, ScriptableObject.CreateInstance<Character>());
			toolsMenu.AddItem (new GUIContent ("Open"), false, Open);
		
			toolsMenu.DropDown (new Rect (0, 0, 0, 16));
			EditorGUIUtility.ExitGUI ();
		}
		
		GUI.enabled=character != null && character.prefab != null;
		if(GUILayout.Button("Add Components", EditorStyles.toolbarButton,GUILayout.Width(120))){
			if(character && character.prefab){
				GameObject empty= new GameObject(character.characterClass.Equals(string.Empty)?character.prefab.name:character.characterClass); 
				AddCharacterComponents(empty,character.prefab);
			}
		}
		GUI.enabled=true;
		GUILayout.FlexibleSpace ();
	}
	
	private void Create(object data)
	{
		
		string mPath = EditorUtility.SaveFilePanelInProject (
         	       "Create Character Asset",
            	   	data.GetType().ToString() + ".asset",
                	"asset", "");
		
		if(mPath.Equals(string.Empty)){
			return;
		}
		AssetDatabase.CreateAsset ((Character)data, mPath);
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = character;
		
		character = (Character)data;
		GameManager.CharacterDatabase.AddCharacter(character);
		EditorUtility.SetDirty(GameManager.CharacterDatabase);
	}
	
	private void Open ()
	{
		string mPath = EditorUtility.OpenFilePanel (
                "Open Character",
                "",
                "asset");
		string[] splitPath= mPath.Split('/');
		mPath=string.Empty;
		foreach(string s in splitPath){
			if(s.Equals("Assets") || !mPath.Equals(string.Empty)){
				mPath+=s+"/";
			}
		}
		mPath = mPath.Remove(mPath.Length - 1);
		character =(Character)AssetDatabase.LoadAssetAtPath(mPath,typeof(Character));
	}
	
	private GameObject CreatePrefab (GameObject obj)
	{
		string mPath = EditorUtility.SaveFilePanelInProject (
         	       "Save Character Prefab",
            	    character.characterClass + ".prefab",
                	"prefab", "");
		
		if(mPath.Equals(string.Empty)){
			return null;
		}
		GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath (mPath, typeof(GameObject));
		if (prefab != obj) {
			prefab = (GameObject)PrefabUtility.CreatePrefab (mPath, obj, ReplacePrefabOptions.ConnectToPrefab);
		}
		return prefab;
	}
	
	private void AddCharacterComponents(GameObject empty,GameObject prefab){
		//Retruns false if the prefab is in scene
		bool persistent = EditorUtility.IsPersistent(prefab);
		if(persistent){
			prefab=(GameObject)Instantiate(prefab);
		}
		
		prefab.transform.parent=empty.transform;
		prefab.transform.localPosition=Vector3.zero;
		prefab.transform.localRotation=Quaternion.Euler(Vector3.zero);
		
		empty.AddComponent<CharacterController>();
		PhotonView view=empty.AddComponent<PhotonView>();
		view.observed=empty.AddComponent<PhotonNetworkPlayer>();
		view.synchronization= ViewSynchronization.ReliableDeltaCompressed;
		
		ThirdPersonMovement mMovement=empty.AddComponent<ThirdPersonMovement>();
		mMovement.playerAnimation=prefab.GetComponent<Animation>();
		mMovement.character=character;
		
		empty.AddComponent<PlayerEquipment>();
		empty.AddComponent<DontDestroyOnLevelChange>();
		empty.AddComponent<MeshHolder>();
		empty.tag=GameManager.PlayerSettings.playerTag;
		empty.AddComponent<AudioSource>();
		
		GameObject projectPrefab= CreatePrefab(empty);
		character.prefab=projectPrefab;
		
		Selection.activeTransform=empty.transform;
		SceneView.lastActiveSceneView.FrameSelected();
	}
	
	private void OnDisable(){
		Resources.UnloadUnusedAssets();
	}
}
