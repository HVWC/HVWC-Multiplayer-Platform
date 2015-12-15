// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
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
