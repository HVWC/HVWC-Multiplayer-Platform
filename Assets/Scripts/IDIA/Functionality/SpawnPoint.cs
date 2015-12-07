using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	void OnLevelWasLoaded(int level) {
        SpawnPlayer();
    }

    public void SpawnPlayer() {
        GameObject localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer");
        localPlayer.transform.position = transform.position;
        localPlayer.transform.rotation = transform.rotation;
    }

}
