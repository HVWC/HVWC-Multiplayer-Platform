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

public class ThirdPersonNetwork : Photon.MonoBehaviour{

    ThirdPersonController controllerScript;

	Vector3 correctPlayerPos = Vector3.zero;
	Quaternion correctPlayerRot = Quaternion.identity;
	int sceneID = 0;
	
    void Awake(){
        controllerScript = GetComponent<ThirdPersonController>();

    }
    void Start(){
		DontDestroyOnLoad(gameObject);
        gameObject.name = gameObject.name + photonView.viewID;
    }

	void Update(){
		if (!photonView.isMine){
			transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
			Renderer[] rs = transform.GetComponentsInChildren<Renderer>();
			foreach(Renderer r in rs){
				r.enabled = sceneID==Application.loadedLevel;
			}
			Collider[] cs = transform.GetComponentsInChildren<Collider>();
			foreach(Collider c in cs){
				c.enabled = sceneID==Application.loadedLevel;
			}
		}
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if (stream.isWriting){
            stream.SendNext((int)controllerScript.CharState);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
			stream.SendNext(Application.loadedLevel);

        }else{
            controllerScript.CharState = (ThirdPersonController.CharacterState)(int)stream.ReceiveNext();
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
			sceneID = (int)stream.ReceiveNext();
        }
    }

}