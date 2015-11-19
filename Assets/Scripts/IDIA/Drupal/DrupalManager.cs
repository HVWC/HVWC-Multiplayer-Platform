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
