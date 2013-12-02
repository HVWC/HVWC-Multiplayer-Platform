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

public class ClickTeleport : MonoBehaviour {
	
	private bool mouseGround = false;
	private Ray ray;
	private RaycastHit hit;
	public float radius = 100f;
	
	private bool doubleClicked = false;
	public float doubleClickSensitivity = .3f;
	private float startTime;
	private bool startTest = false;
	
	void Start () {
	
	}
	
	void Update () {
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Input.GetMouseButtonDown(0)){
			if(!startTest){
				startTest = true;
				startTime = Time.time;
			}else{
				if(Time.time-startTime<doubleClickSensitivity){
					doubleClicked = true;
					startTest = false;
				}else{
					doubleClicked = false;	
					startTest = false;
				}
			}
		}
		if(mouseGround && doubleClicked){
			transform.position = hit.point;
			doubleClicked = false;
		}
	}
	
	void FixedUpdate(){
		mouseGround = Physics.Raycast(ray,out hit,radius);
	}
 
}
