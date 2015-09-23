/* Copyright (c) 2009-11, ReactionGrid Inc. http://reactiongrid.com
 * See License.txt for full licence information. 
 * Converted to C# by Kim Ferguson 7/12/12
 * PlayerCamera.cs revision 1.4.2.1211.19
 * */


using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    #region Member Variables
    public LayerMask layerMask;

    float r; //Radius of the sphere around the target on which the camera moves
    float theta; //Theta angle used to calculate the y-z position of the camera
    float phi; //Phi angle used to calculate the x-z position of the camera
    private float thetaMax = Mathf.PI; //Max value that the theta angle can have
    private float thetaEpsilon = 0.1f; //A small value subtracted from clamping calculations to allow looking at the target from almost the top without spinning over
    private float thetaRange = Mathf.PI; //Total range of the theta angle, 0 to 180
    private float phiMax = Mathf.PI * 2; //Max vaue that the phi angle can have
    private float phiRange = Mathf.PI * 2; //Total range of the phi angle, 0 to 360

    float lerpDamp = 0.05f; //Damping of the camera movement
    private bool lockedToBack = true; //Whether the camera is locked to the back of the character
    private bool cameraEnabled = true;

    public float mouseHorizontalSpeed = 0.3f; //Horizontal rotation speed of the camera
    public float mouseVerticalSpeed = 0.2f; //Vertical rotation speed of the camera
    public float mousePivotRotationSpeed = 5.0f; //Rotation speed of the player movement pivot when using Right Mouse Button

    public bool invertVertical = true; //Inverts the vertical rotation of the camera
    public bool invertHorizontal = false; //Inverts the horizontal rotation of the camera

    public float cameraCollisionRadius = 0.75f; //Collision radius of the camera, used for preventing the camera clipping against surfaces that are very close
    private float camNoLerpAngle = 0.2f; //Angle below which the camera should lock to the back of the character instead of lerping

    private bool freeLook = false; //Whether the camera can freely rotate around the character even when there's movement
    private Vector3 startingLocalPosition; //Camera's starting position relative to the target

    private Vector3 desiredWorldPosition; //World position we want to be if we were to be exactly behind the target
    private Vector3 relativePosition; //Position of the camera relative to the target, in world space

    private Camera cam; //Reference to the Camera component of the gameObject
    private GUILayer guiLayer; //Reference to the GUILayer component of the gameObject
    private Transform t; //Reference to the Transform of the gameObject
    public Transform target; //The target the camera is following and looking at
    Transform defaultTarget;
    bool flyCamEnabled = false;
    Vector3 storedCamPos;
    Quaternion storedCamRot;
    Transform movementPivot; //The movement pivot used by the player character
    //private Cursor cursor; //Reference to the Cursor in the scene
    private bool isChatting = false;
    float distance = 1.0f;
    public float minZoomDistanceScale = 0.7f;
    public float maxZoomDistanceScale = 5.0f;

    GameObject guiParent;
    private bool isReady = false;
    private float initialDistance = 1.0f;
    #endregion Member Variables

    void SetupCamera()
    {
        //Grab references
        cam = GetComponent<Camera>();
        guiLayer = gameObject.GetComponent<GUILayer>();
        t = transform;
        initialDistance = t.localPosition.z * -1;
        //cursor = (Cursor)FindObjectOfType(typeof(Cursor));
        relativePosition = t.position - target.position;
        startingLocalPosition = target.InverseTransformPoint(t.position);
        //Calculate theta and phi from current relative position
        CalculateSphericalParametersFromPosition(relativePosition);
        isReady = true;
    }

    void Update()
    {
        if (isReady)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (distance > minZoomDistanceScale)
                {
                    distance = distance - 0.1f;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (distance < maxZoomDistanceScale)
                {
                    distance = distance + 0.1f;
                }
            }

            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(0))
            {
                Debug.Log("leftalt + click");
                Ray mouseRay = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(mouseRay, out hit, 50.0f))
                {
                    Debug.Log("hit " + hit.transform.name + ", tag: " + hit.transform.tag);
                    if (hit.transform.name.ToLower() != "terrain" && hit.transform.tag != "Player")
                    {
                        if (target == defaultTarget)
                        {
                            // Just started flycam
                            storedCamPos = t.position;
                            storedCamRot = t.rotation;
                        }
                        target = hit.transform;
                        SetupCamera();
                        SendMessageUpwards("SetFlyCamEnabled", true);
                        flyCamEnabled = true;
                    }
                }
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                if (flyCamEnabled)
                {
                    Debug.Log("Reset to default position");
                    target = defaultTarget;
                    t.position = storedCamPos;
                    t.rotation = storedCamRot;
                    SetupCamera();
                    //SnapToBack();
                    SendMessageUpwards("SetFlyCamEnabled", false);
                    flyCamEnabled = false;
                }
                else if (CameraEnabled())
                {
                    SnapToBack();
                }
            }
        }
    }

    void LateUpdate()
    {
        if (isReady)
        {
            DetermineCursorGUILock(); //Determine whether the cursor should show or be hidden and locked

            //Determine if freeLook should be enable and whether the camera should be locked to the back of the target
            if (Input.GetMouseButtonDown(0) && CameraEnabled())
            {
                freeLook = true;
                lockedToBack = false;
            }
            if (Input.GetMouseButtonUp(0))
            {
                freeLook = false;
            }

            if (Input.GetMouseButton(1) && CameraEnabled())
            { //Handle Right Mouse Button type movement of the camera
                lockedToBack = false;
                //Only update theta, for vertical rotation of the camera
                theta += ((invertVertical) ? 1 : -1) * Input.GetAxis("Mouse Y") * mouseVerticalSpeed;
                ClampThetaPhiRanges();
                //Rotate the movement pivot of the player directly using the horizontal movement of the mouse, so we're indirectly controlling the direction of the player
                movementPivot.Rotate(0, Input.GetAxis("Mouse X") * mousePivotRotationSpeed, 0);
                //this.transform.parent.Rotate(Vector3.up * (Input.GetAxis("Mouse X") * mousePivotRotationSpeed), Space.World); // pivot player directly
                SnapToBackOnlyHorizontal(); //Keep the horizontal rotation of the camera locked to the back of the character
            }
            else
            { //Right Mouse Button isn't held down
                if (lockedToBack)
                { //If we're locked to the back of the target, we should stay locked
                    SnapToBack();
                }
                else if (PlayerInputOn() && !freeLook && !flyCamEnabled && !isChatting)
                { //Otherwise, we should lerp to the back of the character if freeLook is false
                    LerpToBack();
                }
                else
                {
                    if (Input.GetMouseButton(0) && CameraEnabled())
                    { //Free look if Left Mouse Button is held down
                        //Update and clamp theta and phi
                        theta += ((invertVertical) ? 1 : -1) * Input.GetAxis("Mouse Y") * mouseVerticalSpeed;
                        phi += ((invertHorizontal) ? 1 : -1) * Input.GetAxis("Mouse X") * mouseHorizontalSpeed;
                        ClampThetaPhiRanges();
                    }
                    t.position = target.position + CalculatePositionFromSphericalParameters(); //Set new position calculated from r, theta, and phi
                }
            }
            t.LookAt(target); //Always look at target

            CheckLineOfSight(); //Make sure nothing is in the way of our line of sight to the target
        }
    }

    bool PlayerInputOn()
    {
        return (!Mathf.Approximately(0.0f, Input.GetAxis("Horizontal")) || !Mathf.Approximately(0.0f, Input.GetAxis("Vertical")));
    }

    bool CameraOnScreen()
    {
        if (Input.mousePosition.x < Screen.width && Input.mousePosition.x > 0 && Input.mousePosition.y < Screen.height && Input.mousePosition.y > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CameraEnabled()
    {
        if (CameraOnScreen() && cameraEnabled)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void SetCameraEnabled(bool enable)
    {
        cameraEnabled = enable;
    }

    void SetChatting(bool chatMode)
    {
        isChatting = chatMode;
    }

    //Preserve line of sight to the target by casting a ray from the target towards the camera
    //and seeing if it hits any colliders. If it does, pull the camera closer towards the target
    //until that collider is no longer between the camera and the target.
    void CheckLineOfSight()
    {
        Ray ray = new Ray(target.position, (t.position - target.position).normalized);
        RaycastHit hit = new RaycastHit();
        //        int layerMask1 = ~(1 << LayerMask.NameToLayer("PlayerSkin"));
        //        int layerMask2 = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
        //        int layerMask = layerMask1 & layerMask2;
        if (Physics.Raycast(ray, out hit, Vector3.Distance(t.position, target.position) + cameraCollisionRadius, layerMask))
        {
            Vector3 dir = (target.position - t.position).normalized; //Calculate direction in which we'll move the camera
            Vector3 newPos = hit.point + dir * cameraCollisionRadius; //New position is camCollisionRadius away from the ray hit point

            t.position = newPos; //Set new position
        }
    }

    void DetermineCursorGUILock()
    {
        ////Determine if the current mouse position is over any GUI elements
        //var hitGUIElement = guiLayer.HitTest(Input.mousePosition);
        //if (hitGUIElement != null)
        //{ //There is a GUI Element in the way
        //    cursor.SetEnabled(true); //Keep cursor enabled
        //    guiParent.SetActiveRecursively(true); //Keep GUI elements enabled
        //    return;
        //}
        //if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        //{ //If either LMB or RMB are held down, cursor and GUI elements should be hidden
        //    cursor.SetEnabled(false); //Disable cursor
        //    guiParent.SetActiveRecursively(false); //Disable GUI elements
        //    return;
        //}
        ////If we haven't returned until now, that means the mouse didn't hit any GUI Elements and LMB or RMB aren't held down
        //cursor.SetEnabled(true); //So enable cursor
        //guiParent.SetActiveRecursively(true); //And keep GUI elements enabled
    }

    //Calculate world position from r, theta, and phi
    Vector3 CalculatePositionFromSphericalParameters()
    {
        r = distance * initialDistance;
        return new Vector3(r * Mathf.Sin(theta) * Mathf.Cos(phi), r * Mathf.Cos(theta), r * Mathf.Sin(theta) * Mathf.Sin(phi));
    }

    //Calculate r, theta, and phi from relative position to the target
    void CalculateSphericalParametersFromPosition(Vector3 pos)
    {
        r = pos.magnitude;
        theta = Mathf.Acos(pos.y / r);
        phi = Mathf.Atan2(pos.z, pos.x);
        ClampThetaPhiRanges();
    }

    //Return theta and phi from relative position to the target
    Vector2 GetSphericalParametersFromPosition(Vector3 pos)
    {
        Vector2 parameters;
        parameters.x = Mathf.Acos(pos.y / r);
        parameters.y = Mathf.Atan2(pos.z, pos.x);
        //parameters = ClampThetaPhiRangesVector2(parameters);
        return parameters;
    }

    //Clamp theta and phi to their respective ranges
    void ClampThetaPhiRanges()
    {
        Vector2 parameters = new Vector2(theta, phi);
        parameters = ClampThetaPhiRangesVector2(parameters);
        theta = parameters.x;
        phi = parameters.y;
    }

    //Get clamped values for theta and phi
    Vector2 ClampThetaPhiRangesVector2(Vector2 parameters)
    {
        if (parameters.x < thetaEpsilon)
        {
            parameters.x = thetaEpsilon;
        }
        else if (parameters.x > thetaMax - thetaEpsilon)
        {
            parameters.x = thetaMax - thetaEpsilon;
        }

        if (parameters.y < 0)
        {
            parameters.y += phiMax;
        }
        else if (parameters.y > phiMax)
        {
            parameters.y -= phiMax;
        }

        return parameters;
    }

    //Lerp theta and phi to slowly move camera to the back of the target
    void LerpToBack()
    {
        desiredWorldPosition = target.TransformPoint(startingLocalPosition);
        relativePosition = desiredWorldPosition - target.position;
        Vector2 parameters = GetSphericalParametersFromPosition(relativePosition);
        float angle = Vector3.Angle(desiredWorldPosition, t.position);
        lockedToBack = (angle < camNoLerpAngle);

        //Find the closer direction to lerp to for both angles
        float endTheta = parameters.x;
        float thetaDiff = theta - parameters.x;
        if (Mathf.Abs(thetaDiff) > thetaRange / 2.0)
            endTheta = parameters.x + Mathf.Sign(thetaDiff) * thetaRange;
        theta = Mathf.Lerp(theta, endTheta, lerpDamp);

        float endPhi = parameters.y;
        float phiDiff = phi - parameters.y;
        if (Mathf.Abs(phiDiff) > phiRange / 2.0)
            endPhi = parameters.y + Mathf.Sign(phiDiff) * phiRange;
        phi = Mathf.Lerp(phi, endPhi, lerpDamp);

        t.position = target.position + CalculatePositionFromSphericalParameters();
    }

    //Snap camera to the back of the target
    void SnapToBack()
    {
        desiredWorldPosition = target.TransformPoint(startingLocalPosition);
        relativePosition = desiredWorldPosition - target.position;
        Vector2 parameters = GetSphericalParametersFromPosition(relativePosition);

        //theta = parameters.x;
        phi = parameters.y;

        t.position = target.position + CalculatePositionFromSphericalParameters();
    }

    //Snap camera to the back of the target, but only horizontally, meaning only manipulating phi
    void SnapToBackOnlyHorizontal()
    {
        desiredWorldPosition = target.TransformPoint(startingLocalPosition);
        relativePosition = desiredWorldPosition - target.position;
        Vector2 parameters = GetSphericalParametersFromPosition(relativePosition);

        phi = parameters.y;

        t.position = target.position + CalculatePositionFromSphericalParameters();
    }

    //Lerp only phi to slowly move camera to the back of the target
    void LerpToBackOnlyHorizontal()
    {
        desiredWorldPosition = target.TransformPoint(startingLocalPosition);
        relativePosition = desiredWorldPosition - target.position;
        Vector2 parameters = GetSphericalParametersFromPosition(relativePosition);

        //Find the closer direction to lerp
        float endPhi = parameters.y;
        float phiDiff = phi - parameters.y;
        if (Mathf.Abs(phiDiff) > phiRange / 2.0)
            endPhi = parameters.y + Mathf.Sign(phiDiff) * phiRange;
        phi = Mathf.Lerp(phi, endPhi, lerpDamp);
        //phi = endPhi;

        t.position = target.position + CalculatePositionFromSphericalParameters();
    }

    void SetGuiParent(GameObject newGuiParent)
    {
        if (newGuiParent != null)
        {
            guiParent = newGuiParent;
        }
    }

    void SetMovementPivot(Transform newPivot)
    {
        if (newPivot != null)
        {
            movementPivot = newPivot;
        }
    }

    void SetTarget(Transform newTarget)
    {
        // only used on initial setup from spawn controller
        if (newTarget != null)
        {
            target = newTarget;
            defaultTarget = newTarget;
        }
    }
}
