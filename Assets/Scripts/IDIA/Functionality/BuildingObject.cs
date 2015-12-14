using UnityEngine;
using DrupalUnity;
using UnityEngine.Events;
using System;

public class BuildingObject : MonoBehaviour {

    public int[] validPlacardIDs;
    public int[] validTourIDs;
    public UnityEvent OnValidTour,OnInvalidTour, OnValidPlacard, OnInvalidPlacard;

    void OnEnable() {
        DrupalUnityIO.OnPlacardSelected += OnPlacardSelected;
        DrupalUnityIO.OnGotTour += OnGotTour;
    }

    private void OnGotTour(Tour tour) {
        if(IsValidTourID(tour.id)) {
            OnValidTour.Invoke();
        } else {
            OnInvalidTour.Invoke();
        }
    }

    void OnPlacardSelected(Placard placard) {
        if (IsValidPlacardID(placard.id)) {
            OnValidPlacard.Invoke();
        } else {
            OnInvalidPlacard.Invoke();
        }
    }

    bool IsValidPlacardID(int id) {
        return validPlacardIDs.Contains(id);
    }

    bool IsValidTourID(int id) {
        return validTourIDs.Contains(id);
    }

    void OnDisable() {
        DrupalUnityIO.OnPlacardSelected -= OnPlacardSelected;
        DrupalUnityIO.OnGotTour -= OnGotTour;
    }
}
