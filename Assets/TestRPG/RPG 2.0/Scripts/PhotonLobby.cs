using UnityEngine;
using System.Collections;

public class PhotonLobby : Photon.MonoBehaviour
{
	private static PhotonLobby instance;

	public static PhotonLobby Instance {
		get{ return instance;}
	}
	
	public PhotonRoom roomPrefab;
	public UITable roomTable;
	public GameObject lobbyWindow;
	public GameObject connectionLabel;
	public UIInput roomName;
	public string level;
	private void Awake ()
	{
		instance = this;
		if (!PhotonNetwork.connected) {
			PhotonNetwork.ConnectUsingSettings ("2.0.3");
		}
		
		//PhotonNetwork.automaticallySyncScene=true;
	}
	
	private void Update ()
	{
		if (!PhotonNetwork.connected) {
			lobbyWindow.SetActive (false);
			connectionLabel.SetActive (true);
		} else {
			lobbyWindow.SetActive (true);
			connectionLabel.SetActive (false);
		}
	}
	
	public void AddRoom (string roomName, int playerCount)
	{
		GameObject room = NGUITools.AddChild (roomTable.gameObject, roomPrefab.gameObject);
		PhotonRoom photonRoom = room.GetComponent<PhotonRoom> ();
		photonRoom.roomName.text = roomName;
		photonRoom.players.text = "Players: " + playerCount.ToString ();
		roomTable.Reposition ();
	}
	
	void OnConnectedToPhoton()
    {
		//Debug.Log("Connected");
		StartCoroutine(RefreshRooms());
		
	}
	
	private bool refreshedRooms;
	
	private IEnumerator RefreshRooms(){
		while(!refreshedRooms){
			yield return new WaitForSeconds(0.2f);
			if(PhotonNetwork.GetRoomList().Length>0){
				RefreshRoomList();
				refreshedRooms=true;
			}
		}
	}
	
	public void OnReceivedRoomList()
    {
        Debug.Log("OnReceivedRoomList");
		RefreshRoomList();
    }
	
	private void RefreshRoomList ()
	{
		foreach (PhotonRoom room in roomTable.GetComponentsInChildren<PhotonRoom>()) {
			Destroy (room.gameObject);
		}
		roomTable.Reposition ();
		
		foreach (RoomInfo game in PhotonNetwork.GetRoomList()) {
			AddRoom(game.name,game.playerCount);
		}
		
		roomTable.Reposition();
	}
	
	private void CreateRoom ()
	{
		PhotonNetwork.CreateRoom (roomName.text, true, true, 10);
	}
	
	private void OnCreatedRoom()
    {
		 StartCoroutine(LoadScene(level));
	}
		
	private void OnJoinedRoom()
    {
       StartCoroutine(LoadScene(level));
    }
	
	private IEnumerator LoadScene(string scene){
		  while (PhotonNetwork.room == null)
        {
            yield return 0;
        }
		
        PhotonNetwork.isMessageQueueRunning = false;
        Application.LoadLevel(scene);
        
	}
    
}
