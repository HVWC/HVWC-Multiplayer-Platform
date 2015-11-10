using UnityEngine;
using UnityEngine.UI;
using Drupal;
using System.Collections;

public class AddPlacardUI : MonoBehaviour {

    public InputField titleInput, latitudeInput, longitudeInput, elevationInput, orientationInput;
    public Button submitButton;

    DrupalManager drupal;

    Transform player;
    double latitude,longitude;

    void Start() {
        drupal = FindObjectOfType<DrupalManager>();
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
        DrupalPlacard newPlacard = new DrupalPlacard();
        newPlacard.title = titleInput.text;
        newPlacard.latitude = float.Parse(latitudeInput.text);
        newPlacard.longitude = float.Parse(longitudeInput.text);
        newPlacard.elevation = float.Parse(elevationInput.text);
        newPlacard.orientation = float.Parse(orientationInput.text);
        drupal.AddPlacard(newPlacard);
    }
}
