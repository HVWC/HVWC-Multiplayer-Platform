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
///  This class handles displaying the player's name tag.
/// </summary>
public class NameTag : MonoBehaviour {

	#region Fields
	/// <summary>
	///  An instance of the PhotonView component on the player.
	/// </summary>
	PhotonView pv;

	/// <summary>
	///  An instance of the TextMesh component on this gameobject.
	/// </summary>
	TextMesh nameTag;

	/// <summary>
	///  The color the text of name tag should be.
	/// </summary>
	public Color nameTagColor;

	/// <summary>
	///  The distance at which the name tag should start to fade.
	/// </summary>
	public float fadeDistance;
	#endregion

	#region Unity Messages
	/// <summary>
	/// A message called once just before Update is called.
	/// </summary>
	void Start () {
		pv = transform.parent.GetComponent<PhotonView>(); //Get the PhotonView component
		if(!pv.isMine){ //If this is a remote player, get the TextMesh component and set its text property to the name of the remote player
			nameTag = GetComponent<TextMesh>();
			nameTag.text = pv.owner.name;
		}else{ //Otherwise, just turn this whole gameobject off.
			transform.gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// A message called every frame.
	/// </summary>
	void Update () {
		transform.LookAt(Camera.main.transform); //Make sure the gameobject is always facing the camera, so it will look like a GUI element rather than a 3D object
		nameTagColor.a = fadeDistance/Vector3.Distance(transform.position,Camera.main.transform.position); //Set the alpha to the fadeDistance divided by the actual distance between it and the camera
		renderer.material.color = nameTagColor; //Set the color
	}
	#endregion

}
