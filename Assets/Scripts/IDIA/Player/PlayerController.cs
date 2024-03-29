﻿// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This class handles the movement and animations of the local player.
/// </summary>
public class PlayerController : MonoBehaviour {

    #region Structs, Enums, and Classes
    /// <summary>
    /// This enumeration lists the different character states.
    /// </summary>
    public enum CharacterState {
        /// <summary>
        /// The IDLE character state.
        /// </summary>
        IDLE = 0,
        /// <summary>
        /// The WALK character state.
        /// </summary>
        WALK = 1,
        /// <summary>
        /// The RUN character state.
        /// </summary>
        RUN = 2,
        /// <summary>
        /// The JUMP character state.
        /// </summary>
        JUMP = 3,
        /// <summary>
        /// The SIT character state.
        /// </summary>
        SIT = 4,
        /// <summary>
        /// The FLY character state.
        /// </summary>
        FLY = 5
    }
    #endregion

    #region Fields
    /// <summary>
    /// The animation controller.
    /// </summary>
    public AnimationController animationController;
    /// <summary>
    /// The character controller.
    /// </summary>
    CharacterController character;
    /// <summary>
    /// The character animation state.
    /// </summary>
    CharacterState charState;
    /// <summary>
    /// The turn speed.
    /// </summary>
    public float turnSpeed = 90f;
    /// <summary>
    /// The walk speed.
    /// </summary>
    public float walkSpeed = 1.0f;
    /// <summary>
    /// The run speed.
    /// </summary>
    public float runSpeed = 1.5f;
    /// <summary>
    /// The fly speed.
    /// </summary>
    public float flySpeed = 2.0f;
    /// <summary>
    /// The jump speed.
    /// </summary>
    public float jumpSpeed = 8.0f;
    /// <summary>
    /// The current speed.
    /// </summary>
    float speed;
    /// <summary>
    /// The vertical speed.
    /// </summary>
    float vSpeed = 0f;
    /// <summary>
    /// The speed threshold.
    /// </summary>
    float speedThreshold = .1f;
    /// <summary>
    /// The gravity.
    /// </summary>
    float gravity = 9.8f;
    /// <summary>
    /// The scale.
    /// </summary>
    float scale = 3;
    /// <summary>
    /// Is the character flying?
    /// </summary>
    bool flying;
    /// <summary>
    /// Is the character running?
    /// </summary>
    bool running;
    /// <summary>
    /// Is the character jumping?
    /// </summary>
    bool jumping;
    #endregion

    #region Properties
    /// <summary>
    /// The character animation state property.
    /// </summary>
    public CharacterState CharState {
        get { return charState; }
        set {
            charState = value;
            animationController.ActivateAnimation((int)charState);
        }
    }
    #endregion

    #region Unity Messages
    /// <summary>
    /// A message called when this script starts.
    /// </summary>
    void Start() {
        character = GetComponent<CharacterController>();
    }
    /// <summary>
    /// A message called when this script updates.
    /// </summary>
    void Update() {
        if (EventSystem.current.currentSelectedGameObject) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            flying = !flying;
        }
        running = Input.GetKey(KeyCode.LeftShift);
        jumping = Input.GetButtonDown("Jump");
        
    }
    /// <summary>
    /// A message called every fixed framerate frame.
    /// </summary>
    void FixedUpdate() {
        if (EventSystem.current.currentSelectedGameObject) {
            return;
        }
        //Variables
        Vector3 movement;
        float rotation;
        float fly;

        //Movement
        speed = flying ? flySpeed : running ? runSpeed : walkSpeed;
        movement = Input.GetAxis("Vertical") * speed * transform.forward;

        //Rotation
        rotation = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        //Fly
        fly = flying ? Input.GetAxis("Fly") : 0;

        //Gravity
        vSpeed = flying ? fly : character.isGrounded && jumping ? jumpSpeed : -gravity * Time.deltaTime;
        movement.y = vSpeed;

        //Apply
        character.Move(movement * Time.deltaTime * scale);
        transform.Rotate(0f, rotation, 0f);

        //Animations
        if (character.isGrounded) { //If the local player is not grounded
            if (!Input.GetAxis("Vertical").AlmostEquals(0, .01f)) { //If the local player is moving
                if (System.Math.Abs(speed - walkSpeed) < speedThreshold) {
                    CharState = CharacterState.WALK; //If the local player's speed is close to the walking speed, then it is walking
                }
                if (System.Math.Abs(speed - runSpeed) < speedThreshold) {
                    CharState = CharacterState.RUN; //If the local player's speed is close to the running speed, then it is running
                }
            } else {
                CharState = CharacterState.IDLE; //If the local player is not moving, then it is idling
            }
        } else { //If the local player is not grounded
            if (flying) {
                CharState = CharacterState.FLY; //If the local player is flying, then it is flying
            } else {
                CharState = CharacterState.JUMP; //If the local player is not grounded and not flying, then it is jumping
            }
        }

    }
    #endregion

}
