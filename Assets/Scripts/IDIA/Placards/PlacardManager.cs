using UnityEngine;
using System.Collections;
using Drupal;
using SimpleJSON;

public class PlacardManager : MonoBehaviour {

    public GameObject placardPrefab;
    public DrupalPlacard[] placards;

    DrupalManager drupal;
    Canvas canvas;

    void OnEnable() {
        DrupalManager.OnGotTour += OnGotTour;
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

    void OnGotTour(string json) {
        placards = drupal.tour.placards;
        SetPlacardLocations();
    }

    void SetPlacardLocations() {
        foreach(DrupalPlacard placard in placards) {
            GameObject newPlacard = (GameObject)Instantiate(placardPrefab, Vector3.zero, Quaternion.identity);
            GeographicManager.Instance.SetObjectCoordinates(newPlacard.transform,placard.latitude + ", " + placard.longitude + ", " + placard.elevation);
            newPlacard.GetComponent<RectTransform>().SetParent(transform,false);
            newPlacard.transform.rotation.eulerAngles.Set(0f,placard.orientation,0f); ;
            newPlacard.GetComponent<Placard>().placard = placard;
        }
    }

    void OnDisable() {
        DrupalManager.OnGotTour -= OnGotTour;
    }

}
