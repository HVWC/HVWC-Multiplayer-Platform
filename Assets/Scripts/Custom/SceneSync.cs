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

public class SceneSync : Photon.MonoBehaviour {
	
	[RPC]
	void SyncScene(int sceneID,PhotonMessageInfo info){
		for(int i=0;i<info.photonView.transform.childCount;i++){
			info.photonView.transform.GetChild(i).gameObject.SetActive(sceneID==Application.loadedLevel); //set the sender's local children to be enabled or disabled depending on their relative scene
		}
		photonView.RPC("ReturnSyncScene",info.sender,Application.loadedLevel); //Send the sender our current scene
		Debug.Log(info+" sent response");
		Debug.Log(info.sender+" sent call");
	}
	
	[RPC]
	void ReturnSyncScene(int sceneID,PhotonMessageInfo info){
		Debug.Log (info.photonView.ownerId+" "+(sceneID==Application.loadedLevel).ToString());
		for(int i=0;i<info.photonView.transform.childCount;i++){
			info.photonView.transform.GetChild(i).gameObject.SetActive(sceneID==Application.loadedLevel); //set the sender's local children to be enabled or disabled depending on their relative scene
		}
		Debug.Log(info+" sent response");
		Debug.Log(info.sender+" sent response");
	}
	
	void OnLevelWasLoaded(int level){
		PhotonNetwork.isMessageQueueRunning=true;
		if(photonView.isMine){
			photonView.RPC("SyncScene",PhotonTargets.Others,level);
		}
	}
	
}
