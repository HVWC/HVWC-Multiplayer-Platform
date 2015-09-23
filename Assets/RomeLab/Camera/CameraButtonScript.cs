/**
 * This script handles the button on the left side of the screen that
 * activates and deactivates panning.
 *
 * Author: Benjamin Niedzielski (bniedzie@ucla.edu)
 * Last Modified: 9/3/15
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraButtonScript : MonoBehaviour {

	public Text cameraText; //The message on the button
	public static bool isPanning = false; 
	public static bool wasPanning = false;

	/**
	 * This method is called when the button is clicked and activates or deactivates panning mode.
	 */
	public void getClicked() {
		EventSystem.current.SetSelectedGameObject (null);
		if (!isPanning) {
			cameraText.text = "Disable Camera Panning";
			isPanning = true;
			wasPanning = true; //Needed elsewhere to ensure a smooth transition between modes
			//Display the message about how to move the camera
			GameObject.FindGameObjectWithTag("PanningText").GetComponent<Text>().text = "Use the mouse scroll button to pan in and out, or hold and move the mouse to rotate the camera in that direction.";
			GameObject.FindGameObjectWithTag("PanningPanel").GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 117.0f/255.0f);
		} else {
			cameraText.text = "Enable Camera Panning";
			isPanning = false;
			//Hide the message about how to move the camera
			GameObject.FindGameObjectWithTag("PanningText").GetComponent<Text>().text = "";
			GameObject.FindGameObjectWithTag("PanningPanel").GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		}
	}

}
