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
///  This class handles the networking of important player properties.
/// </summary>
public class NetworkPlayer : Photon.MonoBehaviour{

	#region Fields
	/// <summary>
	///  An instance of the PlayerController component on the player.
	/// </summary>
    PlayerController controllerScript;

	/// <summary>
	///  The actual position of the remote player we should lerp to.
	/// </summary>
	Vector3 correctPlayerPos = Vector3.zero;

	/// <summary>
	///  The actual rotation of the remote player we should lerp to.
	/// </summary>
	Quaternion correctPlayerRot = Quaternion.identity;

	/// <summary>
	///  The current scene of the remote player.
	/// </summary>
	int sceneID = 0;

	/// <summary>
	///  A boolean to prevent lerping on the first update.
	/// </summary>
	bool gotFirstUpdate;
	#endregion
	
    void Awake(){
		DontDestroyOnLoad(gameObject); //Make this player survive scene changes
        controllerScript = GetComponent<PlayerController>(); //Get the PlayerController component
		gameObject.name = gameObject.name + photonView.viewID; //Add the players network id to the end of their gameobject name
    }

	void Update(){
		if (!photonView.isMine){ //If this is a remote player
			//transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5); //Lerp from the player's current position to their actual position
			//transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5); //Lerp from the player's current rotation to their actual rotation
			Renderer[] rs = transform.GetComponentsInChildren<Renderer>();
			foreach(Renderer r in rs){
				r.enabled = sceneID==Application.loadedLevel; //Turn on/off each renderer on the player depending on their relative scene
			}
			Collider[] cs = transform.GetComponentsInChildren<Collider>();
			foreach(Collider c in cs){
				c.enabled = sceneID==Application.loadedLevel; //Turn on/off each collider on the player depending on their relative scene
			}
		}
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if (stream.isWriting){ //If this is the local player, send the remote players our animation, position, rotation, and scene
            stream.SendNext(controllerScript.CharState);
            //stream.SendNext(transform.position);
            //stream.SendNext(transform.rotation);
			stream.SendNext(Application.loadedLevel);

		}else{ //If this is the remote player, receive their animation, position, rotation, and scene
            controllerScript.CharState = (PlayerController.CharacterState)(int)stream.ReceiveNext();
            //correctPlayerPos = (Vector3)stream.ReceiveNext();
            //correctPlayerRot = (Quaternion)stream.ReceiveNext();
			sceneID = (int)stream.ReceiveNext();

			/*if(!gotFirstUpdate){ //If this is the first update, forget about lerping and just get the player to their position and rotation
				gotFirstUpdate=true;
				transform.position = correctPlayerPos;
				transform.rotation = correctPlayerRot;
			}*/

        }
    }

    void OnLeftRoom() {
        Destroy(gameObject);
    }

}