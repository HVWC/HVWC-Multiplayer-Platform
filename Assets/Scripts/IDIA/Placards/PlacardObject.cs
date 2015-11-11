using UnityEngine;
using Drupal;

public class PlacardObject : MonoBehaviour {

    public Drupal.Placard placard;

    GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("LocalPlayer");
    }

    void Update() {
        if (!player) {
            player = GameObject.FindGameObjectWithTag("LocalPlayer");
        }
    }

    public void TeleportPlayer() {
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
    }

}
