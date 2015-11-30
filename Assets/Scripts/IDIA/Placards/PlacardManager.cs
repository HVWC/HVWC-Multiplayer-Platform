using UnityEngine;
using UnityEngine.UI;
using DrupalUnity;

public class PlacardManager : MonoBehaviour {

    public GameObject placardPrefab;
    public GameObject placardWindow;
    public Text placardTitleText,placardDescriptionText;
    public Button placardTeleportButton;
    public bool teleportImmediately;

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
        foreach(Placard p in placards) {
            Placard placard = p; //capture the iterator; otherwise you always get last placard
            GameObject newPlacard = (GameObject)Instantiate(placardPrefab, Vector3.zero, Quaternion.identity);
            GeographicManager.Instance.SetObjectCoordinates(newPlacard.transform, placard.location.latitude + ", " + placard.location.longitude + ", " + placard.location.elevation);
            newPlacard.GetComponent<RectTransform>().SetParent(transform, false);
            newPlacard.transform.rotation.eulerAngles.Set(0f, (float)placard.location.orientation,0f); ;
            newPlacard.GetComponent<PlacardObject>().placard = placard;
            newPlacard.GetComponent<Button>().onClick.AddListener(() => drupalUnityIO.SelectPlacard(placard));
        }
    }

    void OnPlacardSelected(Placard placard) {
        if(placard.location!=null && teleportImmediately) {
            TeleportPlayerToPlacardLocation(placard.location);
        }
        OpenPlacardInfoWindow(placard);
    }

    public void TeleportPlayerToPlacardLocation(Location location) {
        Transform localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").transform;
        GeographicManager.Instance.SetObjectCoordinates(localPlayer, location.latitude + ", " + location.longitude + ", " + location.elevation);
        localPlayer.rotation = Quaternion.Euler(0f, (float)location.orientation, 0f);
    }

    public void OpenPlacardInfoWindow(Placard placard) {
        placardWindow.SetActive(true);
        placardTitleText.text = placard.title;
        placardDescriptionText.text = placard.description;
        placardTeleportButton.interactable = placard.location != null;
        //placardTeleportButton.gameObject.SetActive(!teleportImmediately);
        placardTeleportButton.onClick.AddListener(() => TeleportPlayerToPlacardLocation(placard.location));
    }

    void OnDisable() {
        DrupalUnityIO.OnGotTour -= OnGotTour;
    }

}
