using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/**
 * This class handles the quit button in the player's heads-up display
 *
 * Author: Benjamin Niedzielski (bniedzie@ucla.edu)
 * Last modified: 9/3/15
 */
public class QuitScript : MonoBehaviour {

	/**
	 * This method handles the case where the quit button is clicked,
	 * updating the UI and resetting any variables that are valid only
	 * for this instance of the player.
	 */
	public void onClick() {
		EventSystem.current.SetSelectedGameObject (null);
		
		//Reset the camera position relative to the player
		GameObject localPlayer = GameObject.FindGameObjectWithTag ("LocalPlayer");
		Camera.main.transform.position = localPlayer.transform.position - 2.0f * localPlayer.transform.forward + new Vector3(0.0f, 0.0f, 2.0f);
		Camera.main.transform.rotation = Quaternion.Euler(localPlayer.transform.rotation.eulerAngles + new Vector3 (15.0f, 0.0f, 0.0f));
		
		//Hide all UI elements
		GameObject.FindGameObjectWithTag("CameraButton").GetComponent<Button>().interactable = false;
		GameObject.FindGameObjectWithTag("CameraText").GetComponent<Text>().text = "";
		GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>().interactable = false;
		GameObject.FindGameObjectWithTag("QuitText").GetComponent<Text>().text = "";
		GameObject.FindGameObjectWithTag ("PanningPanel").GetComponent<Image> ().color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
		GameObject.FindGameObjectWithTag ("PanningText").GetComponent<Text> ().text = "";
		
		//Reset the camera panning variables so a rejoining player does not start in this mode
		CameraButtonScript.isPanning = false;
		CameraPanningScript.zoomMult = 1.0f;
		CameraPanningScript.lastMousePosition = new Vector2 (-1.0f, -1.0f);
		CameraPanningScript.lastCameraRotation = Camera.main.transform.rotation.eulerAngles;
		
		//Ensure the player is destroyed so we do not have 2 instances
		NetworkManager.Instance.LeaveRoom();
		Destroy(localPlayer);
	}
}
