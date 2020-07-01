using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TalentEditorWindow : EditorWindow
{
	
	[MenuItem ("RPG Kit 2.0/Editor/Talent")]
	public static void Init ()
	{
		editor=(TalentEditorWindow)EditorWindow.GetWindow (typeof(TalentEditorWindow));
	}
	private BaseTalent talent;
	private static TalentEditorWindow editor;
	
	private void OnGUI(){
		if(editor== null){
			editor=(TalentEditorWindow)EditorWindow.GetWindow (typeof(TalentEditorWindow));
		}
		
		DrawMenu();
		GUI.changed=false;
		if(talent != null){
			talent.OnGUI();
		}
		
		if(GUI.changed && talent != null){
			EditorUtility.SetDirty(talent);
		}
	}
	
	private void DrawMenu ()
	{
		GUILayout.BeginHorizontal (EditorStyles.toolbar);
		if (GUILayout.Button ("File", EditorStyles.toolbarDropDown, GUILayout.Width (60))) {
			GenericMenu toolsMenu = new GenericMenu ();
			toolsMenu.AddItem (new GUIContent ("Open"), false, Open);
			toolsMenu.AddItem (new GUIContent ("Create/Meele"), false, Create, ScriptableObject.CreateInstance<MeeleTalent>());
			toolsMenu.AddItem (new GUIContent ("Create/Missle"), false, Create,ScriptableObject.CreateInstance<MissleTalent>());
			toolsMenu.AddItem (new GUIContent ("Create/Buff"), false, Create,ScriptableObject.CreateInstance<BuffTalent>());
			toolsMenu.AddItem (new GUIContent ("Create/Mount"), false, Create,ScriptableObject.CreateInstance<MountTalent>());
			toolsMenu.AddItem (new GUIContent ("Create/AOE/Give Target"), false, Create, ScriptableObject.CreateInstance<GiveTargetTalent>());
			toolsMenu.AddItem (new GUIContent ("Create/AOE/Target"), false, Create,ScriptableObject.CreateInstance<TargetTalent>());
			toolsMenu.AddItem (new GUIContent ("Create/AOE/Around Self"), false, Create,ScriptableObject.CreateInstance<AroundSelfTalent>());
			toolsMenu.AddItem (new GUIContent ("Create/Special/Meele Combo"), false, Create,ScriptableObject.CreateInstance<MeeleComboTalent>());
			toolsMenu.AddItem (new GUIContent ("Create/Special/Run"), false, Create,ScriptableObject.CreateInstance<MoveTalent>());
			
			toolsMenu.DropDown (new Rect (0, 0, 0, 16));
			EditorGUIUtility.ExitGUI ();
		}
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal();
	}
	
	private void Create(object data)
	{
		string mPath = EditorUtility.SaveFilePanelInProject (
         	       "Create Talent Asset",
            	   "New "+data.GetType().ToString()+".asset",
                   "asset", "");
		AssetDatabase.CreateAsset ((BaseTalent)data, mPath);
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = (BaseTalent)data;
		talent=(BaseTalent)data;
		GUIUtility.keyboardControl = 0;
		GameManager.TalentDatabase.AddTalent(talent);
	}
	
	private void Open ()
	{
		string mPath = EditorUtility.OpenFilePanel (
                "Open Talent",
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
		talent=(BaseTalent)AssetDatabase.LoadAssetAtPath(mPath,typeof(BaseTalent));
	}
	
	private void OnDisable(){
		Resources.UnloadUnusedAssets();
	}
	
}
