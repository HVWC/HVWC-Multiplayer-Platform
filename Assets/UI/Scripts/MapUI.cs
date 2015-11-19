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
    }

}
