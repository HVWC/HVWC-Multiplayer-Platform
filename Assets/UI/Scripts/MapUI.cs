using UnityEngine;
using System.Collections;

public class MapUI : MonoBehaviour {

    SceneChanger sceneChanger;

	void Update () {
        if (!sceneChanger) {
            sceneChanger = FindObjectOfType<SceneChanger>();
        }
    }

    public void ChangeScene(string sceneName) {
        sceneChanger.LoadScene(sceneName);
    }

}
