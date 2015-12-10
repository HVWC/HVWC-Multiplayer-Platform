using UnityEngine;

public class WebManager : MonoBehaviour {

	public void OpenURL(string url) {
        Application.OpenURL(url);
    }

#if UNITY_WEBPLAYER
    public void OpenURLInNewTab(string url) {
        Application.ExternalCall("window.open",url,"_blank");
    }
#endif

}
