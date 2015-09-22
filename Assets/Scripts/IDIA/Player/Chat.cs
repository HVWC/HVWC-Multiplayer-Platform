// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

/// <summary>
///  This class handles public and private chat messaging between players.
/// </summary>
public class Chat : Photon.MonoBehaviour{

	#region Fields
	/// <summary>
	///  A list of the messages that have been sent.
	/// </summary>
    public List<string> Messages = new List<string>();
	#endregion

	#region RPCs
	/// <summary>
	///  An RPC method to add a message to the message list.
	/// </summary>
	/// <param name="text">
	/// The message to send.
	/// </param>
	/// <param name="info">
	/// Info about the RPC call.
	/// </param>
	[PunRPC]
	void AddMessage(string text, PhotonMessageInfo info){
        Messages.Add("[" + info.sender + "] " + text);
        if (Messages.Count > 15){
            Messages.RemoveAt(0);
		}
    }
	#endregion

	#region Methods
	/// <summary>
	///  A method to send a public chat message.
	/// </summary>
	/// <param name="targets">
	/// The players to whom this message should be sent.
	/// </param>
	/// <param name="message">
	/// The message to send.
	/// </param>
    public void SendChat(PhotonTargets targets,string message){
        if (message != ""){
            photonView.RPC("AddMessage", targets, message);
        }
    }

	/// <summary>
	///  A method to send a private chat message.
	/// </summary>
	/// <param name="target">
	/// The player to whom this message should be sent.
	/// </param>
	/// <param name="message">
	/// The message to send.
	/// </param>
    public void SendChat(PhotonPlayer target,string message){
        if (message != ""){
            message = "[PM] " + message;
            photonView.RPC("AddMessage", target, message);
		}
    }
	#endregion

}
