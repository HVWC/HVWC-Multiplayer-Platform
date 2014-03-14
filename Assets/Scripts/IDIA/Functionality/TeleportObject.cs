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
/// This class allows for teleportation by clicking on a map icon.
/// </summary>
public class TeleportObject : MonoBehaviour {

	#region Fields
	/// <summary>
	/// The map camera.
	/// </summary>
	Camera mapCamera;
	/// <summary>
	/// The icon.
	/// </summary>
	GameObject icon;
	/// <summary>
	/// The local player.
	/// </summary>
	GameObject player;
	#endregion

	#region Unity Messages
	/// <summary>
	/// A message called once just before Update is called.
	/// </summary>
	void Start () {
		mapCamera = GameObject.Find("Map").GetComponent<Camera>();
		icon = transform.FindChild("Icon").gameObject;
		player = GameObject.FindGameObjectWithTag("LocalPlayer");
	}

	/// <summary>
	/// A message called every frame.
	/// </summary>
	void Update () {
		Ray ray = mapCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(icon.collider.Raycast(ray, out hit, Mathf.Infinity)){
			if(Input.GetMouseButtonUp(0)){
				player.transform.position = transform.position;
			}
		}
	}
	#endregion

	#region Photon Messages
	/// <summary>
	/// A message called when the local player joins a room.
	/// </summary>
	void OnJoinedRoom(){
		player = GameObject.FindGameObjectWithTag("LocalPlayer");
	}

	/// <summary>
	/// A message called when the local player leaves a room.
	/// </summary>
	void OnLeftRoom(){
		player = null;
	}
	#endregion

}
