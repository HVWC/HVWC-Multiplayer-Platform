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

public class NameTag : MonoBehaviour {
	
	private PhotonView pv;
	private TextMesh nameTag;
	public Color nameTagColor;
	public float fadeDistance;
	
	// Use this for initialization
	void Start () {
		pv = transform.parent.GetComponent<PhotonView>();
		if(!pv.isMine){
			nameTag = GetComponent<TextMesh>();
			nameTag.text = pv.owner.name;
		}else{
			transform.gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(Camera.main.transform);
		nameTagColor.a = fadeDistance/Vector3.Distance(transform.position,Camera.main.transform.position);
		renderer.material.color = nameTagColor;
	}
}
