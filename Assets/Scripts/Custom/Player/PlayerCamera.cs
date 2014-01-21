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

public class PlayerCamera : MonoBehaviour{
	GameObject target;
    Camera playerCamera;
	public float damping = 1;
	private Vector3 targetOnBody;
    void Start(){
		if (target == null){
			target = this.gameObject;
		}
		if (playerCamera == null){
			if (Camera.main != null){
				playerCamera = Camera.main;
			}
		}
		playerCamera.transform.parent = target.transform;
		playerCamera.transform.position = new Vector3(target.transform.position.x,target.transform.position.y+1.5f,target.transform.position.z+3f);
		playerCamera.name = "PlayerCamera";
    }
	
	void Update(){
		if(PhotonNetwork.room!=null){
			if(GameObject.Find("Main Camera")!=null){
				Destroy(GameObject.Find("Main Camera"));
				playerCamera.farClipPlane = 1000;
			}
		}
	}
	
    void LateUpdate(){
		targetOnBody = new Vector3(target.transform.position.x,target.transform.position.y+1f,target.transform.position.z);
        playerCamera.transform.LookAt(targetOnBody);
    }
	
}
