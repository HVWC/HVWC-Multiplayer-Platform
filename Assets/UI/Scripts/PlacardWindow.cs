using UnityEngine;
using UnityEngine.UI;
using DrupalUnity;

public class PlacardWindow : MonoBehaviour {

    public GameObject placardWindow;
    public Text placardTitleText, placardDescriptionText;
    public Button placardTeleportButton;

    public void OpenPlacardInfoWindow(Placard placard) {
        placardWindow.SetActive(true);
        placardTitleText.text = placard.title;
        placardDescriptionText.text = placard.description;
        placardTeleportButton.interactable = placard.location != null;
        placardTeleportButton.onClick.AddListener(() => TeleportPlayerToPlacardLocation(placard));
    }

    public void TeleportPlayerToPlacardLocation(Placard placard) {
        Transform localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").transform;
        localPlayer.position = GeographicManager.Instance.GetPosition(placard.location.latitude, placard.location.longitude, placard.location.elevation);
        localPlayer.rotation = Quaternion.Euler(0f, (float)placard.location.orientation, 0f);
    }

}
