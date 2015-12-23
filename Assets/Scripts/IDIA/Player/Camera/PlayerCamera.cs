// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    #region Fields
    /// <summary>
    /// The first person mode.
    /// </summary>
    public FirstPersonMode firstPersonMode;
    /// <summary>
    /// The third person mode.
    /// </summary>
    public ThirdPersonMode thirdPersonMode;
    /// <summary>
    /// The birds eye mode.
    /// </summary>
    public BirdsEyeMode birdsEyeMode;
    /// <summary>
    /// The camera mode.
    /// </summary>
    [HideInInspector]
    public IPlayerCameraMode mode;
    /// <summary>
    /// The camera.
    /// </summary>
    [HideInInspector]
    public Camera cam;
    /// <summary>
    /// The key to decrease field of view.
    /// </summary>
    public KeyCode fovMinus = KeyCode.Plus;
    /// <summary>
    /// The key to increase field of view.
    /// </summary>
    public KeyCode fovPlus = KeyCode.Minus;
    /// <summary>
    /// The key to look up.
    /// </summary>
    public KeyCode lookUp = KeyCode.PageUp;
    /// <summary>
    /// The key to look down.
    /// </summary>
    public KeyCode lookDown = KeyCode.PageDown;
    /// <summary>
    /// The angle to look up or down.
    /// </summary>
    public float lookStep = 30f;
    /// <summary>
    /// The current look offset.
    /// </summary>
    float offset = 0f;
    #endregion

    #region Unity Messages
    /// <summary>
	/// A message called when the script instance is being loaded.
	/// </summary>
    void Awake() {
        firstPersonMode.SetPlayerCamera(this);
        thirdPersonMode.SetPlayerCamera(this);
        birdsEyeMode.SetPlayerCamera(this);
        cam = GetComponent<Camera>();
    }
    /// <summary>
    /// A message called when the script starts.
    /// </summary>
    void Start() {
        mode = thirdPersonMode;
        cam = GetComponent<Camera>();
    }
    /// <summary>
    /// A message called when the script updates.
    /// </summary>
	void Update() {
        mode.Update();
        ChangeFOV();
        ChangeLook();
    }
    /// <summary>
    /// A message called after the script updates.
    /// </summary>
    void LateUpdate() {
        cam.transform.localEulerAngles = new Vector3(offset, cam.transform.localEulerAngles.y,cam.transform.localEulerAngles.z);
    }
    #endregion

    #region Methods
    /// <summary>
    /// A method to change the field of view.
    /// </summary>
    void ChangeFOV() {
        if(Input.GetKey(fovMinus)) {
            cam.fieldOfView -= .1f;
        }
        if(Input.GetKey(fovPlus)) {
            cam.fieldOfView += .1f;
        }
    }
    /// <summary>
    /// A method to change the look angle.
    /// </summary>
    void ChangeLook() {
        if(Input.GetKeyDown(lookUp)) {
            Debug.Log("lookup");
            offset = Mathf.Clamp(offset-lookStep, -lookStep, lookStep);
        }
        if(Input.GetKeyDown(lookDown)) {
            Debug.Log("look down");
            offset = Mathf.Clamp(offset + lookStep, -lookStep, lookStep);
        }
    }
    #endregion

}
