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
/// This class handles the movement and animations of the local player.
/// </summary>
public class PlayerController : MonoBehaviour{

	#region Structs, Enums, and Classes
	/// <summary>
	/// This enumeration lists the different character states.
	/// </summary>
	public enum CharacterState{
		/// <summary>
		/// The IDLE character state.
		/// </summary>
		IDLE=0,
		/// <summary>
		/// The WALK character state.
		/// </summary>
		WALK=1,
		/// <summary>
		/// The RUN character state.
		/// </summary>
		RUN=2,
		/// <summary>
		/// The JUMP character state.
		/// </summary>
		JUMP=3,
		/// <summary>
		/// The SIT character state.
		/// </summary>
		SIT=4,
		/// <summary>
		/// The FLY character state.
		/// </summary>
		FLY=5
	}
	#endregion

	#region Fields
	/// <summary>
	/// An instance of the Rigidbody component on this player.
	/// </summary>
	//Rigidbody rigidbody;

	/// <summary>
	/// An instance of the AniimationController component on this player.
	/// </summary>
	public AnimationController animationController;

	/// <summary>
	/// An instance of the CharacterState enumeration.
	/// </summary>
	CharacterState charState;

	/// <summary>
	/// The walk speed.
	/// </summary>
	public float walkSpeed=1.0f;
	/// <summary>
	/// The run speed.
	/// </summary>
	public float runSpeed=1.5f;
	/// <summary>
	/// The fly speed.
	/// </summary>
	public float flySpeed=2.0f;

	//The current speed
	float speed;

	//The differential between speeds use to determine animation states
	float speedThreshold = .1f;

	/// <summary>
	/// The height offset for where the grounded raycast should originate.
	/// </summary>
	public float heightOffset = 0.0f;

	/// <summary>
	/// The grounding distance.
	/// </summary>
	public float groundingDistance = 0.1f;

	/// <summary>
	/// The ground layers.
	/// </summary>
	public LayerMask groundLayers;

	/// <summary>
	/// The artificial gravity.
	/// </summary>
	public float gravity = 50.0f;

	//The downward force that will be applied to the player.
	Vector3 downwardForce;

	//Booleans to check if the player is grounded or flying
	/// <summary>
	/// A boolean to check if the player is grounded.
	/// </summary>
	[HideInInspector]
	public bool grounded;

	/// <summary>
	/// A boolean to check if the player is flying.
	/// </summary>
	[HideInInspector]
	public bool flying;
	#endregion

	#region Properties
	/// <summary>
	/// A property to get/set and trigger the character state of the player.
	/// </summary>
	public CharacterState CharState{
		get{return charState;}
		set{
			charState=value;
			animationController.ActivateAnimation((int)charState);
		}
	}
	#endregion
		
	#region Unity Messages
	/// <summary>
	/// A message called once just before Update is called.
	/// </summary>
	void Start(){
		rigidbody.freezeRotation = true; //Freeze the rotation, because we'll do that manually
		downwardForce = new Vector3(0,-gravity,0); //Set our artifical gravity variable
	}
	
	/// <summary>
	/// A message called every frame.
	/// </summary>
	void Update(){
		if(Input.GetKeyDown(KeyCode.F)){ //If the player hits the 'F' key
			rigidbody.AddForce(rigidbody.transform.up*(flySpeed+5),ForceMode.VelocityChange); //Add a little hop to get off the ground
			Fly(!flying); //Toggle flying
		}

		if(flying && !grounded){ //If the local player is flying, our speed should be equal to the flySpeed
			speed = flySpeed;
		}else if(Input.GetKey(KeyCode.LeftShift) && grounded){ //If the local player is grounded and hitting the Left Shift, our speed should be equal to the runSpeed
			speed = runSpeed;
		}else if(grounded){ //If the local player is just grounded, our speed should be equal to the walkSpeed
			speed = walkSpeed;
		}
	}

	/// <summary>
	/// A message called every fixed framerate frame.
	/// </summary>
	void FixedUpdate(){
		
		grounded = Physics.Raycast( // Shoot a ray downward to see if the local player is touching the ground
			rigidbody.transform.position + rigidbody.transform.up * -heightOffset,
			rigidbody.transform.up * -1,
			groundingDistance,
			groundLayers
		);	
		
		//Movement
		Vector3 movement;
		if(!flying){
			if(!grounded){
				speed = Mathf.Lerp(speed,0,Time.deltaTime); //If the local player is jumping or falling, decelerate
			}
			rigidbody.AddForce(downwardForce, ForceMode.Force); //If the local player is not flying, add gravity to make falling and movement more natural
		}
		movement = Input.GetAxis("Vertical") * rigidbody.transform.forward;
		rigidbody.AddForce(movement.normalized*speed,ForceMode.VelocityChange); //Add our movement * speed to the local player
		
		//Jump
		if (grounded && Input.GetButton ("Jump")){ //If the local player is grounded and hits the Jump button, make the rigidbody jump up
			rigidbody.AddForce (25 * rigidbody.transform.up,ForceMode.VelocityChange);
		}	
		
		//Rotation
		float rotationAmount = Input.GetAxis("Horizontal")* 100f * Time.deltaTime;
		rigidbody.transform.RotateAround (transform.position, rigidbody.transform.up, rotationAmount); //Add our rotation to the local player
		
		//Fly
		Vector3 fly = Input.GetAxis("Fly") * rigidbody.transform.up;
		rigidbody.AddForce(fly.normalized*flySpeed,ForceMode.VelocityChange);
		if(!flying && !grounded && Mathf.Abs(Input.GetAxis("Fly"))>0){ //If the local player has not been flying, is not grounded, and is inputting on the Fly axis, then the local player is flying
			Fly(true);
		}
		if(flying && grounded && rigidbody.velocity.y<=-.1f){ //If the local player has been flying, but now is grounded and falling, then the local player is not flying anymore
			Fly(false);
		}
		
		//Animations
		if(grounded){ //If the local player is not grounded
			if(!movement.AlmostEquals(Vector3.zero,.01f)){ //If the local player is moving
				if (System.Math.Abs (speed - walkSpeed) < speedThreshold) {
					CharState = CharacterState.WALK; //If the local player's speed is close to the walking speed, then it is walking
				}
				if(System.Math.Abs (speed - runSpeed) < speedThreshold){
					CharState = CharacterState.RUN; //If the local player's speed is close to the running speed, then it is running
				}
			}else{
				CharState = CharacterState.IDLE; //If the local player is not moving, then it is idling
			}
		}else{ //If the local player is not grounded
			if(flying){
				CharState = CharacterState.FLY;	//If the local player is flying, then it is flying
			}else{
				CharState = CharacterState.JUMP; //If the local player is not grounded and not flying, then it is jumping
			}
		}
		
	}
	#endregion

	#region Methods
	/// <summary>
	/// A method to enable flying and set properties to allow flight.
	/// </summary>
	public void Fly(bool val){
		rigidbody.useGravity = !val; //Turn on/off gravity depending on whether the local player is flying
		flying = val; //Toggle flying
	}
	#endregion

}
