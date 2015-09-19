using UnityEngine;
using System.Collections;

/**
 * This class handles the movement of the player and its interaction with the keyboard
 * and mouse.
 *
 * Modified by: Benjamin Niedzielski (bniedzie@ucla.edu)
 * Last modified: 9/3/15
 */
public class PlayerCharacterController : MonoBehaviour {

	// The speed when walking
	public float walkSpeed = 3.0f;
	// after trotAfterSeconds of walking we trot with trotSpeed
	public float trotSpeed = 4.0f;
	// when pressing "Fire3" button (cmd) we start running
	public float runSpeed = 6.0f;
	
	public float inAirControlAcceleration = 3.0f;
	
	// How high do we jump when pressing jump and letting go immediately
	public float jumpHeight = 0.5f;
	// We add extraJumpHeight meters on top when holding the button down longer while jumping
	public float extraJumpHeight = 2.5f;
	
	// The gravity for the character
	public float gravity = 20.0f;
	// The gravity in cape fly mode
	public float capeFlyGravity = 2.0f;
	public float speedSmoothing = 10.0f;
	public float rotateSpeed = 500.0f;
	public float trotAfterSeconds = 3.0f;
	
	public bool canJump = true;
	public bool canCapeFly = true;
	public bool canWallJump = false;
	
	private float jumpRepeatTime = 0.05f;
	private float wallJumpTimeout = 0.15f;
	private float jumpTimeout = 0.15f;
	private float groundedTimeout = 0.25f;
	
	// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
	private float lockCameraTimer = 0.0f;
	
	// The current move direction in x-z
	private Vector3 moveDirection = Vector3.zero;
	// The current vertical speed
	private float verticalSpeed = 0.0f;
	// The current x-z move speed
	private float moveSpeed = 0.0f;
	
	// The last collision flags returned from controller.Move
	private CollisionFlags collisionFlags; 
	
	// Are we jumping? (Initiated with jump button and not grounded yet)
	private bool jumping = false;
	private bool jumpingReachedApex = false;
	
	// Are we moving backwards (This locks the camera to not do a 180 degree spin)
	private bool movingBack = false;
	// Is the user pressing any keys?
	public bool isMoving = false;
	// When did the user start walking (Used for going into trot after a while)
	private float walkTimeStart = 0.0f;
	// Last time the jump button was clicked down
	private float lastJumpButtonTime = -10.0f;
	// Last time we performed a jump
	private float lastJumpTime = -1.0f;
	// Average normal of the last touched geometry
	private Vector3 wallJumpContactNormal;
	private float wallJumpContactNormalHeight;
	
	// the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
	private float lastJumpStartHeight = 0.0f;
	// When did we touch the wall the first time during this jump (Used for wall jumping)
	private float touchWallJumpTime = -1.0f;
	
	private Vector3 inAirVelocity = Vector3.zero;
	
	private float lastGroundedTime = 0.0f;
	
	private float lean = 0.0f;

	//track the actual game model and animation controller so they can match the information used here
	public GameObject model;
	private AnimationController animController;
	
	// The vertical/horizontal input axes and jump button from user input, synchronized over network
	public float verticalInput;
	public float horizontalInput;
	public bool jumpButton;

	//enable input to be locked in cases where the camera is moved or moving
	public bool getUserInput = true;

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
	public CharacterState charState;

	/**
	 * This method changes the animation state to a new one
	 */
	public void setCharState(CharacterState newState) {
		charState = newState;
		animController.ActivateAnimation ((int)charState);
	}

	void Awake ()
	{
		//Get the avatar's model
		animController = model.GetComponent<AnimationController> ();
	}
	
	void UpdateSmoothedMovementDirection ()
	{
		//Only move the local player
		if (gameObject.tag == "LocalPlayer") {
			Transform cameraTransform = Camera.main.transform;
			bool grounded = IsGrounded ();
			
			// Forward vector relative to the camera along the x-z plane	
			var forward = cameraTransform.TransformDirection (Vector3.forward);
			forward.y = 0;
			forward = forward.normalized;
			
			// Right vector relative to the camera
			// Always orthogonal to the forward vector
			Vector3 right = new Vector3 (forward.z, 0, -forward.x);

			//Only allow movement input in cases where the camera is ready, since movement is relative to the camera
			//and the current player location cannot change if the camera is lerping to its former position
			if (getUserInput && !CameraButtonScript.isPanning) {
				verticalInput = Input.GetAxisRaw ("Vertical");
				horizontalInput = Input.GetAxisRaw ("Horizontal");
			} else {
				verticalInput = 0;
				horizontalInput = 0;
			}

			// Are we moving backwards or looking backwards
			if (verticalInput < -0.2f) {
				if (!movingBack) {
					//This effectively locks the camera so the player will not repeatedly turn 180 degrees when back is hit.
					gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y + 180, gameObject.transform.rotation.eulerAngles.z);
					Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y + 180, Camera.main.transform.rotation.eulerAngles.z);
					Camera.main.transform.position = Camera.main.transform.position + 4 * gameObject.transform.forward;
				}
				movingBack = true;
			} else {
				if (movingBack) {
					//Unlock the camera to the proper facing and location
					verticalInput = 0;
					horizontalInput = 0;
					gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y + 180, gameObject.transform.rotation.eulerAngles.z);
					Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z);
					Camera.main.transform.position = Camera.main.transform.position - 4 * gameObject.transform.forward;
					moveDirection = gameObject.transform.forward.normalized;
				}
				movingBack = false;
			}
			
			var wasMoving = isMoving;
			isMoving = Mathf.Abs (horizontalInput) > 0.1f || Mathf.Abs (verticalInput) > 0.1f;
			
			// Target direction relative to the camera
			var targetDirection = horizontalInput * right + verticalInput * forward;
			
			// Grounded controls
			if (grounded) {
				// Lock camera for short period when transitioning moving & standing still
				lockCameraTimer += Time.deltaTime;
				if (isMoving != wasMoving)
					lockCameraTimer = 0.0f;
				
				// We store speed and direction seperately,
				// so that when the character stands still we still have a valid forward direction
				// moveDirection is always normalized, and we only update it if there is user input.
				if (targetDirection != Vector3.zero) {
					// If we are really slow and moving backwards, just snap to the target direction
					if (moveSpeed < walkSpeed * 0.8f && grounded && movingBack) {
						moveDirection = targetDirection.normalized;
					}
					// Otherwise smoothly turn towards it
					else {
						var effRotateSpeed = rotateSpeed;
						moveDirection = Vector3.RotateTowards (moveDirection, targetDirection, effRotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
						
						moveDirection = moveDirection.normalized;
					}
				}
				
				// Smooth the speed based on the current target direction
				var curSmooth = speedSmoothing * Time.deltaTime;
				
				// Choose target speed
				//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
				var targetSpeed = Mathf.Min (targetDirection.magnitude, 1.0f);
				
				// Pick speed modifier
				if (Input.GetButton ("Fire3")) {
					targetSpeed *= runSpeed;
				} else if (Time.time - trotAfterSeconds > walkTimeStart) {
					targetSpeed *= trotSpeed;
				} else {
					targetSpeed *= walkSpeed;
				}
				
				moveSpeed = Mathf.Lerp (moveSpeed, targetSpeed, curSmooth);
				if (!isMoving) {
					moveSpeed = 0.0f;
					setCharState(CharacterState.IDLE);
				}
				
				// Reset walk time start when we slow down
				else if (moveSpeed < walkSpeed * 0.3f) {
					walkTimeStart = Time.time;
					setCharState(CharacterState.WALK);
				} else {
					setCharState(CharacterState.RUN);
				}
			}
			// In air controls
			else {
				// Lock camera while in air
				if (jumping)
					lockCameraTimer = 0.0f;
				
				if (isMoving) {
					inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
				}
			}
		}
	}
	
	void ApplyWallJump ()
	{
		// We must actually jump against a wall for this to work
		if (!jumping)
			return;
		
		// Store when we first touched a wall during this jump
		if (collisionFlags == CollisionFlags.CollidedSides && touchWallJumpTime < 0)
		{
			touchWallJumpTime = Time.time;
		}
		
		// The user can trigger a wall jump by hitting the button shortly before or shortly after hitting the wall the first time.
		var mayJump = lastJumpButtonTime > touchWallJumpTime - wallJumpTimeout && lastJumpButtonTime < touchWallJumpTime + wallJumpTimeout;
		if (!mayJump)
			return;
		
		// Prevent jumping too fast after each other
		if (lastJumpTime + jumpRepeatTime > Time.time)
			return;
		
		wallJumpContactNormal.y = 0;	
		if (wallJumpContactNormal != Vector3.zero)
		{
			moveDirection = wallJumpContactNormal.normalized;
			// Wall jump gives us at least trotspeed
			moveSpeed = Mathf.Clamp(moveSpeed * 1.5f, trotSpeed, runSpeed);
		}
		else
		{
			moveSpeed = 0;
		}
		
		verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
		DidJump();
		SendMessage("DidWallJump", null, SendMessageOptions.DontRequireReceiver);
	}
	
	void ApplyJumping ()
	{
		// Prevent jumping too fast after each other
		if (lastJumpTime + jumpRepeatTime > Time.time)
			return;
		
		if (IsGrounded()) {
			// Jump
			// - Only when pressing the button down
			// - With a timeout so you can press the button slightly before landing		
			if (canJump && Time.time < lastJumpButtonTime + jumpTimeout) {
				setCharState(CharacterState.JUMP);
				verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
				DidJump();
			}
		}
	}
	
	
	void ApplyGravity ()
	{
		//Other players will send their new locations after being affected by gravity
		if (gameObject.tag == "LocalPlayer") {
			// Apply gravity
			if (getUserInput)
				jumpButton = Input.GetButton ("Jump");
		
			// * When falling down we use capeFlyGravity (only when holding down jump)
			var capeFly = canCapeFly && verticalSpeed <= 0.0f && jumpButton && jumping;
		
			// When we reach the apex of the jump we send out a message
			if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f) {
				jumpingReachedApex = true;
				SendMessage ("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
			}
		
			// * When jumping up we don't apply gravity for some time when the user is holding the jump button
			//   This gives more control over jump height by pressing the button longer
			var extraPowerJump = IsJumping () && verticalSpeed > 0.0f && jumpButton && transform.position.y < lastJumpStartHeight + extraJumpHeight;
		
			if (capeFly)			
				verticalSpeed -= capeFlyGravity * Time.deltaTime;
			else if (extraPowerJump)
				return;
			else if (IsGrounded ())
				verticalSpeed = -gravity * 0.2f;
			else
				verticalSpeed -= gravity * Time.deltaTime;
		}
	}
	
	float CalculateJumpVerticalSpeed (float targetJumpHeight)
	{
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * targetJumpHeight * gravity);
	}
	
	void DidJump ()
	{
		jumping = true;
		jumpingReachedApex = false;
		lastJumpTime = Time.time;
		lastJumpStartHeight = transform.position.y;
		touchWallJumpTime = -1;
		lastJumpButtonTime = -10;
	}
	
	void Update() {
		if (gameObject.tag == "LocalPlayer") {
			if (getUserInput) {
				if (Input.GetButtonDown ("Jump"))
					lastJumpButtonTime = Time.time;
			} else {
				if (jumpButton)
					lastJumpButtonTime = Time.time;
			}
		
		
			UpdateSmoothedMovementDirection ();
		
			// Apply gravity
			// - extra power jump modifies gravity
			// - capeFly mode modifies gravity
			ApplyGravity ();
		
			// Perform a wall jump logic
			// - Make sure we are jumping against wall etc.
			// - Then apply jump in the right direction)
			if (canWallJump)
				ApplyWallJump ();
		
			// Apply jumping logic
			ApplyJumping ();
		
			// Calculate actual motion
			Vector3 movement = moveDirection * moveSpeed + new Vector3 (0, verticalSpeed, 0) + inAirVelocity;
			movement *= Time.deltaTime;
		
			// Move the controller
			if (!CameraButtonScript.isPanning) {
				CharacterController controller = GetComponent<CharacterController> ();
				wallJumpContactNormal = Vector3.zero;
				collisionFlags = controller.Move (movement);
				model.transform.position = transform.position;
		
				Vector3 xzMove = movement;
				xzMove.y = 0;
		
				if (!movingBack) {
					// Set rotation to the move direction
					if (IsGrounded () && !jumping && moveDirection != Vector3.zero) {
						if (xzMove.magnitude > 0.001f) {
							transform.rotation = Quaternion.LookRotation (xzMove);
							model.transform.rotation = Quaternion.LookRotation (xzMove);

						}
					} else {
				
						if (xzMove.magnitude > 0.001f) {
							transform.rotation = Quaternion.LookRotation (xzMove);
							model.transform.rotation = Quaternion.LookRotation (xzMove);
						}
					}
			
					// We are in jump mode but just became grounded
					if (IsGrounded ()) {
						lastGroundedTime = Time.time;
				
						if (jumping) {
							moveDirection = transform.rotation.eulerAngles;
							//moveDirection = new Vector3(0.0f,0.0f,0.0f);
							jumping = false;
							SendMessage ("DidLand", SendMessageOptions.DontRequireReceiver);
						}
						inAirVelocity = Vector3.zero;
					}
				}
			}
		}
		//Ensure that the avatar's model correctly displays with the right facing and location
		model.transform.position = transform.position;
		model.transform.rotation = transform.rotation;
	}
	
	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (hit.moveDirection.y > 0.01f) 
			return;
		wallJumpContactNormal = hit.normal;
	}
	
	float GetSpeed () {
		return moveSpeed;
	}
	
	bool IsJumping () {
		return jumping;
	}
	
	bool IsGrounded () {
		return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
	}
	
	void SuperJump (float height)
	{
		verticalSpeed = CalculateJumpVerticalSpeed (height);
		collisionFlags = CollisionFlags.None;
		DidJump();
	}
	
	void SuperJump (float height, Vector3 jumpVelocity)
	{
		verticalSpeed = CalculateJumpVerticalSpeed (height);
		inAirVelocity = jumpVelocity;
		
		collisionFlags = CollisionFlags.None;
		DidJump();
	}
	
	
	Vector3 GetDirection () {
		return moveDirection;
	}
	
	bool IsMovingBackwards () {
		return movingBack;
	}
	
	float GetLockCameraTimer () 
	{
		return lockCameraTimer;
	}
	
	float GetLean ()
	{
		return lean;
	}
	
	bool HasJumpReachedApex ()
	{
		return jumpingReachedApex;
	}
	
	bool IsGroundedWithTimeout ()
	{
		return lastGroundedTime + groundedTimeout > Time.time;
	}
	
	bool IsCapeFlying ()
	{
		// * When falling down we use capeFlyGravity (only when holding down jump)
		if (getUserInput)
			jumpButton = Input.GetButton("Jump");
		return canCapeFly && verticalSpeed <= 0.0f && jumpButton && jumping;
	}
	
	void Reset ()
	{
		gameObject.tag = "Player";
	}
}
