using UnityEngine;
using System.Collections;
using UnityEditor;

public class AttributeEditorWindow : EditorWindow {
	[MenuItem ("RPG Kit 2.0/Editor/Attribute")]
	public static void Init ()
	{
		editor=(AttributeEditorWindow)EditorWindow.GetWindow (typeof(AttributeEditorWindow));
	}
	
	private PlayerAttribute attribute;
	private Vector2 scroll;
	private static AttributeEditorWindow editor;
	
	private void OnGUI ()
	{
		if(editor == null){
			editor=(AttributeEditorWindow)EditorWindow.GetWindow (typeof(AttributeEditorWindow));
		}
		GUILayout.BeginHorizontal (EditorStyles.toolbar);
		DrawToolStrip ();
		GUILayout.EndHorizontal ();
		
		scroll = EditorGUILayout.BeginScrollView (scroll);
		if(attribute != null){
			GUILayout.BeginVertical ("box");
			attribute.OnGUI();
			GUILayout.EndVertical();
		}
		EditorGUILayout.EndScrollView();
	}
	
	private void DrawToolStrip ()
	{
		if (GUILayout.Button ("File", EditorStyles.toolbarDropDown, GUILayout.Width (60))) {
			GenericMenu toolsMenu = new GenericMenu ();
			toolsMenu.AddItem (new GUIContent ("Create"), false, Create,ScriptableObject.CreateInstance<PlayerAttribute>());
			toolsMenu.AddItem (new GUIContent ("Open"), false, Open);

			toolsMenu.DropDown (new Rect (0, 0, 0, 16));
			EditorGUIUtility.ExitGUI ();
		}
		GUILayout.FlexibleSpace ();
	}
	
	private void Open(){
		string mPath = EditorUtility.OpenFilePanel (
                "Overwrite current attribute",
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
		attribute=(PlayerAttribute)AssetDatabase.LoadAssetAtPath(mPath,typeof(PlayerAttribute));

	}
	
	private void Create(object data){
		string mPath = EditorUtility.SaveFilePanelInProject (
         	       "Create Attribute Asset",
            	    "New "+ data.GetType().ToString() + ".asset",
                	"asset", "");
		AssetDatabase.CreateAsset ((PlayerAttribute)data, mPath);
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = (PlayerAttribute)data;
		attribute=(PlayerAttribute)data;
		
	}
	
	private void OnDisable(){
		Resources.UnloadUnusedAssets();
	}
}
