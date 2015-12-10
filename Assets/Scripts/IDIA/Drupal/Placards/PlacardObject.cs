using UnityEngine;
using DrupalUnity;

public class PlacardObject : MonoBehaviour {

    public Placard placard;

    GameObject player;
    DrupalUnityIO drupalUnityIO;

    void Start() {
        player = GameObject.FindGameObjectWithTag("LocalPlayer");
        drupalUnityIO = FindObjectOfType<DrupalUnityIO>();
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

    void OnTriggerEnter(Collider col) {
        if(col.tag == "LocalPlayer") {
            drupalUnityIO.SelectPlacard(placard);
        }
    }

}
