// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;

[System.Serializable]
public class FirstPersonMode : IPlayerCameraMode {

    PlayerCamera pC;

    public Transform transform;
    public LayerMask mask;
    public float cameraTransitionSpeed;

    bool isMouseDragging;
    Vector2 mouseDragDelta;
    Quaternion camLookRotation;
    Quaternion lookRotation;

    public void SetPlayerCamera(PlayerCamera pCam) {
        pC = pCam;
    }

    public void ToBirdsEyeMode() {
        pC.mode = pC.birdsEyeMode;
    }

    public void ToFirstPersonMode() {
        Debug.LogWarning("Can't transition to the same camera mode!");
    }

    public void ToThirdPersonMode() {
        pC.mode = pC.thirdPersonMode;
    }

    public void Update() {
        CheckMouseWheel();
        CheckArrowKeys();
        Look();
        UpdateCamera();
    }

    void CheckMouseWheel() {
        if (Input.mouseScrollDelta.y < 0) {
            ToThirdPersonMode();
        }
    }

    void CheckArrowKeys() {
        if (Input.GetKeyDown(KeyCode.End)) {
            ToThirdPersonMode();
        }
    }

    void Look() {
        mouseDragDelta.x += Input.GetAxis("Mouse X");
        mouseDragDelta.y -= Input.GetAxis("Mouse Y");
    }

    void UpdateCamera() {
        camLookRotation = Quaternion.Euler(mouseDragDelta.y * 2f, 0f, 0f);
        lookRotation = Quaternion.Euler(0f, mouseDragDelta.x * 2f, 0f);
        camLookRotation = Quaternion.Euler(ClampAngle(camLookRotation.eulerAngles.x, 300f, 70f), 0f, 0f);
        pC.transform.position = Vector3.Lerp(pC.transform.position, transform.position, Time.deltaTime * cameraTransitionSpeed);
        pC.transform.localRotation = Quaternion.Lerp(pC.transform.localRotation, camLookRotation, Time.deltaTime * cameraTransitionSpeed);
        pC.transform.parent.rotation = Quaternion.Lerp(pC.transform.parent.rotation, lookRotation, Time.deltaTime * cameraTransitionSpeed);
        pC.cam.cullingMask = mask;
    }

    float ClampAngle(float angle, float min, float max) {

        if(angle < 90 || angle > 270) { 
            if(angle > 180) angle -= 360; 
            if(max > 180) max -= 360;
            if(min > 180) min -= 360;
        }
        angle = Mathf.Clamp(angle, min, max);
        if(angle < 0) angle += 360;
        return angle;
    }

}
