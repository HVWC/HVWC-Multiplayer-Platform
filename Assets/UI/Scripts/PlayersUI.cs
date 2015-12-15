// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;

public class PlayersUI : MonoBehaviour {

    public GameObject players;
    public GameObject playerPrefab;

    [HideInInspector]
    public PhotonPlayer selectedPlayer;

    void Start () {
        RefreshPlayers();
	}

    public void RefreshPlayers() {
        for (int i = 0; i < players.transform.childCount; i++) {
            players.transform.GetChild(i).GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
            Destroy(players.transform.GetChild(i).gameObject);
        }
        foreach (PhotonPlayer player in PhotonNetwork.playerList) {
            GameObject playerObj = Instantiate(playerPrefab);
            playerObj.transform.SetParent(players.transform);
            playerObj.transform.FindChild("Name").GetComponent<Text>().text = player.name;
            playerObj.GetComponent<Toggle>().onValueChanged.AddListener((on) => { selectedPlayer = on ? player : null; });
        }
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
        RefreshPlayers();
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        RefreshPlayers();
    }

}
