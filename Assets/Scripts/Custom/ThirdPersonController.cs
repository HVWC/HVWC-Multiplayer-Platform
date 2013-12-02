﻿// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class ThirdPersonController : MonoBehaviour{
	
	private Rigidbody target; // The object we're steering
	public AnimationController animationController;
	
	public enum CharacterState{
		idle=0,
		walk=1,
		run=2,
		jump=3,
		sit=4,
		fly=5
	}
	private CharacterState charState;
	public CharacterState CharState{
		get{return charState;}
		set{
			charState=value;
			animationController.ActivateAnimation((int)charState);
		}
	}
	
	//Speeds
	public float walkSpeed=1.0f,runSpeed=1.5f,flySpeed=2.0f;
	private float speed;
	float speedThreshold = .1f;
	
	public float heightOffset = 0.0f,groundingDistance = 0.1f;
	public LayerMask groundLayers;
	
	public float gravity = 50.0f;
	private Vector3 downwardForce;
	
	[HideInInspector]
	public bool grounded,flying,isRemotePlayer = true;
		
	void Start(){ // Verify setup, configure rigidbody
		if (target == null){
			target = GetComponent<Rigidbody> ();
		}
		target.freezeRotation = true; // We will be controlling the rotation of the target, so we tell the physics system to leave it be
		downwardForce = new Vector3(0,-gravity,0);
	}
	
	
	void Update(){ 
		
		/*switch(characterState){
			case CharacterState.idle: animationController.ActivateAnimation("idle"); break;
			case CharacterState.walk: animationController.ActivateAnimation("walk"); break;	
			case CharacterState.run: animationController.ActivateAnimation("run"); break;	
			case CharacterState.jump: animationController.ActivateAnimation("jump"); break;	
			case CharacterState.sit: animationController.ActivateAnimation("sit"); break;	
			case CharacterState.fly: animationController.ActivateAnimation("fly"); break;	
			default: break;	
		}*/
		
		if (isRemotePlayer){
			return;
		}
		
		if(flying && !grounded){
			speed = flySpeed;
		}else if(Input.GetKey(KeyCode.LeftShift) && grounded){
			speed = runSpeed;
		}else if(grounded){
			speed = walkSpeed;
		}
	}

	void FixedUpdate(){	// Handle movement here since physics will only be calculated in fixed frames anyway	
		
		grounded = Physics.Raycast( // Shoot a ray downward to see if we're touching the ground
			target.transform.position + target.transform.up * -heightOffset,
			target.transform.up * -1,
			groundingDistance,
			groundLayers
		);	
		
		if (isRemotePlayer){
			return;
		}
		
		if(!flying){
			target.AddForce(downwardForce, ForceMode.Force); //added gravity to make it more natural
		}
		
		//Movement
		Vector3 movement = new Vector3(); //has caused problems before
		if(grounded || flying){
			movement = Input.GetAxis("Vertical") * target.transform.forward;	
			target.AddForce(movement.normalized*speed,ForceMode.VelocityChange);
		}
		
		//Jump
		if(grounded){
			if (Input.GetButton ("Jump")){ // Handle jumping // When jumping, we set the velocity upward with our jump speed plus some application of directional movement
				target.AddForce (25 * target.transform.up + (target.velocity*target.velocity.magnitude),ForceMode.VelocityChange);
			}	
		}
		
		//Rotation
		float rotationAmount = Input.GetAxis("Horizontal")* 2f * Time.deltaTime;
		target.transform.RotateAround (target.transform.up, rotationAmount);
		
		//Flyf
		Vector3 fly = Input.GetAxis("Fly") * target.transform.up;	
		target.AddForce(fly.normalized*flySpeed,ForceMode.VelocityChange);
		if(!grounded && Mathf.Abs(Input.GetAxis("Fly"))>0){
			Fly(true);
		}
		if(flying && grounded){
			Fly(false);
		}
		if(Input.GetKeyDown(KeyCode.F)){
			if(!flying){
				Fly(true);
				target.AddForce(target.transform.up*(flySpeed+2),ForceMode.VelocityChange); //the +2 is to give the avatar a little boost to get ungrounded, but maintain a more realistic flySpeed
			}else{
				Fly(false);
			}
		}
		
		//Animations
		if(grounded){
			if(!movement.AlmostEquals(Vector3.zero,.01f)){
				if (System.Math.Abs (speed - walkSpeed) < speedThreshold) {
					CharState = CharacterState.walk;
				}
				if(System.Math.Abs (speed - runSpeed) < speedThreshold){
					CharState = CharacterState.run;
				}
			}else{
				CharState = CharacterState.idle;
			}
		}else{
			if(flying){
				CharState = CharacterState.fly;	
			}else{
				CharState = CharacterState.jump;	
			}
		}
		
	}
	
	public void Fly(bool val){
		rigidbody.useGravity = !val;
		flying = val;
	}
	
	public void SetIsRemotePlayer(bool val){
        isRemotePlayer = val;
    }
	
	void OnDrawGizmos(){ // Use gizmos to gain information about the state of your setup
		Gizmos.color = grounded ? Color.blue : Color.red;
		Gizmos.DrawLine (target.transform.position + target.transform.up * -heightOffset,
			target.transform.position + target.transform.up * -(heightOffset + groundingDistance));
	}

}
