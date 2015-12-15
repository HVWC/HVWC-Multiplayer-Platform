// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;
using DrupalUnity;
using UnityEngine.Events;

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
