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

public class DrupalManager : MonoBehaviour {

    DrupalUnityIO drupalUnityIO;

    #region Class Fields
    public Environment currentEnvironment;
    public Tour currentTour;
    #endregion

    void OnEnable() {
        DrupalUnityIO.OnGotCurrentEnvironment += OnGotCurrentEnvironment;
        DrupalUnityIO.OnGotTour += OnGotTour;
    }

    void Awake() {
        drupalUnityIO = FindObjectOfType<DrupalUnityIO>();
    }

    void Start () {
        drupalUnityIO.GetCurrentEnvironment();
	}

    void OnGotCurrentEnvironment(Environment currentEnvironment) {
        drupalUnityIO.GetTour(currentEnvironment.tours[0].id);
    }

    void OnGotTour(Tour tour) {
        currentTour = tour;
    }

    void OnDisable() {
        DrupalUnityIO.OnGotCurrentEnvironment -= OnGotCurrentEnvironment;
        DrupalUnityIO.OnGotTour -= OnGotTour;
    }

}
