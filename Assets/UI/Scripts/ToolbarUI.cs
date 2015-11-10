using UnityEngine;
using System.Collections;

public class ToolbarUI : MonoBehaviour {

    public void JoinMe() {
        Application.OpenURL("joinme://");
    }

    public void Exit() {
        PhotonNetwork.LeaveRoom();
    }

}
