using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public FirstPersonMode firstPersonMode;
    public ThirdPersonMode thirdPersonMode;
    public BirdsEyeMode birdsEyeMode;

    [HideInInspector]
    public IPlayerCameraMode mode;
    [HideInInspector]
    public Camera cam;

    public KeyCode fovMinus = KeyCode.Plus, fovPlus=KeyCode.Minus, lookUp = KeyCode.PageUp, lookDown = KeyCode.PageDown;
    public float lookStep = 30f;
    float offset = 0f;

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
        ChangeFOV();
        ChangeLook();
    }

    void LateUpdate() {
        cam.transform.localEulerAngles = new Vector3(offset, cam.transform.localEulerAngles.y,cam.transform.localEulerAngles.z);
    }

    void ChangeFOV() {
        if(Input.GetKey(fovMinus)) {
            cam.fieldOfView -= .1f;
        }
        if(Input.GetKey(fovPlus)) {
            cam.fieldOfView += .1f;
        }
    }

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

}
