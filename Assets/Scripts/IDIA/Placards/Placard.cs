using UnityEngine;
using Drupal;

public class Placard : MonoBehaviour {

    public DrupalPlacard placard;

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
