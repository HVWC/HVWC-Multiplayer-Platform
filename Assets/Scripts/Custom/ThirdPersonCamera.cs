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

public class ThirdPersonCamera : MonoBehaviour{
	GameObject target;
    new Camera camera;
	public float damping = 1;
    private Vector3 offset;
	private Vector3 targetOnBody;
    void Start(){
		if (target == null){
			target = this.gameObject;
		}
		if (camera == null){
			if (Camera.main != null){
				camera = Camera.main;
			}
		}
		camera.transform.parent = target.transform;
		camera.transform.position = new Vector3(target.transform.position.x,target.transform.position.y+1.5f,target.transform.position.z+3f);
        offset = target.transform.position - camera.transform.position;
		camera.name = "PlayerCamera";
    }
	
	void Update(){
		if(PhotonNetwork.room!=null){
			if(GameObject.Find("Main Camera")!=null){
				Destroy(GameObject.Find("Main Camera"));
				camera.farClipPlane = 1000;
			}
		}
	}
	
    void LateUpdate(){
        //float currentAngle = transform.eulerAngles.y;
        //float desiredAngle = target.transform.eulerAngles.y;
        //float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);
		//Vector3 velocity = transform.rigidbody.velocity*-.1f;
        //Quaternion rotation = Quaternion.Euler(0, angle, 0);
        //camera.transform.position = Vector3.Lerp(camera.transform.position,target.transform.position - (rotation * offset),Time.deltaTime);
		//camera.transform.position = Vector3.SmoothDamp(camera.transform.position,target.transform.position - (rotation * offset),ref velocity,.2f);
		targetOnBody = new Vector3(target.transform.position.x,target.transform.position.y+1f,target.transform.position.z);
        camera.transform.LookAt(targetOnBody);
    }
	
}
