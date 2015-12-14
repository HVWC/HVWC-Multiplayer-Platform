using System;
using UnityEngine;

[System.Serializable]
public class ThirdPersonMode : IPlayerCameraMode {

    PlayerCamera pC;

    public Transform transform;
    public LayerMask mask;
    public float cameraTransitionSpeed;

    public void SetPlayerCamera(PlayerCamera pCam) {
        pC = pCam;
    }

    public void ToBirdsEyeMode() {
        pC.mode = pC.birdsEyeMode;
    }

    public void ToFirstPersonMode() {
        pC.mode = pC.firstPersonMode;
    }

    public void ToThirdPersonMode() {
        Debug.LogWarning("Can't transition to the same camera mode!");
    }

    public void Update() {
        CheckMouseWheel();
        CheckArrowKeys();
        UpdateCamera();
    }

    void CheckMouseWheel() {
        if (Input.mouseScrollDelta.y < 0) {
            ToBirdsEyeMode();
        } else if(Input.mouseScrollDelta.y > 0) {
            ToFirstPersonMode();
        }
    }

    void CheckArrowKeys() {
        if (Input.GetKeyDown(KeyCode.Home)) {
            ToFirstPersonMode();
        }
        if (Input.GetKeyDown(KeyCode.End)) {
            ToBirdsEyeMode();
        }
    }

    void UpdateCamera() {
        pC.transform.position = Vector3.Lerp(pC.transform.position, transform.position, Time.deltaTime * cameraTransitionSpeed);
        pC.transform.rotation = Quaternion.Lerp(pC.transform.rotation, transform.rotation, Time.deltaTime * cameraTransitionSpeed);
        pC.cam.cullingMask = mask;
    }
}