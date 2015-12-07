using UnityEngine;
using System.Collections;

public class SceneChanger : Photon.MonoBehaviour {

    public static SceneChanger Instance { get; private set; }
    public GameObject loadingScreen;

    void Awake() {
        DontDestroyOnLoad(loadingScreen);
        if(Instance == null) { //If there is no instance of this script, then set the instance to be this script and make the gameobject survive scene changes
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else { //If there is already an instance of this script, then destroy this gameobject
            Destroy(gameObject);
        }
    }

    void Update() {
        if(!loadingScreen) {
            loadingScreen = GameObject.Find("LoadingScreen");
        }
    }

    public void LoadScene(string sceneName) {
        StartCoroutine(DoLoadScene(sceneName));
    }

    IEnumerator DoLoadScene(string sceneName) {
#if UNITY_EDITOR || UNITY_STANDALONE
        loadingScreen.SetActive(true);
        AsyncOperation async = Application.LoadLevelAsync(sceneName);
        while(!async.isDone) {
            yield return new WaitForSeconds(.5f);
        }
        loadingScreen.SetActive(false);
        yield return async;
#elif UNITY_WEBPLAYER
        loadingScreen.SetActive(true);
        while (!Application.CanStreamedLevelBeLoaded(sceneName)) {
            yield return new WaitForSeconds(.1f);
        }
        AsyncOperation async = Application.LoadLevelAsync(sceneName);
        while(!async.isDone) {
            yield return new WaitForSeconds(.1f);
        }
        loadingScreen.SetActive(false);
        yield return async;
#endif
    }

    void OnLeftRoom() {
        LoadScene("MainMenu");
    }

}
