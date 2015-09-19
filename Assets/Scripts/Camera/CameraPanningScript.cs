using UnityEngine;
using System.Collections;

/**
 * This class provides behavior to allow the camera to zoom and rotate if the panning button has been clicked.
 *
 * Written by Benjamin Niedzielski (bniedzie@ucla.edu)
 * Last modified: 8/28/15
 */
public class CameraPanningScript : MonoBehaviour {
	//Let the camera know where to return to when done
	private Vector3 defaultPosition;
	private Vector3 defaultRotation;
	
	//Allows the program to know in what direction the mouse was moved.
	//(-1, -1) is the default when the mouse is not being clicked 
	public static Vector2 lastMousePosition = new Vector2(-1.0f, -1.0f);
	
	public static float zoomMult = 1.0f;
	public float maxMult = 3.5f, minMult = -10.0f;
	public static Vector3 lastCameraRotation;

	void Start () {
		defaultPosition = transform.position;
		defaultRotation = transform.rotation.eulerAngles;
	}
	
	void Update () {
		if (!CameraButtonScript.isPanning) {
			//This distinction allows the program to know if it should reset the camera because panning is done
			//or set the new "default" camera position to the new location
			if (!CameraButtonScript.wasPanning) {
				defaultPosition = transform.position;
				defaultRotation = transform.rotation.eulerAngles;
			} else {
				transform.position = defaultPosition;
				transform.rotation = Quaternion.Euler(defaultRotation);
				CameraButtonScript.wasPanning = false;
			}
			//Reset all modifiers to the panning camera so it starts anew next time
			zoomMult = 1.0f;
			lastMousePosition = new Vector2(-1.0f, -1.0f);
			lastCameraRotation = defaultRotation;
		} else {
			if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
			{
				zoomMult += 5.0f * Time.deltaTime;
				if (zoomMult > maxMult) {
					zoomMult = maxMult;
				}
				if (zoomMult >= 1.65f && zoomMult <= 2.45f) { // inside the model's head
					zoomMult = 2.45f;
				}
			}
			if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
			{
				zoomMult -= 5.0f * Time.deltaTime;
				if (zoomMult < minMult) {
					zoomMult = minMult;
				}
				if (zoomMult >= 1.65f && zoomMult <= 2.45f) { // inside the model's head
					zoomMult = 1.65f;
				}
			}
			Vector2 diff = new Vector2(0.0f, 0.0f);
			if (Input.GetMouseButton(0)) { //left click
				Vector2 curMousePositon = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				if (lastMousePosition == new Vector2(-1.0f, -1.0f)) { //mouse is newly clicked
					lastMousePosition = curMousePositon;
				} else { //mouse is being held down
					diff = curMousePositon - lastMousePosition;
					lastMousePosition = curMousePositon;
				}
			} else {
				lastMousePosition = new Vector2(-1.0f, -1.0f);
			}
			//Update the camera based on any new information
			Camera.main.transform.position = defaultPosition + 2.0f * (zoomMult - 1.0f) * GameObject.FindGameObjectWithTag("LocalPlayer").transform.forward - new Vector3 (0, 0.4f*(zoomMult - 1.0f), 0);
			float cameraVertRot = lastCameraRotation.x - 5000.0f * (diff.y / Screen.height) * Time.deltaTime;
			//limit vertical rotation
			if (cameraVertRot > 90.0f) {
				cameraVertRot = 90.0f;
			}
			if (cameraVertRot < -90.0f) {
				cameraVertRot = -90.0f;
			}
			float cameraHorizRot = lastCameraRotation.y + 5000.0f * (diff.x / Screen.width) * Time.deltaTime;
			lastCameraRotation = new Vector3(cameraVertRot, cameraHorizRot, defaultRotation.z);
			Camera.main.transform.rotation = Quaternion.Euler(lastCameraRotation);
		}
	}
}
