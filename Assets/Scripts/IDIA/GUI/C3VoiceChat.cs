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
/// This class allows for the creation or joining of rooms in the Vivox C3 voice chat application.
/// </summary>
public class C3VoiceChat : MonoBehaviour{

	#region Fields
	/// <summary>
	/// An identifier that associates this application with channels on C3.
	/// </summary>
	static string c3AppId = "HVWC_Multiplayer_Platform";
	#endregion

	#region Methods
	/// <summary>
	/// Joins or creates a C3 voice channel with the player's username.
	/// </summary>
	/// <param name="roomName">The room name.</param>
	/// <param name="userName">The player's username</param>
	public static void JoinVoice(string roomName, string userName){
		// construct the URL for the web page that allows download of C3 and also redirects to a URL protocal request that causes C3 to join the specified channel
		string c3Url = string.Format ("http://social.c3p.vivox.com/lite_protocol.php?app={0}&channel={1}&user={2}", c3AppId, roomName, userName);
		if(Application.isWebPlayer){ //If this is a webplayer
			Application.ExternalEval("window.open('" + c3Url + "','C3');"); //Just open the C3 URL in a new tab
		}else{ //If this is not a webplayer
			string c3Path = System.Environment.GetEnvironmentVariable("C3PATH"); //Check for an environment variable for C3
			//Construct a protocol address that allows this application to initialize C3 with our parameters
			string c3Protocol = string.Format("c3:TP-{0}-{1}-{2}", c3AppId, roomName, userName);
			if (!string.IsNullOrEmpty(c3Path)){ //If there is an environment variable for C3
				Application.OpenURL(c3Protocol); //Open the protocol in the browser to initialize C3 with our parameters
			}else{ //If there is no environment variable
				c3Path = "C:\\Program Files (x86)\\Vivox\\C3";
				if(System.IO.File.Exists(c3Path + "\\C3.exe") || System.IO.File.Exists(c3Path + "\\c3.exe")){ //Check the filesystem for a common install location of C3
					Application.OpenURL(c3Protocol); //If it exists, open the protocol in the browser
				}else{
					Application.OpenURL(c3Url); //If not, just open the C3 URL in the browser
				}
			}
		}
	}
	#endregion

}