using UnityEngine;
using UnityEngine.UI;
using DrupalUnity;
using LitJson;

public class DrupalUI : MonoBehaviour {

    public Text results;
    DrupalUnityIO drupalIO;

    void Start() {
        drupalIO = FindObjectOfType<DrupalUnityIO>();
        DrupalUnityIO.OnAddedPlacard += DrupalManager_OnAddedPlacard;
        DrupalUnityIO.OnGotCurrentEnvironment += DrupalManager_OnGotCurrentEnvironment;
        DrupalUnityIO.OnGotCurrentPlacardId += DrupalManager_OnGotCurrentPlacardId;
        DrupalUnityIO.OnGotCurrentTourId += DrupalManager_OnGotCurrentTourId;
        DrupalUnityIO.OnGotEnvironment += DrupalManager_OnGotEnvironment;
        DrupalUnityIO.OnGotPlacards += DrupalManager_OnGotPlacards;
        DrupalUnityIO.OnGotTour += DrupalManager_OnGotTour;
    }

    private void DrupalManager_OnAddedPlacard(Status added) {
        results.text += added.success + "\n\n";
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

    private void DrupalManager_OnGotPlacards(DrupalUnity.Placard[] placards) {
        results.text += JsonMapper.ToJson(placards) + "\n\n";
    }

    private void DrupalManager_OnGotTour(Tour tour) {
        results.text += JsonMapper.ToJson(tour) + "\n\n";
    }

    public void AddPlacard() {
        DrupalUnity.Placard newPlacard = new DrupalUnity.Placard();
        newPlacard.id = 0;
        newPlacard.location = new Location();
        newPlacard.location.latitude = 0.0;
        newPlacard.location.longitude = 0.0;
        newPlacard.location.orientation = 0.0;
        newPlacard.title = "New Placard";
        newPlacard.description = "Description";
        drupalIO.AddPlacard(newPlacard);
    }

    void OnDestroy() {
        DrupalUnityIO.OnAddedPlacard -= DrupalManager_OnAddedPlacard;
        DrupalUnityIO.OnGotCurrentEnvironment -= DrupalManager_OnGotCurrentEnvironment;
        DrupalUnityIO.OnGotCurrentPlacardId -= DrupalManager_OnGotCurrentPlacardId;
        DrupalUnityIO.OnGotCurrentTourId -= DrupalManager_OnGotCurrentTourId;
        DrupalUnityIO.OnGotEnvironment -= DrupalManager_OnGotEnvironment;
        DrupalUnityIO.OnGotPlacards -= DrupalManager_OnGotPlacards;
        DrupalUnityIO.OnGotTour -= DrupalManager_OnGotTour;
    }
    	
}
