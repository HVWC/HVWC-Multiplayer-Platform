using System;
using UnityEngine;

[System.Serializable]
public class FirstPersonMode : IPlayerCameraMode {

    PlayerCamera pC;

    public Transform transform;
    public LayerMask mask;
    public float cameraTransitionSpeed;

    bool isMouseDragging;
    Vector2 mouseDragDelta;
    Quaternion startRotation;

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
        if (Input.GetKeyDown(KeyCode.PageDown)) {
            ToThirdPersonMode();
        }
    }

    void Look() {
        mouseDragDelta.x += Input.GetAxis("Mouse X");
        mouseDragDelta.y -= Input.GetAxis("Mouse Y");
    }

    void UpdateCamera() {
        Quaternion lookRotation = Quaternion.Euler(mouseDragDelta.y*5f, mouseDragDelta.x * 5f, 0f);
        pC.transform.position = Vector3.Lerp(pC.transform.position, transform.position, Time.deltaTime * cameraTransitionSpeed);
        pC.transform.localRotation = Quaternion.Lerp(pC.transform.localRotation,lookRotation,Time.deltaTime*cameraTransitionSpeed);
        pC.cam.cullingMask = mask;
    }

    
}
