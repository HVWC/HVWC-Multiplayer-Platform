using UnityEngine;
using UnityEngine.UI;
using Drupal;

public class DrupalUI : MonoBehaviour {

    public Text results;
    DrupalManager drupal;

    void Start() {
        drupal = FindObjectOfType<DrupalManager>();
        DrupalManager.OnGotCurrentEnvironment += OnGotResult;
        DrupalManager.OnGotTour += OnGotResult;
        DrupalManager.OnGotInWorldObjects += OnGotResult;
        DrupalManager.OnRegisteredPlacardClick += OnGotResult;
        DrupalManager.OnAddedPlacard += OnGotResult;
    }

    public void AddPlacard() {
        DrupalPlacard newPlacard = new DrupalPlacard();
        newPlacard.id = 0;
        newPlacard.latitude = 0f;
        newPlacard.longitude = 0f;
        newPlacard.orientation = 0f;
        newPlacard.title = "New Placard";
        drupal.AddPlacard(newPlacard);
    }

    void OnGotResult(string result) {
        results.text += result + "\n";
    }

    void OnDestroy() {
        DrupalManager.OnGotCurrentEnvironment -= OnGotResult;
        DrupalManager.OnGotTour -= OnGotResult;
        DrupalManager.OnGotInWorldObjects -= OnGotResult;
        DrupalManager.OnRegisteredPlacardClick -= OnGotResult;
        DrupalManager.OnAddedPlacard -= OnGotResult;
    }
    	
}
