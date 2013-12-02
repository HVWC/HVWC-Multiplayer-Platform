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

public class TeleportObject : MonoBehaviour {
	
	public Material teleportMarker;
	private Camera miniMapCamera;
	private bool mouseOver;
	private GameObject icon;
	private GameObject target;
	
	// Use this for initialization
	void Start () {
		icon = GameObject.CreatePrimitive(PrimitiveType.Plane);
		icon.name = "Icon";
		icon.layer = 10; //MiniMap Layer
		icon.transform.parent = this.transform;
		icon.transform.localPosition = new Vector3(0,50,0);
		icon.renderer.material = teleportMarker;
	}
	
	// Update is called once per frame
	void Update () {
		if(PhotonNetwork.room!=null){
			miniMapCamera = GameObject.Find("MiniMap").GetComponent<Camera>();
			target = GameObject.FindGameObjectWithTag("Player");
		}
		Ray ray = miniMapCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(icon.collider.Raycast(ray, out hit, Mathf.Infinity)){
			if(Input.GetMouseButtonUp(0)){
				Debug.Log("teleport");
				target.transform.position = transform.position;
			}
		}
	}
}
