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

/// <summary>
/// This class displays the main menu used in the lobby.
/// </summary>
public class MainMenu : MonoBehaviour{
	
	#region Structs, Enums, and Classes
	/// <summary>
	/// This class constructs an Avatar object.
	/// </summary>
	[System.Serializable]
	public class Avatar{
		/// <summary>
		/// The name of the avatar prefab.
		/// </summary>
		public string name;

		/// <summary>
		/// The prefab for this avatar.
		/// </summary>
		public GameObject prefab;

		/// <summary>
		/// The icon for this avatar.
		/// </summary>
		public Texture icon;
	}
	#endregion

	#region Fields
	/// <summary>
	/// An array of Avatar.
	/// </summary>
	public Avatar[] avatars;

	/// <summary>
	/// The selected avatar.
	/// </summary>
	[HideInInspector]
	public string selectedAvatar = "";

	/// <summary>
	/// The spawn radius.
	/// </summary>
	public float spawnRadius=3f;

	/// <summary>
	/// The custom skin.
	/// </summary>
	public GUISkin CustomSkin;

	/// <summary>
	/// The background for the main menu.
	/// </summary>
	public Texture background;

	//GUI styles used to style the buttons of unselected and selected avatars
	GUIStyle unselected,selected;

	//The room name
	string roomName;

	//The starting scroll position for the room list
    Vector2 scrollPos = Vector2.zero;
	#endregion

	#region Unity Messages
	/// <summary>
	/// A message called once just before Update is called.
	/// </summary>
    void Start(){
        //Load values from PlayerPrefs
		selectedAvatar = PlayerPrefs.GetString("avatar",avatars[0].name);
        NetworkManager.Instance.PlayerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));
		roomName = PlayerPrefs.GetString("roomName", "New Room");

		//Get the appropriate styles from the custom skin
		unselected = CustomSkin.customStyles[0];
		selected = CustomSkin.customStyles[1];
	}

	/// <summary>
	/// A message called for rendering and handling GUI events.
	/// </summary>
    void OnGUI(){

		if (NetworkManager.Instance.Room != null){ //If the local player is in a room, do not show the main menu
			return;
		}

		GUI.skin = CustomSkin; //Set the GUI skin to be the custom skin
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background); //Give the main menu a background

		if (!NetworkManager.Instance.Connected && !NetworkManager.Instance.OfflineMode){ //If the player is not connected and offline mode is not on, do not show the rest of the main menu
            return;
        }

		//Avatar Selection
        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		for(int i=0;i<avatars.Length;i++){ //For each of the avatars
			if(avatars[i].name==selectedAvatar){ //If it is the selected avatar
				if(GUILayout.Button(avatars[i].icon,selected,GUILayout.Width(74),GUILayout.Height(74))){ //If the player hits this button
					selectedAvatar = avatars[i].name; //Make this the selected avatar
				}
			}else{ //If it is the unselected avatar
				if(GUILayout.Button(avatars[i].icon,unselected,GUILayout.Width(74),GUILayout.Height(74))){ //If the player hits this button
					selectedAvatar = avatars[i].name; //Make this the selected avatar
				}
			}
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

		//Avatar Name
        GUILayout.Label("Name:", GUILayout.Width(150));
		NetworkManager.Instance.PlayerName = GUILayout.TextField(NetworkManager.Instance.PlayerName); //Set the player's name to whatever the player types in the text field
        GUILayout.EndHorizontal();

		//Room List
        GUILayout.Space(30);
        GUILayout.Label("Rooms:");
		if (NetworkManager.Instance.RoomList.Length == 0){ //If there are no active rooms
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            GUILayout.Label("No active rooms"); //Display a label saying so
            GUILayout.EndScrollView();
        }else{ //If there are active rooms
            scrollPos = GUILayout.BeginScrollView(scrollPos);
			foreach (RoomInfo room in NetworkManager.Instance.RoomList){ //For each room
                GUILayout.BeginHorizontal();
                GUILayout.Label(room.name + " " + room.playerCount + "/" + room.maxPlayers); //Display a label indicating the room name, player count, and max players
                if (GUILayout.Button("Join")){ //If the player hits this button
					NetworkManager.Instance.JoinRoom(room.name); //Join this room
					SaveSettings(); //Save the settings
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

		//Room Creation
		GUILayout.Space(15);
        GUILayout.BeginHorizontal();
        roomName = GUILayout.TextField(roomName,GUILayout.Width(300f)); //Set the room name to whatever the player types in the text field
		if (GUILayout.Button("Create")){ //If the player hits this button
			NetworkManager.Instance.CreateRoom(roomName, true, true, 10); //Create this room with the specified room name
			SaveSettings(); //Save the settings
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

	/// <summary>
	/// A message called for drawing and rendering elements in the scene view.
	/// </summary>
	void OnDrawGizmos(){
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position,spawnRadius); //Draw a green wire sphere showing the spawn radius
	}
	#endregion

	#region Photon Messages
	/// <summary>
	/// A message called when the local player joins a room.
	/// </summary>
	void OnJoinedRoom(){    
		camera.enabled = false; //Turn off the camera
		GetComponent<AudioListener>().enabled = false; //Turn off the Audio Listener
		Vector3 spawnPosition = new Vector3(Random.Range(transform.position.x-spawnRadius,transform.position.x+spawnRadius),transform.position.y,Random.Range(transform.position.z-spawnRadius,transform.position.z+spawnRadius)); //Generate a random spawn position within the spawn radius
		SpawnPlayer(selectedAvatar, spawnPosition, gameObject.transform.rotation, 0); //Spawn the selected avatar at the spawn position
	}

	/// <summary>
	/// A message called when the local player leaves a room.
	/// </summary>
	void OnLeftRoom(){
		camera.enabled = true; //Turn on the camera
		GetComponent<AudioListener>().enabled = true; //Turn on the Audio Listener
	}
	#endregion

	#region Methods
	/// <summary>
	/// Spawns the player.
	/// </summary>
	/// <param name="avatarName">The avatar name.</param>
	/// <param name="spawnPosition">The spawn position.</param>
	/// <param name="spawnRotation">The spawn rotation.</param>
	/// <param name="group">The group.</param>
	void SpawnPlayer(string avatarName,Vector3 spawnPosition,Quaternion spawnRotation,int group){
		GameObject player = NetworkManager.Instance.Instantiate(avatarName, spawnPosition, spawnRotation, group);
		GameObject cameraGO = player.transform.FindChild("Camera").gameObject;
		cameraGO.SetActive(true);
		cameraGO.tag = "MainCamera";
		player.GetComponent<PlayerController>().enabled = true;
		player.GetComponent<PlayerHUD>().enabled = true;
		player.tag = "LocalPlayer";
		WebWarpLocalPlayer.Instance.SetLocalPlayer(player);
		Transform icon = player.transform.FindChild("Icon");
		icon.renderer.material.color = Color.green;
		icon.transform.position += Vector3.up * .05f;
	}

	/// <summary>
	/// Saves the settings.
	/// </summary>
	void SaveSettings(){
		PlayerPrefs.SetString("playerName", NetworkManager.Instance.PlayerName);
		PlayerPrefs.SetString("avatar",selectedAvatar);
		PlayerPrefs.SetString("roomName",roomName);
	}
	#endregion

}