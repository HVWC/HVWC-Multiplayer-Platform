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

public class MainMenu : MonoBehaviour{
	
	[System.Serializable]
	public class Avatar{
		public string name;
		public GameObject prefab;
		public Texture icon;		
	}
	public Avatar[] avatars;
	[HideInInspector]
	public string selectedAvatar = "";
	
	public float spawnRadius=3f;

	public GUISkin PhotonSkin;
	public Texture background;

	private GUIStyle unselected,selected;
	private string roomName;
    private Vector2 scrollPos = Vector2.zero;

    void Start(){
        //Load values from PlayerPrefs
		selectedAvatar = PlayerPrefs.GetString("avatar",avatars[0].name);
        NetworkManager.Instance.PlayerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));
		roomName = PlayerPrefs.GetString("roomName", "New Room");

        //Set camera clipping for nicer "main menu" background
        Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;
	
		unselected = PhotonSkin.customStyles[0];
		selected = PhotonSkin.customStyles[1];
	}
	
    void OnGUI(){

		if (NetworkManager.Instance.Room != null){ //Only when we're not in a Room
			return;
		}

		GUI.skin = PhotonSkin;
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background);

		if (!NetworkManager.Instance.Connected && !NetworkManager.Instance.OfflineMode){ //Wait for a connection
            return;
        }

        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		for(int i=0;i<avatars.Length;i++){
			if(avatars[i].name==selectedAvatar){
				if(GUILayout.Button(avatars[i].icon,selected,GUILayout.Width(74),GUILayout.Height(74))){
					selectedAvatar = avatars[i].name;
				}
			}else{
				if(GUILayout.Button(avatars[i].icon,unselected,GUILayout.Width(74),GUILayout.Height(74))){
					selectedAvatar = avatars[i].name;
				}
			}
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndHorizontal();
        //Player name
        GUILayout.BeginHorizontal();
		
        GUILayout.Label("Name:", GUILayout.Width(150));
		NetworkManager.Instance.PlayerName = GUILayout.TextField(NetworkManager.Instance.PlayerName);
        GUILayout.EndHorizontal();

        GUILayout.Space(30);
        GUILayout.Label("Games:");
		if (NetworkManager.Instance.RoomList.Length == 0){
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            GUILayout.Label("No active games");
            GUILayout.EndScrollView();
        }else{
            scrollPos = GUILayout.BeginScrollView(scrollPos);
			foreach (RoomInfo room in NetworkManager.Instance.RoomList){
                GUILayout.BeginHorizontal();
                GUILayout.Label(room.name + " " + room.playerCount + "/" + room.maxPlayers);
                if (GUILayout.Button("Join")){
					NetworkManager.Instance.JoinRoom(room.name);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
		
		GUILayout.Space(15);

        //Create a room (fails if exist!)
        GUILayout.BeginHorizontal();
        roomName = GUILayout.TextField(roomName,GUILayout.Width(300f));
        if (GUILayout.Button("Create")){
			NetworkManager.Instance.CreateRoom(roomName, true, true, 10);
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
		if (GUI.changed){//Save name
			PlayerPrefs.SetString("playerName", NetworkManager.Instance.PlayerName);
			PlayerPrefs.SetString("avatar",selectedAvatar);
			PlayerPrefs.SetString("roomName",roomName);
		}
    }

	void OnJoinedRoom(){
		Camera.main.farClipPlane = 1000;    
		Vector3 spawnPosition = new Vector3(Random.Range(transform.position.x-spawnRadius,transform.position.x+spawnRadius),transform.position.y,Random.Range(transform.position.z-spawnRadius,transform.position.z+spawnRadius));
		SpawnPlayer(selectedAvatar, spawnPosition, gameObject.transform.rotation, 0);
	}

	public void SpawnPlayer(string avatarName,Vector3 spawnPosition,Quaternion spawnRotation,int group){	
		GameObject player = NetworkManager.Instance.Instantiate(avatarName, spawnPosition, spawnRotation, group);
		player.GetComponent<PlayerCamera>().enabled = true;
		player.GetComponent<PlayerController>().enabled = true;
		player.GetComponent<PlayerHUD>().enabled = true;
		player.tag = "LocalPlayer";
		WebWarpLocalPlayer.SetLocalPlayer(player);
		player.transform.FindChild("Icon").renderer.material.color = Color.green;
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position,spawnRadius);
	}

}
