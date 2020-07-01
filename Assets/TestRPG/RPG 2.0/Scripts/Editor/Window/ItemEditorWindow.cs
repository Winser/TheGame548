using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class ItemEditorWindow : EditorWindow
{
	[MenuItem ("RPG Kit 2.0/Editor/Item")]
	public static void Init ()
	{
		editor = (ItemEditorWindow)EditorWindow.GetWindow (typeof(ItemEditorWindow));
	}
	
	private ItemTable itemTable;
	private BaseItem item;
	private Vector2 scroll;
	private int menu = 0;
	private static ItemEditorWindow editor;

	private void OnGUI ()
	{
		if (editor == null) {
			editor = (ItemEditorWindow)EditorWindow.GetWindow (typeof(ItemEditorWindow));
		}
		DrawMenu ();
		
		scroll = EditorGUILayout.BeginScrollView (scroll);
		GUI.changed = false;
		switch (menu) {
		case 0://Item
			if (item) {
				item.OnGUI ();
			}
			if (GUI.changed && item) {
				EditorUtility.SetDirty (item);
			}
			break;
		case 1://ItemTable
			if (itemTable) {
				itemTable.OnGUI ();
			}
			
			if (GUI.changed && itemTable) {
				EditorUtility.SetDirty (itemTable);
			}
			break;
		}
		EditorGUILayout.EndScrollView ();
	}
	
	private void DrawMenu ()
	{
		GUILayout.BeginHorizontal (EditorStyles.toolbar);
		if (GUILayout.Button ("File", EditorStyles.toolbarDropDown, GUILayout.Width (60))) {
			GenericMenu toolsMenu = new GenericMenu ();
			toolsMenu.AddItem (new GUIContent ("Open/Item"), false, OpenItem);
			toolsMenu.AddItem (new GUIContent ("Open/Table"), false, OpenItemTable);
			toolsMenu.AddItem (new GUIContent ("Create/Table"), false, Create, ScriptableObject.CreateInstance<ItemTable> ());
			toolsMenu.AddItem (new GUIContent ("Create/Base Item"), false, Create, ScriptableObject.CreateInstance<BaseItem> ());
			toolsMenu.AddItem (new GUIContent ("Create/Sellable Item"), false, Create, ScriptableObject.CreateInstance<SellableItem> ());
			toolsMenu.AddItem (new GUIContent ("Create/Equipment"), false, Create, ScriptableObject.CreateInstance<EquipmentItem> ());
			toolsMenu.AddItem (new GUIContent ("Create/Consumable/Potion"), false, Create, ScriptableObject.CreateInstance<Potion> ());
			toolsMenu.AddItem (new GUIContent ("Create/Consumable/Talent Scroll"), false, Create, ScriptableObject.CreateInstance<TalentScroll> ());
			toolsMenu.AddItem (new GUIContent ("Create/Collectable"), false, Create, ScriptableObject.CreateInstance<CollectableItem> ());
			
			toolsMenu.DropDown (new Rect (0, 0, 0, 16));
			EditorGUIUtility.ExitGUI ();
		}
		
		GUI.enabled=item != null && item.prefab != null;
		if (menu == 0) {
			if (GUILayout.Button ("Add Components", EditorStyles.toolbarButton, GUILayout.Width (120))) {
				if (item) {
					AddComponents (item.prefab);
				}
			}
		}
		GUI.enabled=true;
		
		if (itemTable && menu == 1) {
			if (GUILayout.Button ("Add Item", EditorStyles.toolbarButton, GUILayout.Width (80))) {
				itemTable.items.Add (null);
			}
		}
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
	}
	
	private void OpenItem ()
	{
		string mPath = EditorUtility.OpenFilePanel (
                "Open Item",
                "",
                "asset");
		string[] splitPath = mPath.Split ('/');
		mPath = string.Empty;
		foreach (string s in splitPath) {
			if (s.Equals ("Assets") || !mPath.Equals (string.Empty)) {
				mPath += s + "/";
			}
		}
		mPath = mPath.Remove (mPath.Length - 1);
		item = (BaseItem)AssetDatabase.LoadAssetAtPath (mPath, typeof(BaseItem));

	}
	
	private void OpenItemTable ()
	{
		string mPath = EditorUtility.OpenFilePanel (
                "Open ItemTable",
                "",
                "asset");
		string[] splitPath = mPath.Split ('/');
		mPath = string.Empty;
		foreach (string s in splitPath) {
			if (s.Equals ("Assets") || !mPath.Equals (string.Empty)) {
				mPath += s + "/";
			}
		}
		mPath = mPath.Remove (mPath.Length - 1);
		itemTable = (ItemTable)AssetDatabase.LoadAssetAtPath (mPath, typeof(ItemTable));

	}
	
	private void Create (object data)
	{
		string mPath = EditorUtility.SaveFilePanelInProject (
         	       "Create Asset",
            	   "New " + data.GetType ().ToString () + ".asset",
                   "asset", "");
		if (data is BaseItem) {
			AssetDatabase.CreateAsset ((BaseItem)data, mPath);
			AssetDatabase.SaveAssets ();
			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = (BaseItem)data;
			item = (BaseItem)data;
			menu = 0;
			GameManager.ItemDatabase.AddItem(item);
			EditorUtility.SetDirty(GameManager.ItemDatabase);
		}
		
		if (data is ItemTable) {
			AssetDatabase.CreateAsset ((ItemTable)data, mPath);
			AssetDatabase.SaveAssets ();
			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = (ItemTable)data;
			itemTable = (ItemTable)data;
			menu = 1;
		}
		GUIUtility.keyboardControl = 0;
	}
	
	private GameObject CreatePrefab (GameObject obj)
	{
		string mPath = EditorUtility.SaveFilePanelInProject (
         	       "Save Item Prefab",
            	    item.itemName + ".prefab",
                	"prefab", "");
		
		if (mPath.Equals (string.Empty)) {
			return null;
		}
		GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath (mPath, typeof(GameObject));
		if (prefab != obj) {
			prefab = (GameObject)PrefabUtility.CreatePrefab (mPath, obj, ReplacePrefabOptions.ConnectToPrefab);
		}
		return prefab;
	}
	
	private void AddComponents (GameObject prefab)
	{
		//Retruns false if the prefab is in scene
		bool persistent = EditorUtility.IsPersistent (prefab);
		if (persistent) {
			prefab = (GameObject)Instantiate (prefab);
		}
		
		Rigidbody rigidbody=prefab.AddComponent<Rigidbody>();
		rigidbody.freezeRotation=true;
		
		MeshCollider collider=prefab.AddComponent<MeshCollider>();
		collider.convex=true;
		
		Lootable lootable=prefab.AddComponent<Lootable>();
		lootable.item=item;
		
		prefab.AddComponent<PhotonView>();
		
		GameObject projectPrefab = CreatePrefab (prefab);
		item.prefab = projectPrefab;
		
		Selection.activeTransform = prefab.transform;
		SceneView.lastActiveSceneView.FrameSelected ();
	}
	
	private void OnDisable(){
		Resources.UnloadUnusedAssets();
	}
}

