using UnityEngine;
using UnityEngine.UI;
using DrupalUnity;

public class AddPlacardUI : MonoBehaviour {

    public InputField titleInput, latitudeInput, longitudeInput, elevationInput, orientationInput;
    public Button submitButton;

    DrupalUnityIO drupal;

    Transform player;
    double latitude,longitude;

    void Start() {
        drupal = FindObjectOfType<DrupalUnityIO>();
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
        DrupalUnity.Placard newPlacard = new DrupalUnity.Placard();
        newPlacard.title = titleInput.text;
        newPlacard.location.latitude = float.Parse(latitudeInput.text);
        newPlacard.location.longitude = float.Parse(longitudeInput.text);
        newPlacard.location.elevation = float.Parse(elevationInput.text);
        newPlacard.location.orientation = float.Parse(orientationInput.text);
        drupal.AddPlacard(newPlacard);
    }
}
