using UnityEngine;
using UnityEngine.UI;
using Drupal;
using LitJson;

public class DrupalUI : MonoBehaviour {

    public Text results;
    DrupalManager drupal;

    void Start() {
        drupal = FindObjectOfType<DrupalManager>();
        DrupalManager.OnAddedPlacard += DrupalManager_OnAddedPlacard;
        DrupalManager.OnGotCurrentEnvironment += DrupalManager_OnGotCurrentEnvironment;
        DrupalManager.OnGotCurrentPlacardId += DrupalManager_OnGotCurrentPlacardId;
        DrupalManager.OnGotCurrentTourId += DrupalManager_OnGotCurrentTourId;
        DrupalManager.OnGotEnvironment += DrupalManager_OnGotEnvironment;
        DrupalManager.OnGotInWorldObjects += DrupalManager_OnGotInWorldObjects;
        DrupalManager.OnGotPlacards += DrupalManager_OnGotPlacards;
        DrupalManager.OnGotTour += DrupalManager_OnGotTour;
        DrupalManager.OnRegisteredPlacardClick += DrupalManager_OnRegisteredPlacardClick;
    }

    private void DrupalManager_OnAddedPlacard(bool added) {
        results.text += added.ToString() + "\n\n";
    }

    private void DrupalManager_OnGotCurrentEnvironment(Environment currentEnvironment) {
        results.text += JsonMapper.ToJson(currentEnvironment) + "\n\n";
    }

    private void DrupalManager_OnGotCurrentPlacardId(int placardId) {
        results.text += placardId.ToString() + "\n\n";
    }

    private void DrupalManager_OnGotCurrentTourId(int currentTourId) {
        results.text += currentTourId.ToString() + "\n\n";
    }

    private void DrupalManager_OnGotEnvironment(Environment environment) {
        results.text += JsonMapper.ToJson(environment) + "\n\n";
    }

    private void DrupalManager_OnGotInWorldObjects(Drupal.Placard[] inWorldObjects) {
        results.text += JsonMapper.ToJson(inWorldObjects) + "\n\n";
    }

    private void DrupalManager_OnGotPlacards(Drupal.Placard[] placards) {
        results.text += JsonMapper.ToJson(placards) + "\n\n";
    }

    private void DrupalManager_OnGotTour(Tour tour) {
        results.text += JsonMapper.ToJson(tour) + "\n\n";
    }

    private void DrupalManager_OnRegisteredPlacardClick(bool registered) {
        results.text += registered.ToString() + "\n\n";
    }

    public void AddPlacard() {
        Drupal.Placard newPlacard = new Drupal.Placard();
        newPlacard.id = 0;
        newPlacard.location = new Location();
        newPlacard.location.latitude = 0.0;
        newPlacard.location.longitude = 0.0;
        newPlacard.location.orientation = 0.0;
        newPlacard.title = "New Placard";
        newPlacard.description = "Description";
        drupal.AddPlacard(newPlacard);
    }

    void OnDestroy() {
        DrupalManager.OnAddedPlacard -= DrupalManager_OnAddedPlacard;
        DrupalManager.OnGotCurrentEnvironment -= DrupalManager_OnGotCurrentEnvironment;
        DrupalManager.OnGotCurrentPlacardId -= DrupalManager_OnGotCurrentPlacardId;
        DrupalManager.OnGotCurrentTourId -= DrupalManager_OnGotCurrentTourId;
        DrupalManager.OnGotEnvironment -= DrupalManager_OnGotEnvironment;
        DrupalManager.OnGotInWorldObjects -= DrupalManager_OnGotInWorldObjects;
        DrupalManager.OnGotPlacards -= DrupalManager_OnGotPlacards;
        DrupalManager.OnGotTour -= DrupalManager_OnGotTour;
        DrupalManager.OnRegisteredPlacardClick -= DrupalManager_OnRegisteredPlacardClick;
    }
    	
}
