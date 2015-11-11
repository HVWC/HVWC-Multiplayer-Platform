using UnityEngine;
using Drupal;

public class PlacardManager : MonoBehaviour {

    public GameObject placardPrefab;
    public Placard[] placards;

    DrupalManager drupal;
    Canvas canvas;

    void OnEnable() {
        DrupalManager.OnGotTour += OnGotTour; ;
    }

    void Start() {
        drupal = FindObjectOfType<DrupalManager>();
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    void Update() {
        if (!canvas.worldCamera) {
            canvas.worldCamera = Camera.main;
        }
    }

    void OnGotTour(Tour tour) {
        placards = drupal.tour.placards;
        SetPlacardLocations();
    }

    void SetPlacardLocations() {
        foreach(Placard placard in placards) {
            GameObject newPlacard = (GameObject)Instantiate(placardPrefab, Vector3.zero, Quaternion.identity);
            GeographicManager.Instance.SetObjectCoordinates(newPlacard.transform, placard.location.latitude + ", " + placard.location.longitude + ", " + placard.location.elevation);
            newPlacard.GetComponent<RectTransform>().SetParent(transform, false);
            newPlacard.transform.rotation.eulerAngles.Set(0f, (float)placard.location.orientation,0f); ;
            newPlacard.GetComponent<PlacardObject>().placard = placard;
        }
    }

    void OnDisable() {
        DrupalManager.OnGotTour -= OnGotTour;
    }

}
