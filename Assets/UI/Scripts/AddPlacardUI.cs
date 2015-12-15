// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using DrupalUnity;

public class AddPlacardUI : MonoBehaviour {

    public InputField titleInput, descriptionInput, latitudeInput, longitudeInput, elevationInput, orientationInput;
    public Button submitButton;

    DrupalUnityIO drupalIO;

    Transform player;
    double latitude,longitude;

    void Start() {
        drupalIO = FindObjectOfType<DrupalUnityIO>();
    }

	// Update is called once per frame
	void Update () {
        submitButton.interactable = titleInput.text.Length > 0;
        if (!player) {
            player = GameObject.FindGameObjectWithTag("LocalPlayer").transform;
        } else {
            GetLatLong(player);
            latitudeInput.text = latitude.ToString();
            longitudeInput.text = longitude.ToString();
            elevationInput.text = player.transform.position.y.ToString();
            orientationInput.text = player.rotation.eulerAngles.y.ToString();
        }
	}

    void GetLatLong(Transform t) {
        GeographicCoord.MercatorSphericalToDecimalDegrees(out latitude, out longitude, t.position.z, t.position.x);
    }

    public void CreatePlacard() {
        Placard newPlacard = new Placard();
        newPlacard.title = titleInput.text;
        newPlacard.description = descriptionInput.text;
        newPlacard.location.latitude = float.Parse(latitudeInput.text);
        newPlacard.location.longitude = float.Parse(longitudeInput.text);
        newPlacard.location.elevation = float.Parse(elevationInput.text);
        newPlacard.location.orientation = float.Parse(orientationInput.text);
        drupalIO.AddPlacard(newPlacard);
    }

    public void ClearFields() {
        titleInput.text = "";
        descriptionInput.text = "";
    }

}
