using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public FirstPersonMode firstPersonMode;
    public ThirdPersonMode thirdPersonMode;
    public BirdsEyeMode birdsEyeMode;

    [HideInInspector]
    public IPlayerCameraMode mode;
    [HideInInspector]
    public Camera cam;

    void Awake() {
        firstPersonMode.SetPlayerCamera(this);
        thirdPersonMode.SetPlayerCamera(this);
        birdsEyeMode.SetPlayerCamera(this);
        cam = GetComponent<Camera>();
    }

    void Start() {
        mode = thirdPersonMode;
        cam = GetComponent<Camera>();
    }

	void Update() {

        mode.Update();
    }

}
