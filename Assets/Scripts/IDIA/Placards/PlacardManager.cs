using UnityEngine;
using System.Collections;
using Drupal;
using SimpleJSON;

public class PlacardManager : MonoBehaviour {

    public DrupalPlacard[] placards;

    DrupalManager drupal;

    void OnEnable() {
        DrupalManager.OnGotTour += OnGotTour; ;
    }

    void Start() {
        drupal = FindObjectOfType<DrupalManager>();
    }

    void OnGotTour(string json) {
        placards = drupal.tour.placards;
        SetPlacardLocations();
    }

    void SetPlacardLocations() {
        foreach(DrupalPlacard placard in placards) {
            GameObject newPlacard = new GameObject(placard.title);
            GeographicManager.Instance.SetObjectCoordinates(newPlacard.transform,placard.latitude + ", " + placard.longitude + ", " + placard.elevation);
            newPlacard.transform.parent = transform;
        }
    }

    void OnDisable() {
        DrupalManager.OnGotTour -= OnGotTour;
    }

}
