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
using System; 
using System.IO;

public class C3JoinVoice : MonoBehaviour{
	
	private static string c3AppId = "HVWC";

	void OnLeftRoom()
    {
        this.enabled = false;
    }

    void OnJoinedRoom()
    {
        this.enabled = true;
    }
    void OnCreatedRoom()
    {
        this.enabled = true;
    }
	
	
	public static void JoinVoice(string channelId, string userId)
	{
		
		// construct the URL for the web page that allows download of C3 and also redirects to a URL protocal request that causes C3 to join the specified channel
		string sUrl = string.Format ("http://social.c3p.vivox.com/lite_protocol.php?app={0}&channel={1}&user={2}", c3AppId, channelId, userId);
		string sProtocol = "c3:TP-" + c3AppId + "-" + channelId + "-" + userId;
		
#if UNITY_WEBPLAYER
		// start browser and show web page
		if(Application.isWebPlayer){
			Application.ExternalEval("window.open('" + sUrl + "','C3');");
		}else{
			Application.OpenURL(sProtocol);
		}
		
#elif UNITY_STANDALONE_WIN
		// check if C3 is installed
		string sC3Path = System.Environment.GetEnvironmentVariable("C3PATH");
		
		if (string.IsNullOrEmpty(sC3Path)){
			sC3Path = "C:\\Program Files (x86)\\Vivox\\C3";
		}
		
		if (System.IO.File.Exists(sC3Path + "\\C3.exe") || System.IO.File.Exists(sC3Path + "\\c3.exe")){
			// C3 is installed
			// send URL protocol request to C3 without starting the browser to show the web page
			Application.OpenURL(sProtocol);
		}else{
			// start browser and show web page
			Application.OpenURL(sUrl);
		}
#elif UNITY_STANDALONE_OSX
		// check if C3 is installed (on OSX we only check for the existance of the environment variable)
		string sC3Path = System.Environment.GetEnvironmentVariable("C3PATH");
		// 05/2012,  the C3PATH does not exist even after C3 is installed on a Mac

		if (!String.IsNullOrEmpty(sC3Path))
		{
			// C3 is installed
			// send URL protocol request to C3 without starting the browser to show the web page
			string sProtocol = "c3:TP-" + yourC3AppId + "-" + channelId + "-" + userId;
			Application.OpenURL(sProtocol);
		}
		else
		{
			// start browser and show web page
			Application.OpenURL(sUrl);
		}
		
#else
		// start browser and show web page
		//Application.OpenURL(sUrl);
#endif
	}
}