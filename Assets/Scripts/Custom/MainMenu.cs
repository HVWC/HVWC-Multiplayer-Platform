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
	
	public GUISkin skin;
	public Texture background;
	
	GameManager gm;
	private GUIStyle unselected,selected;
	private string roomName;
    private Vector2 scrollPos = Vector2.zero;

    void Awake(){
		//PlayerPrefs.DeleteAll();
        //PhotonNetwork.logLevel = NetworkLogLevel.Full;

        //Connect to the main photon server. This is the only IP and port we ever need to set(!)
        if (!PhotonNetwork.connected){
			PhotonNetwork.ConnectUsingSettings("v1.0"); // version of the game/demo. used to separate older clients from newer ones (e.g. if incompatible)
		}
		gm = GetComponent<GameManager>();
		
        //Load values from PlayerPrefs
		gm.selectedAvatar = PlayerPrefs.GetString("avatar",gm.avatars[0].name);
        PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));
		roomName = PlayerPrefs.GetString("roomName", "New Room");

        //Set camera clipping for nicer "main menu" background
        Camera.main.farClipPlane = Camera.main.nearClipPlane + 0.1f;
	
		unselected = gm.photonSkin.customStyles[0];
		selected = gm.photonSkin.customStyles[1];
	}
	
    void OnGUI(){
		
        if (!PhotonNetwork.connected && !PhotonNetwork.offlineMode)
        {
            ShowConnectingGUI();
            return;   //Wait for a connection
        }


        if (PhotonNetwork.room != null)
            return; //Only when we're not in a Room
		
		GUI.skin = skin;
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background);

        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		for(int i=0;i<gm.avatars.Length;i++){
			if(gm.avatars[i].name==gm.selectedAvatar){
				if(GUILayout.Button(gm.avatars[i].icon,selected,GUILayout.Width(74),GUILayout.Height(74))){
					gm.selectedAvatar = gm.avatars[i].name;
				}
			}else{
				if(GUILayout.Button(gm.avatars[i].icon,unselected,GUILayout.Width(74),GUILayout.Height(74))){
					gm.selectedAvatar = gm.avatars[i].name;
				}
			}
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndHorizontal();
        //Player name
        GUILayout.BeginHorizontal();
		
        GUILayout.Label("Name:", GUILayout.Width(150));
        PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
        GUILayout.EndHorizontal();

        GUILayout.Space(30);
        GUILayout.Label("Games:");
        if (PhotonNetwork.GetRoomList().Length == 0)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            GUILayout.Label("No active games");
            GUILayout.EndScrollView();
        }
        else
        {
            //Room listing: simply call GetRoomList: no need to fetch/poll whatever!
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            foreach (RoomInfo game in PhotonNetwork.GetRoomList())
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(game.name + " " + game.playerCount + "/" + game.maxPlayers);
                if (GUILayout.Button("Join"))
                {
                    PhotonNetwork.JoinRoom(game.name);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
		
		GUILayout.Space(15);

        //Create a room (fails if exist!)
        GUILayout.BeginHorizontal();
        roomName = GUILayout.TextField(roomName,GUILayout.Width(300f));
        if (GUILayout.Button("Create"))
        {
            if(!PhotonNetwork.offlineMode){
				PhotonNetwork.CreateRoom(roomName, true, true, 10);
			}else{
				PhotonNetwork.CreateRoom(null);
			}
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
		if (GUI.changed){//Save name
            PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
			PlayerPrefs.SetString("avatar",gm.selectedAvatar);
			PlayerPrefs.SetString("roomName",roomName);
		}
    }


    void ShowConnectingGUI(){
        GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));

        GUILayout.Label("Connecting to Photon server.");
        GUILayout.Label("Hint: This demo uses a settings file and logs the server address to the console.");

        GUILayout.EndArea();
    }
}
