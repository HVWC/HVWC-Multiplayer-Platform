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

public class GameManager : Photon.MonoBehaviour {
	
	public static GameManager Instance;
	
    // this is a object name (must be in any Resources folder) of the prefab to spawn as player avatar.
    // read the documentation for info how to spawn dynamically loaded game objects at runtime (not using Resources folders)
	[System.Serializable]
	public class Avatar{
		public string name;
		public GameObject prefab;
		public Texture icon;		
	}
	public Avatar[] avatars;
	[HideInInspector]
	public string selectedAvatar = "";
	public GUISkin photonSkin;
	
	public float spawnRadius=3f;
	
	void Awake(){
		if(Instance){
			DestroyImmediate(gameObject);
		}else{
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
	}
	
	void Start(){
		if(selectedAvatar==""){selectedAvatar = avatars[0].name;}
	}
	
    void OnJoinedRoom(){
        StartGame();
    }
    
    IEnumerator OnLeftRoom(){
        //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
        while(PhotonNetwork.room!=null || PhotonNetwork.connected==false){
            yield return 0;
		}
        Application.LoadLevel(Application.loadedLevel);
    }

    void StartGame(){
        Camera.main.farClipPlane = 1000; //Main menu set this to 0.4 for a nicer BG    
		
		Vector3 randomPos = new Vector3(Random.Range(transform.position.x-spawnRadius,transform.position.x+spawnRadius),transform.position.y,Random.Range(transform.position.z-spawnRadius,transform.position.z+spawnRadius));
		
        // Spawn our local player
        PhotonNetwork.Instantiate(selectedAvatar, randomPos, gameObject.transform.rotation, 0);
    }
	
	void OnDrawGizmos(){
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position,spawnRadius);
	}

    void OnDisconnectedFromPhoton(){
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }
    void OnFailedToConnectToPhoton(){
        Debug.LogWarning("OnFailedToConnectToPhoton");
		PhotonNetwork.offlineMode = true;
    }
  
}
