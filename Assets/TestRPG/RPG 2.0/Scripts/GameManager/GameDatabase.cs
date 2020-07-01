using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum DatabaseType
{
	Binary,
	MySql
}

public delegate void PlayerLoadCallback(byte[] data);
public delegate void SceneLoadCallBack(string scene);

[System.Serializable]
public class GameDatabase : ScriptableObject
{
	public DatabaseType databaseType = DatabaseType.Binary;
	public float saveInterval = 120;
	public string playerDataFile = "playerData";
	public string sceneDataFile = "playerScene";
	public string serverAddress = "";
	private byte[] playerData;
	private string accountName;
	
	public void SaveGame ()
	{
		MessageManager.Instance.AddMessage(GameManager.GameMessages.startSaving);
		BinaryFormatter formatter = new BinaryFormatter ();
		formatter.Binder = new VersionDeserializationBinder ();
		
		switch (databaseType) {
		case DatabaseType.Binary:
			FileStream fileStream = new FileStream (Application.dataPath + "/" + playerDataFile + ".bytes", FileMode.Create); 
			formatter.Serialize (fileStream, GameManager.Player);
		
			fileStream.Close ();
		
			SaveScene ();
			MessageManager.Instance.AddMessage(GameManager.GameMessages.endSaving);
			break;
		case DatabaseType.MySql:
			MemoryStream stream = new MemoryStream ();
			formatter.Serialize (stream, GameManager.Player);
			byte[] data = stream.ToArray ();
			UnityTools.StartCoroutine (SaveToMySql (data, Application.loadedLevelName,accountName));
			stream.Close ();
			break;
		}
	}
	
	public void SaveScene ()
	{
		FileStream fileStream = new FileStream (Application.dataPath + "/" + sceneDataFile + ".bytes", FileMode.Create); 
		BinaryFormatter formatter = new BinaryFormatter ();
		formatter.Binder = new VersionDeserializationBinder ();
		
		formatter.Serialize (fileStream, Application.loadedLevelName);
		
		fileStream.Close ();
	}
	
	
	
	public void LoadScene (SceneLoadCallBack callback)
	{
		switch(databaseType){
		case DatabaseType.Binary:
		if (!File.Exists (Application.dataPath + "/" + sceneDataFile + ".bytes")) {
			Debug.Log ("File does not exist: " + Application.dataPath + "/" + sceneDataFile + ".bytes");
			callback(string.Empty);
			return;
		}
	
		FileStream fileStream = new FileStream (Application.dataPath + "/" + sceneDataFile + ".bytes", FileMode.Open); 
		byte[] bytes = new byte[fileStream.Length];
		fileStream.Read (bytes, 0, (int)fileStream.Length);
		
		MemoryStream stream = new MemoryStream (bytes);
		BinaryFormatter formatter = new BinaryFormatter ();
		formatter.Binder = new VersionDeserializationBinder ();
		
		string scene = (string)formatter.Deserialize (stream);

		fileStream.Close ();
		stream.Close ();
		callback(scene);
		return;
			
		case DatabaseType.MySql:
			UnityTools.StartCoroutine(LoadScene(accountName,callback));
			return;
		}
		
		callback(string.Empty);
	}
	
	public bool LoadGame ()
	{
		switch (databaseType) {
		case DatabaseType.Binary:
			if (!File.Exists (Application.dataPath + "/" + playerDataFile + ".bytes")) {
				Debug.Log ("File does not exist: " + Application.dataPath + "/" + playerDataFile + ".bytes");
				return false;
			}
		
			if (DataStorage.Instance != null) {
				return false;
			}
		
			FileStream fileStream = new FileStream (Application.dataPath + "/" + playerDataFile + ".bytes", FileMode.Open); 
			byte[] bytes = new byte[fileStream.Length];
			fileStream.Read (bytes, 0, (int)fileStream.Length);
		
			MemoryStream stream = new MemoryStream (bytes);
			BinaryFormatter formatter = new BinaryFormatter ();
			formatter.Binder = new VersionDeserializationBinder ();
			try{
				GameManager.Player = (Player)formatter.Deserialize (stream);
				GameObject player = PhotonNetwork.Instantiate (GameManager.Player.Character.prefab.name, UnityTools.RandomPointInArea (GameManager.Player.Checkpoint, 1) + Vector3.up, UnityTools.RandomQuaternion (Vector3.up, 0, 360), 0);
				GameManager.Player.Initialize (player.transform);
			}catch{
				fileStream.Close ();
				stream.Close ();
				return false;
			}
			fileStream.Close ();
			stream.Close ();
			return true;
		case DatabaseType.MySql:
			MemoryStream str = new MemoryStream (playerData);
			Debug.Log(playerData.Length);
			BinaryFormatter f = new BinaryFormatter ();
			f.Binder = new VersionDeserializationBinder ();
			try{
				GameManager.Player = (Player)f.Deserialize (str);
				GameObject pl = PhotonNetwork.Instantiate (GameManager.Player.Character.prefab.name, UnityTools.RandomPointInArea (GameManager.Player.Checkpoint, 1) + Vector3.up, UnityTools.RandomQuaternion (Vector3.up, 0, 360), 0);
				GameManager.Player.Initialize (pl.transform);
			}catch{
				str.Close ();
				return false;
			}
			str.Close ();
			return true;
		}
		return false;
	}
	
	private IEnumerator SaveToMySql (byte[] data, string scene,string username)
	{
		WWWForm newForm = new WWWForm ();

		newForm.AddBinaryData ("player", data, "playerFile");
		newForm.AddField ("scene", scene);
		newForm.AddField("name",username);
		
		WWW w = new WWW (serverAddress + "/SavePlayer.php", newForm);

		while (!w.isDone) {
			yield return new WaitForEndOfFrame();
		}

		if (w.error != null) {
			Debug.LogError (w.error);
		}

		string res = w.text.Trim ();
		if (res.Equals ("success")) {  
			MessageManager.Instance.AddMessage(GameManager.GameMessages.endSaving);
			Debug.Log ("Successfuly saved.");
		}else{
			Debug.Log(res);
		}
	}
	
	public IEnumerator LoadPlayer(string username,PlayerLoadCallback callback){
		WWWForm newForm = new WWWForm ();
		newForm.AddField ("name", username);
		
		WWW w = new WWW (serverAddress + "/LoadPlayer.php", newForm);

		while (!w.isDone) {
			yield return new WaitForEndOfFrame();
		}

		if (w.error != null) {
			Debug.LogError (w.error);
		}
		
		playerData=w.bytes;
		callback(w.bytes);
		
	}
	
	public IEnumerator LoadScene(string username, SceneLoadCallBack callback){
		WWWForm newForm = new WWWForm ();
		newForm.AddField ("name", username);
		
		WWW w = new WWW (serverAddress + "/LoadScene.php", newForm);

		while (!w.isDone) {
			yield return new WaitForEndOfFrame();
		}

		if (w.error != null) {
			Debug.LogError (w.error);
		}
		callback(w.text.TrimEnd().TrimStart());
	}
	
	public IEnumerator RegisterProfile (string username, string password, string email, GameObject target)
	{

		WWWForm newForm = new WWWForm ();
		newForm.AddField ("name", username);
		newForm.AddField ("password", password);
		newForm.AddField ("email", email);
		
		WWW w = new WWW (serverAddress + "/RegisterAccount.php", newForm);

		while (!w.isDone) {
			yield return new WaitForEndOfFrame();
		}

		if (w.error != null) {
			Debug.LogError (w.error);
		}
		
		string res = w.text.Trim ();
		Debug.Log(res);
		if (res.Equals ("success")) {  
			target.SendMessage ("OnSuccess", SendMessageOptions.DontRequireReceiver);
		} else {
			target.SendMessage ("OnFail", SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public IEnumerator Login (string username, string password, GameObject target)
	{

		WWWForm newForm = new WWWForm ();
		newForm.AddField ("name", username);
		newForm.AddField ("password", password);
		
		WWW w = new WWW (serverAddress + "/Login.php", newForm);

		while (!w.isDone) {
			yield return new WaitForEndOfFrame();
		}

		if (w.error != null) {
			Debug.LogError (w.error);
		}
		
		string res = w.text.Trim ();
		if (res.Equals ("success")) {  
			accountName=username;
			target.SendMessage ("OnSuccess", SendMessageOptions.DontRequireReceiver);
		} else {
			target.SendMessage ("OnFail", SendMessageOptions.DontRequireReceiver);
		}
	}
	
	#if UNITY_EDITOR
	public void OnGUI(){
		GUI.changed= false;
		
		GUILayout.BeginVertical("Game Database","box");
		GUILayoutUtility.GetRect(0,15);
		databaseType=(DatabaseType)EditorGUILayout.EnumPopup("Database",databaseType);
		saveInterval=EditorGUILayout.FloatField("Save Interval",saveInterval);
		
		switch(databaseType){
		case DatabaseType.Binary:
			playerDataFile=EditorGUILayout.TextField("Save File",playerDataFile);
			sceneDataFile=EditorGUILayout.TextField("Scene File",sceneDataFile);
			break;
		case DatabaseType.MySql:
			serverAddress=EditorGUILayout.TextField("Server Address",serverAddress);
			serverAddress=serverAddress.Replace(" ","%20");
			break;
		}
		GUILayout.EndVertical();
		
		if(GUI.changed){
			EditorUtility.SetDirty(this);
		}
	}
	#endif
}
