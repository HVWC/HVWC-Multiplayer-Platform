using UnityEngine;
using UnityEngine.UI;
using DrupalUnity;

public class PlacardManager : MonoBehaviour {

    public GameObject placardPrefab;
    public Placard[] placards;

    DrupalUnityIO drupalUnityIO;

    Canvas canvas;

    void OnEnable() {
        DrupalUnityIO.OnGotTour += OnGotTour;
        DrupalUnityIO.OnPlacardSelected += OnPlacardSelected;
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
        foreach(Placard placard in placards) {
            GameObject newPlacard = (GameObject)Instantiate(placardPrefab, Vector3.zero, Quaternion.identity);
            GeographicManager.Instance.SetObjectCoordinates(newPlacard.transform, placard.location.latitude + ", " + placard.location.longitude + ", " + placard.location.elevation);
            newPlacard.GetComponent<RectTransform>().SetParent(transform, false);
            newPlacard.transform.rotation.eulerAngles.Set(0f, (float)placard.location.orientation,0f); ;
            newPlacard.GetComponent<PlacardObject>().placard = placard;
            newPlacard.GetComponent<Button>().onClick.AddListener(() => drupalUnityIO.SelectPlacard(placard));
        }
    }

    void OnPlacardSelected(Placard placard) {
        GeographicManager.Instance.SetObjectCoordinates(GameObject.FindGameObjectWithTag("LocalPlayer").transform, placard.location.latitude + ", " + placard.location.longitude);
    }

    void OnDisable() {
        DrupalUnityIO.OnGotTour -= OnGotTour;
    }

}
