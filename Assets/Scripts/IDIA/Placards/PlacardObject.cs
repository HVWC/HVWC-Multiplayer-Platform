using UnityEngine;
using DrupalUnity;

public class PlacardObject : MonoBehaviour {

    public Placard placard;

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
