// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///  This singleton class manages the network and wraps useful network methods.
/// </summary>
public class NetworkManager : MonoBehaviour {

	#region Fields
	/// <summary>
	///  A static instance of this script to make this gameobject a singleton.
	/// </summary>
	public static NetworkManager Instance{get; private set;}

	/// <summary>
	///  The version of this build. Photon separates versions on the network.
	/// </summary>
	public string version;
	#endregion

	#region Properties
	/// <summary>
	/// Gets or sets the name of the local player.
	/// </summary>
	/// <value>The name of the local player.</value>
	public string PlayerName{
		get{return PhotonNetwork.playerName;}
		set{PhotonNetwork.playerName = value;}
	}

	/// <summary>
	/// Gets the current room.
	/// </summary>
	/// <value>The current room.</value>
	public Room Room{
		get{return PhotonNetwork.room;}
	}

	/// <summary>
	/// Gets a value indicating whether <see cref="PhotonNetwork"/> is connected.
	/// </summary>
	/// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
	public bool Connected{
		get{return PhotonNetwork.connected;}
	}

	/// <summary>
	/// Gets the room list.
	/// </summary>
	/// <value>The room list.</value>
	public RoomInfo[] RoomList{
		get{return PhotonNetwork.GetRoomList();}
	}

	/// <summary>
	/// Gets or sets a value indicating whether <see cref="PhotonNetwork"/> is in offline mode.
	/// </summary>
	/// <value><c>true</c> if in offline mode; otherwise, <c>false</c>.</value>
	public bool OfflineMode{
		get{return PhotonNetwork.offlineMode;}
		set{PhotonNetwork.offlineMode = value;}
	}

	/// <summary>
	/// Gets or sets a value indicating whether <see cref="PhotonNetwork"/> is or should be running the message queue.
	/// </summary>
	/// <value><c>true</c> if this instance is message queue running; otherwise, <c>false</c>.</value>
	public bool IsMessageQueueRunning{
		get{return PhotonNetwork.isMessageQueueRunning;}
		set{PhotonNetwork.isMessageQueueRunning = value;}
	}
	#endregion

	#region Unity Messages
	/// <summary>
	/// A message called when the script instance is being loaded.
	/// </summary>
	void Awake(){
		if(Instance==null){ //If there is no instance of this script, then set the instance to be this script and make the gameobject survive scene changes
			Instance = this;
			gameObject.AddComponent<PhotonView>();
			gameObject.GetPhotonView().viewID = 1;
			DontDestroyOnLoad(gameObject);
		}else{ //If there is already an instance of this script, then destroy this gameobject
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// A message called when the script is enabled just before Update is called.
	/// </summary>
	void Start(){
		if(!PhotonNetwork.connected){ //As soon as the game loads up, we want to connect if we aren't connected already
			Connect(version);
		}
	}

	/// <summary>
	/// A message called when the local player loads a level.
	/// </summary>
	/// <param name="level">
	/// The integer id of the level that was loaded.
	/// </param>
	void OnLevelWasLoaded(int level){
		IsMessageQueueRunning = true;
	}
	#endregion

	#region Methods
	/// <summary>
	/// A method to connect to the Photon server. Uses the settings you set in the PUN Setup Wizard.
	/// </summary>
	/// <param name="version">
	/// The version of this build. Photon uses this to separate different versions on the server.
	/// </param>
	public void Connect(string version){
		PhotonNetwork.ConnectUsingSettings(version);
	}

	/// <summary>
	/// A method to create a new room on the Photon server.
	/// </summary>
	/// <param name="roomName">
	/// The name of the room we're creating.
	/// </param>
	/// <param name="isVisible">
	/// Should this room be visible to others?
	/// </param>
	/// <param name="isOpen">
	/// Should this room be open to others?
	/// </param>
	/// <param name="maxPlayers">
	/// The maximum number of players that can be in this room at one time.
	/// </param>
	public void CreateRoom(string roomName,bool isVisible,bool isOpen,int maxPlayers){
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.isVisible = isVisible;
        roomOptions.isOpen = isOpen;
        roomOptions.maxPlayers = (byte)maxPlayers;
        TypedLobby typedLobby = new TypedLobby();
        PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby);
	}

	/// <summary>
	/// A method to join an existing room on the Photon server.
	/// </summary>
	/// <param name="roomName">
	/// The name of the room we're trying to join.
	/// </param>
	public void JoinRoom(string roomName){
		PhotonNetwork.JoinRoom(roomName);
	}

	/// <summary>
	/// A method to join a random room on the Photon server.
	/// </summary>
	public void JoinRandomRoom(){
		PhotonNetwork.JoinRandomRoom();
	}

	/// <summary>
	/// A method to leave the current room.
	/// </summary>
	public void LeaveRoom(){
		PhotonNetwork.LeaveRoom();
	}

	/// <summary>
	/// A method to instantiate a prefab on the network.
	/// </summary>
	/// <param name="prefabName">
	/// The name of the prefab we're instantiating. NOTE: The prefab must be located in the Resources folder.
	/// </param>
	/// <param name="position">
	/// The position where the prefab should be instantiated.
	/// </param>
	/// <param name="rotation">
	/// How the prefab should be orientated.
	/// </param>
	/// <param name="group">
	/// The group the prefab. Can be used for team-based play.
	/// </param>
	public GameObject Instantiate(string prefabName,Vector3 position,Quaternion rotation,int group){
		return PhotonNetwork.Instantiate(prefabName,position,rotation,group);
	}

	/// <summary>
	/// A method to load a level.
	/// </summary>
	/// <param name="levelID">
	/// The integer id of the level to load.
	/// </param>
	public void LoadLevel(int levelID){
		IsMessageQueueRunning = false;
		PhotonNetwork.LoadLevel(levelID);
	}

	/// <summary>
	/// A method to load a level.
	/// </summary>
	/// <param name="levelName">
	/// The name of the level to load.
	/// </param>
	public void LoadLevel(string levelName){
		IsMessageQueueRunning = false;
		PhotonNetwork.LoadLevel(levelName);
	}
	#endregion
    
	#region Photon Messages
	/// <summary>
	/// A message called when the local player fails to join a random room.
	/// </summary>
	void OnPhotonRandomJoinFailed(){
		CreateRoom(null,true,true,10);
	}
	#endregion
  
}
