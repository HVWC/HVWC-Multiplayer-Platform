// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using System.Collections.Generic;

/// <summary>
///  This class handles public and private chat messaging between players.
/// </summary>
public class Chat : Photon.MonoBehaviour{

    public delegate void GotChat(string message);
    public static event GotChat OnGotChat;

	#region Fields
	/// <summary>
	///  A list of the messages that have been sent.
	/// </summary>
    public static List<string> Messages = new List<string>();
    /// <summary>
	///  The max number of messages to keep.
	/// </summary>
    public int maxNumberOfMessages = 15;
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
        string message = "[" + info.sender + "] " + text;
        Messages.Add(message);
        if (Messages.Count > maxNumberOfMessages){
            Messages.RemoveAt(0);
		}
        if(OnGotChat != null) {
            OnGotChat(message);
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

    /// <summary>
	///  A method to clear all chat messages.
	/// </summary>
    public static void ClearMessages() {
        Messages.Clear();
    }
    #endregion

    void OnLeftRoom() {
        ClearMessages();
    }

}
