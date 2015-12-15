// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

public class MapUI : MonoBehaviour {

    GameObject player;
    SceneChanger sceneChanger;

	void Update () {
        player = GameObject.FindGameObjectWithTag("LocalPlayer");
        if (!sceneChanger) {
            sceneChanger = FindObjectOfType<SceneChanger>();
        }
    }

    public void ChangeScene(string sceneName) {
        sceneChanger.LoadScene(sceneName);
    }

    public void TeleportPlayer(Transform t) {
        player.transform.position = t.position;
        player.transform.rotation = t.rotation;
    }

}
