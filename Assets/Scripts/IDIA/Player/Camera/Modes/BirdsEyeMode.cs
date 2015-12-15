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
public class BirdsEyeMode : IPlayerCameraMode {

    PlayerCamera pC;

    public Transform transform;
    public LayerMask mask;
    public float cameraTransitionSpeed;

    public void SetPlayerCamera(PlayerCamera pCam) {
        pC = pCam;
    }

    public void ToBirdsEyeMode() {
        Debug.LogWarning("Can't transition to the same camera mode!");
    }

    public void ToFirstPersonMode() {
        pC.mode = pC.firstPersonMode;
    }

    public void ToThirdPersonMode() {
        pC.mode = pC.thirdPersonMode;
    }

    public void Update() {
        CheckMouseWheel();
        CheckArrowKeys();
        UpdateCamera();
    }

    void CheckMouseWheel() {
        if (Input.mouseScrollDelta.y > 0) {
            ToThirdPersonMode();
        }
    }

    void CheckArrowKeys() {
        if (Input.GetKeyDown(KeyCode.Home)) {
            ToThirdPersonMode();
        }
    }

    void UpdateCamera() {
        pC.transform.position = Vector3.Lerp(pC.transform.position, transform.position, Time.deltaTime * cameraTransitionSpeed);
        pC.transform.rotation = Quaternion.Lerp(pC.transform.rotation, transform.rotation, Time.deltaTime * cameraTransitionSpeed);
        pC.cam.cullingMask = mask;
    }

}
