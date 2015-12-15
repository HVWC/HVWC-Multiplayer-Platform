// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DrupalUnity;
using UnityEngine.EventSystems;

[System.Serializable]
public class PlacardEvent : UnityEvent<Placard> { }

public class PlacardManager : MonoBehaviour {

    public GameObject placardPrefab;
    public Placard[] placards;
    public PlacardEvent OnPlacardSelected;

    DrupalUnityIO drupalUnityIO;
    Canvas canvas;

    void OnEnable() {
        DrupalUnityIO.OnGotTour += OnGotTour;
        DrupalUnityIO.OnPlacardSelected += OnPlacardWasSelected;
    }

    void Start() {
        drupalUnityIO = FindObjectOfType<DrupalUnityIO>();
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    void Update() {
        if (!canvas.worldCamera) {
            canvas.worldCamera = Camera.main;
        }
    }

    void OnGotTour(Tour tour) {
        placards = tour.placards;
        GeneratePlacards();
    }

    void GeneratePlacards() {
        int count = 0;
        foreach(Placard p in placards) {
            count++;
            Placard placard = p; //capture the iterator; otherwise you always get last placard
            GameObject newPlacard = (GameObject)Instantiate(placardPrefab, Vector3.zero, Quaternion.identity);
            newPlacard.transform.position = GeographicManager.Instance.GetPosition(placard.location.latitude, placard.location.longitude, placard.location.elevation);
            newPlacard.GetComponent<RectTransform>().SetParent(transform, false);
            newPlacard.transform.rotation.eulerAngles.Set(0f, (float)placard.location.orientation,0f); ;
            newPlacard.GetComponent<PlacardObject>().placard = placard;
            newPlacard.GetComponent<Text>().text = "#"+count;
            EventTrigger trigger = newPlacard.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((e) => { drupalUnityIO.SelectPlacard(placard); });
            trigger.triggers.Add(entry);
        }
    }

    void OnPlacardWasSelected(Placard placard) {
        if(OnPlacardSelected != null) {
            OnPlacardSelected.Invoke(placard);
        }
    }

    void OnDisable() {
        DrupalUnityIO.OnGotTour -= OnGotTour;
    }

}
