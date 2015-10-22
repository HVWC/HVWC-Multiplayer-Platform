using System;
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
        if (Input.GetKeyDown(KeyCode.PageUp)) {
            ToThirdPersonMode();
        }
    }

    void UpdateCamera() {
        pC.transform.position = Vector3.Lerp(pC.transform.position, transform.position, Time.deltaTime * cameraTransitionSpeed);
        pC.transform.rotation = Quaternion.Lerp(pC.transform.rotation, transform.rotation, Time.deltaTime * cameraTransitionSpeed);
        pC.cam.cullingMask = mask;
    }

    
}
