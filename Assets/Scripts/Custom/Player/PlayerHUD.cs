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

public class PlayerHUD : Photon.MonoBehaviour {

	private NetworkManager networkManager;
	private Chat chat;
	private PlayerList playerList;
	private PhotonPlayer selectedPlayer;
	
	public GUISkin PhotonSkin;
	private GUIStyle unselected,selected;
	private Rect topHudRect,bottomHudRect;
	private float topHudHeight,bottomHudHeight,buttonWH;
	private GameObject map;
	private Camera mapCamera;
	private WebWarpLocalPlayer warp;
	private bool showHelp=false,showPlayers=false,showChat=false,showMap=false,showVolume=false,showHUD=true;
	private float volume = 100;
	private string chatInput = "";
	
	public Texture2D C3_Tex,MapTex,VolumeTex,HelpTex,ExitTex,EmptyProgressBar,FullProgressBar;
 
	private AsyncOperation async = null; // When assigned, load is in progress.
	
	// UsplayerCamera for initialization
	void Start () {
		networkManager = FindObjectOfType(typeof(NetworkManager)) as NetworkManager;
		//enabled = photonView.isMine;
		chat = FindObjectOfType(typeof(Chat)) as Chat;
		playerList = FindObjectOfType(typeof(PlayerList)) as PlayerList;
		unselected = PhotonSkin.customStyles[0];
		selected = PhotonSkin.customStyles[1];
		map = GameObject.Find("Map");
		mapCamera = map.camera;
		warp = map.GetComponent<WebWarpLocalPlayer>();
		if(photonView.isMine){
			WebWarpLocalPlayer.SetLocalPlayer(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!chat){
			chat = FindObjectOfType(typeof(Chat)) as Chat;	
		}
		if(!playerList){
			playerList = FindObjectOfType(typeof(PlayerList)) as PlayerList;
		}
		
		//HUD Rects
		topHudHeight = Screen.width*.1f;
		bottomHudHeight = Mathf.Clamp(Screen.width*.03f,Screen.height*.02f,Screen.height*.05f);
		topHudRect = new Rect(Screen.width*.005f,0,Screen.width*.99f,topHudHeight);
		bottomHudRect = new Rect(Screen.width*.005f,Screen.height-bottomHudHeight,Screen.width*.99f,bottomHudHeight);
		
		buttonWH = Screen.width*.022f;
		AudioListener.volume = volume * .01f;
		
		if(Input.GetKeyDown(KeyCode.Escape)){
			showHUD = !showHUD;	
		}
		
	}

	void OnGUI(){
	
		if(!photonView.isMine || networkManager.Room==null || !showHUD){
			return;	
		}

		GUI.skin = PhotonSkin;
		PhotonSkin.button.fontSize = Mathf.RoundToInt(Screen.width*.013f);
		PhotonSkin.window.fontSize = Mathf.RoundToInt(Screen.width*.013f);
		PhotonSkin.label.fontSize = Mathf.RoundToInt(Screen.width*.01f);
		
		//Top HUD
		GUILayout.BeginArea(topHudRect);
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Load LVL1")){networkManager.LoadLevel("Demo");}
		if(GUILayout.Button("Load LVL2")){networkManager.LoadLevel("Demo 2");}
		GUILayout.FlexibleSpace();
		if(GUILayout.Button(C3_Tex,"miniButton",GUILayout.Width(buttonWH),GUILayout.Height(buttonWH))){C3JoinVoice.JoinVoice(networkManager.Room.name,networkManager.PlayerName);}
		if(GUILayout.Button(MapTex,"miniButton",GUILayout.Width(buttonWH),GUILayout.Height(buttonWH))){showMap=!showMap;mapCamera.enabled = warp.enabled = showMap;}
		GUILayout.BeginVertical();
		if(GUILayout.Button(VolumeTex,"miniButton",GUILayout.Width(buttonWH),GUILayout.Height(buttonWH))){showVolume=!showVolume;}
		if(showVolume){volume = GUILayout.VerticalSlider(volume,100,0,GUILayout.Width(buttonWH),GUILayout.Height((topHudHeight-buttonWH)*.9f));}
		GUILayout.EndVertical();
		if(showHelp){GUILayout.Window(2,new Rect((Screen.width*.5f)-(Screen.width*.2f),(Screen.height*.5f)-(Screen.height*.2f),Screen.width*.4f,Screen.height*.4f),HelpWindow,"H E L P");}
		if(GUILayout.Button(HelpTex,"miniButton",GUILayout.Width(buttonWH),GUILayout.Height(buttonWH))){showHelp=!showHelp;}
		if(GUILayout.Button(ExitTex,"miniButton",GUILayout.Width(buttonWH),GUILayout.Height(buttonWH))){networkManager.LeaveRoom();}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		
		//Bottom HUD
		GUILayout.BeginArea(bottomHudRect);
		GUILayout.BeginHorizontal();
		if(showPlayers){GUILayout.Window(0,new Rect(0,Screen.height-Screen.width*.28f,Screen.width*.2f,Screen.width*.25f),PlayerListWindow,"P L A Y E R S");}
		if(GUILayout.Button("P L A Y E R S")){showPlayers=!showPlayers;}
		GUILayout.FlexibleSpace();
		if(showChat){
			chatInput = GUILayout.TextField(chatInput,GUILayout.Width(Screen.width*.7f));
			if(GUILayout.Button("S E N D")){
				if(selectedPlayer!=null){
					chat.SendChat(selectedPlayer,chatInput);	
				}else{
					chat.SendChat(PhotonTargets.All,chatInput);	
				}
				chatInput = "";
			}
			GUILayout.Window(1,new Rect(Screen.width-Screen.width*.2f,Screen.height-Screen.width*.28f,Screen.width*.2f,Screen.width*.25f),ChatWindow,"C H A T");}
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("C H A T")){showChat=!showChat;}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		
    	if (async != null){
			float progW=Screen.width*.2f;
			float progH=Screen.height*.02f;
			float progX=Screen.width*.5f-(progW*.5f);
			float progY=Screen.height*.5f-(progH*.5f);
    		GUI.DrawTexture(new Rect(progX, progY, progW, progH), EmptyProgressBar);
    		GUI.DrawTexture(new Rect(progX, progY, progW * async.progress, progH), FullProgressBar);
    	}
	}
	
	void PlayerListWindow(int windowID){
        Vector2 scrollPos = Vector2.zero;
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		foreach(PhotonPlayer player in playerList.Players){
			if(selectedPlayer != player){
				if(GUILayout.Button(player.name,unselected) && player != playerList.LocalPlayer){
					selectedPlayer = player;
				}
			}else{
				if(GUILayout.Button(player.name,selected)){
					selectedPlayer = null;
				}
			}
		}
        GUILayout.EndScrollView();	
	}
	
	void ChatWindow(int windowID){
		Vector2 scrollPos = new Vector2(Mathf.Infinity,Mathf.Infinity);;
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		foreach(string message in chat.Messages){
			GUILayout.Label(message);
		}
		GUILayout.EndScrollView();	
	}
	
	void HelpWindow(int windowID){
		GUILayout.Label("Help");
	}
	
}
