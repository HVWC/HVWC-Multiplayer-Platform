public interface IPlayerCameraMode {

    void Update();
    void ToFirstPersonMode();
    void ToThirdPersonMode();
    void ToBirdsEyeMode();
    void SetPlayerCamera(PlayerCamera pCam);

}
