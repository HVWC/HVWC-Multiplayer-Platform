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
	
    ThirdPersonCamera cameraScript;
    ThirdPersonController controllerScript;
	
    void Awake(){
        cameraScript = GetComponent<ThirdPersonCamera>();
        controllerScript = GetComponent<ThirdPersonController>();

    }
    void Start(){
        //TODO: Bugfix to allow .isMine and .owner from AWAKE!
        if (photonView.isMine){
            //MINE: local player, simply enable the local scripts
            cameraScript.enabled = true;
            controllerScript.enabled = true;
            Camera.main.transform.parent = transform;
            Camera.main.transform.localPosition = new Vector3(0, 2, -10);
            Camera.main.transform.localEulerAngles = new Vector3(10, 0, 0);
			DontDestroyOnLoad(gameObject); //make avatar survive a scene change
        }else{           
            cameraScript.enabled = false;
            controllerScript.enabled = true;
			DontDestroyOnLoad(gameObject);
			//this.enabled = false;
        }
        controllerScript.SetIsRemotePlayer(!photonView.isMine);
        gameObject.name = gameObject.name + photonView.viewID;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if (stream.isWriting){
            //We own this player: send the others our data
            stream.SendNext((int)controllerScript.CharState);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            //stream.SendNext(rigidbody.velocity); 

        }else{
            //Network player, receive data
            controllerScript.CharState = (ThirdPersonController.CharacterState)(int)stream.ReceiveNext();
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
            //rigidbody.velocity = (Vector3)stream.ReceiveNext();
        }
    }

    private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this

    void Update(){
        if (!photonView.isMine){
            //Update remote player (smooth this, this looks good, at the cost of some accuracy)
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
        }
    }

    void OnPhotonInstantiate(PhotonMessageInfo info){
        //We know there should be instantiation data..get our bools from this PhotonView!
        //object[] objs = photonView.instantiationData; //The instantiate data.. //I commented out, because wasn't being used
        //bool[] mybools = (bool[])objs[0];   //Our bools! //I commented out, because wasn't being used

        //disable the axe and shield meshrenderers based on the instantiate data
        //MeshRenderer[] rens = GetComponentsInChildren<MeshRenderer>(); //I commented out, because wasn't being used
        //rens[0].enabled = mybools[0];//Axe
        //rens[1].enabled = mybools[1];//Shield
    }

}