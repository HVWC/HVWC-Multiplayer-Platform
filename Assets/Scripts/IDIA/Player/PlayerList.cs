// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------

/// <summary>
/// This class wraps useful network player methods.
/// </summary>
public class PlayerList : Photon.MonoBehaviour{

	#region Properties
	/// <summary>
	/// Gets all networked players.
	/// </summary>
	/// <value>All networked players.</value>
	public PhotonPlayer[] Players{
		get{return PhotonNetwork.playerList;}
	}

	/// <summary>
	/// Gets the local player.
	/// </summary>
	/// <value>The local player.</value>
	public PhotonPlayer LocalPlayer{
		get{return PhotonNetwork.player;}
	}

	/// <summary>
	/// Gets the remote players.
	/// </summary>
	/// <value>The remote players.</value>
	public PhotonPlayer[] RemotePlayers{
		get{return PhotonNetwork.otherPlayers;}
	}
	#endregion

}
