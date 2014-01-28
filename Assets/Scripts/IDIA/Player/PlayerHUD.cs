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
/// This class displays the local player's HUD.
/// </summary>
public class PlayerHUD : MonoBehaviour {

	#region Fields
	/// <summary>
	/// An instance of the Chat component.
	/// </summary>
	Chat chat;

	/// <summary>
	/// An instance of the PlayerList component
	/// </summary>
	PlayerList playerList;

	//The selected player from the player list
	PhotonPlayer selectedPlayer;

	//GUI styles used to style the buttons of unselected and selected players
	GUIStyle unselected,selected;

	//Rectangles and floats used to map out the area on the screen the HUD and its elements should occupy
	Rect topHudRect,bottomHudRect;
	float topHudHeight,bottomHudHeight,buttonWH;

	//The map camera used to display a 2D orthographic map of the 3D world
	Camera mapCamera;

	//Booleans to check whether to show different parts of the HUD
	bool showHelp,showPlayers,showChat,showMap,showVolume,showHUD=true;

	//A float used to change the volume
	float volume = 100;

	//A string used to hold the chat text field value
	string chatInput = "";

	/// <summary>
	/// The custom skin used to style the player HUD.
	/// </summary>
	public GUISkin CustomSkin;

	/// <summary>
	/// The C3 button texture.
	/// </summary>
	public Texture2D C3_Tex;

	/// <summary>
	/// The map button texture.
	/// </summary>
	public Texture2D MapTex;

	/// <summary>
	/// The volume button texture.
	/// </summary>
	public Texture2D VolumeTex;

	/// <summary>
	/// The help button texture.
	/// </summary>
	public Texture2D HelpTex;

	/// <summary>
	/// The exit button texture.
	/// </summary>
	public Texture2D ExitTex;

	/// <summary>
	/// The loading screen texture.
	/// </summary>
	public Texture2D loadingScreen;
	#endregion

	#region Unity Messages
	/// <summary>
	/// A message called once just before Update is called.
	/// </summary>
	void Start () {
		chat = FindObjectOfType(typeof(Chat)) as Chat; //Get the Chat component
		playerList = FindObjectOfType(typeof(PlayerList)) as PlayerList; //Get the PlayerList component
		unselected = CustomSkin.customStyles[0]; //Set unselected to the appropriate custom style set in the custom skin
		selected = CustomSkin.customStyles[1]; //Set unselected to the appropriate custom style set in the custom skin
		mapCamera = GameObject.Find("Map").camera; //Get the Camera component on the Map object
	}
	
	/// <summary>
	/// A message called every frame.
	/// </summary>
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

		//Minibutton width and height
		buttonWH = Screen.width*.022f;

		AudioListener.volume = volume * .01f; //Volume is a float from 0-1 and since we are using a scale from 0-100, we need to set the actual volume to be 1/100 of our variable
		
		if(Input.GetKeyDown(KeyCode.Escape)){ //If the local player hits Escape, toggle the HUD
			showHUD = !showHUD;
		}
		
	}

	/// <summary>
	/// A message called for rendering and handling GUI events.
	/// </summary>
	void OnGUI(){
		//Loading screen
		GUI.depth=-1000; //Make sure the loading screen renders on top of everything else
		if(Application.isLoadingLevel){ //If we are loading a level, show the loading screen
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),loadingScreen);
		}
		GUI.depth=0; //Set the depth to 0 for everything else

		if(NetworkManager.Instance.Room==null || !showHUD || Application.isLoadingLevel){ //If the local player is not in a room, if the HUD is toggled off, or if a level is loading
			return;	//Do not show the rest of the HUD
		}

		GUI.skin = CustomSkin; //Set the GUI skin to the CustomSkin and set screen-sensitive properties
		CustomSkin.button.fontSize = Mathf.RoundToInt(Screen.width*.013f);
		CustomSkin.window.fontSize = Mathf.RoundToInt(Screen.width*.013f);
		CustomSkin.label.fontSize = Mathf.RoundToInt(Screen.width*.01f);
		
		//Top HUD
		GUILayout.BeginArea(topHudRect);
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Load LVL1")){NetworkManager.Instance.LoadLevel("Demo");} //If player hits button, load the first level
		if(GUILayout.Button("Load LVL2")){NetworkManager.Instance.LoadLevel("Demo 2");} //If player hits button, load the second level
		GUILayout.FlexibleSpace();
		if(GUILayout.Button(C3_Tex,"miniButton",GUILayout.Width(buttonWH),GUILayout.Height(buttonWH))){C3VoiceChat.JoinVoice(NetworkManager.Instance.Room.name,NetworkManager.Instance.PlayerName);} //If player hits button, start or join a room in C3
		if(GUILayout.Button(MapTex,"miniButton",GUILayout.Width(buttonWH),GUILayout.Height(buttonWH))){showMap=!showMap;mapCamera.enabled = WebWarpLocalPlayer.Instance.showCoords = showMap;} //If player hits button, toggle map and GeoCoord controls
		GUILayout.BeginVertical();
		if(GUILayout.Button(VolumeTex,"miniButton",GUILayout.Width(buttonWH),GUILayout.Height(buttonWH))){showVolume=!showVolume;} //If player hits button, toggle volume controls
		if(showVolume){volume = GUILayout.VerticalSlider(volume,100,0,GUILayout.Width(buttonWH),GUILayout.Height((topHudHeight-buttonWH)*.9f));}
		GUILayout.EndVertical();
		if(showHelp){GUILayout.Window(2,new Rect((Screen.width*.5f)-(Screen.width*.2f),(Screen.height*.5f)-(Screen.height*.2f),Screen.width*.4f,Screen.height*.4f),HelpWindow,"H E L P");}
		if(GUILayout.Button(HelpTex,"miniButton",GUILayout.Width(buttonWH),GUILayout.Height(buttonWH))){showHelp=!showHelp;} //If player hits button, toggle help window
		if(GUILayout.Button(ExitTex,"miniButton",GUILayout.Width(buttonWH),GUILayout.Height(buttonWH))){NetworkManager.Instance.LeaveRoom();} //If player hits button, leave the room
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		
		//Bottom HUD
		GUILayout.BeginArea(bottomHudRect);
		GUILayout.BeginHorizontal();
		if(showPlayers){GUILayout.Window(0,new Rect(0,Screen.height-Screen.width*.28f,Screen.width*.2f,Screen.width*.25f),PlayerListWindow,"P L A Y E R S");}
		if(GUILayout.Button("P L A Y E R S")){showPlayers=!showPlayers;} //If player hits button, toggle player list window
		GUILayout.FlexibleSpace();
		if(showChat){
			chatInput = GUILayout.TextField(chatInput,GUILayout.Width(Screen.width*.7f)); //The chat message
			if(GUILayout.Button("S E N D")){
				if(selectedPlayer!=null){
					chat.SendChat(selectedPlayer,chatInput); //If there is a selected player in the player list, send the message to that player in private
				}else{
					chat.SendChat(PhotonTargets.AllBuffered,chatInput);	//If there is no selected player, send the message to all players
				}
				chatInput = ""; //Reset the text field
			}
			GUILayout.Window(1,new Rect(Screen.width-Screen.width*.2f,Screen.height-Screen.width*.28f,Screen.width*.2f,Screen.width*.25f),ChatWindow,"C H A T");}
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("C H A T")){showChat=!showChat;} //If player hits button, toggle chat window
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
	#endregion

	#region GUI Windows
	/// <summary>
	/// A method called from OnGUI that renders the player list window.
	/// </summary>
	void PlayerListWindow(int windowID){
        Vector2 scrollPos = Vector2.zero;
		scrollPos = GUILayout.BeginScrollView(scrollPos); //Allow the local player to scroll through a list of players
		foreach(PhotonPlayer player in playerList.Players){ //For each player in the list
			if(selectedPlayer != player){ //If it is not the selected player
				if(GUILayout.Button(player.name,unselected) && player != playerList.LocalPlayer){ //If the local player hits the button and it is not the local player
					selectedPlayer = player; //Set this player as the selected player
				}
			}else{ //If it is the selected player
				if(GUILayout.Button(player.name,selected)){ //If the local player hits the button
					selectedPlayer = null; //Nullify the selected player
				}
			}
		}
        GUILayout.EndScrollView();	
	}

	/// <summary>
	/// A method called from OnGUI that renders the chat window.
	/// </summary>
	void ChatWindow(int windowID){
		Vector2 scrollPos = new Vector2(Mathf.Infinity,Mathf.Infinity);
		scrollPos = GUILayout.BeginScrollView(scrollPos); //Allow the local player to scroll through a list of chat messages
		foreach(string message in chat.Messages){ //For each message in the chat messages
			GUILayout.Label(message); //Display the message in a label
		}
		GUILayout.EndScrollView();	
	}

	/// <summary>
	/// A method called from OnGUI that renders the help window.
	/// </summary>
	void HelpWindow(int windowID){
		GUILayout.Label("Help"); //Display the help
	}
	#endregion
	
}
