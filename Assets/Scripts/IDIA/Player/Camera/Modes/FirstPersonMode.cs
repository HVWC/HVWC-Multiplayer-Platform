using System;
using UnityEngine;

[System.Serializable]
public class FirstPersonMode : IPlayerCameraMode {

    PlayerCamera pC;

    public Transform transform;
    public LayerMask mask;
    public float cameraTransitionSpeed;

    bool isMouseDragging;
    Vector2 mouseDelta;
    Quaternion camLookRotation;
    Quaternion lookRotation;

    bool initialized = false;

    public void SetPlayerCamera(PlayerCamera pCam) {
        pC = pCam;
    }

    public void ToBirdsEyeMode() {
        pC.mode = pC.birdsEyeMode;
        initialized = false;
    }

    public void ToFirstPersonMode() {
        Debug.LogWarning("Can't transition to the same camera mode!");
    }

    public void ToThirdPersonMode() {
        pC.mode = pC.thirdPersonMode;
        initialized = false;
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
        mouseDelta.x += Input.GetAxis("Mouse X");
        mouseDelta.y -= Input.GetAxis("Mouse Y");
    }

    void UpdateCamera() {
        camLookRotation = Quaternion.Euler(mouseDelta.y*5f, 0f, 0f);
        lookRotation = Quaternion.Euler(0f, mouseDelta.x * 5f, 0f);
        pC.transform.position = Vector3.Lerp(pC.transform.position, transform.position, Time.deltaTime * cameraTransitionSpeed);
        pC.transform.localRotation = Quaternion.Lerp(pC.transform.localRotation,camLookRotation,Time.deltaTime*cameraTransitionSpeed);
        pC.transform.parent.localRotation = Quaternion.Lerp(pC.transform.parent.localRotation, lookRotation, Time.deltaTime * cameraTransitionSpeed);
        pC.cam.cullingMask = mask;
    }

}
