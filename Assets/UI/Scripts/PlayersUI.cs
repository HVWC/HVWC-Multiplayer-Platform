using UnityEngine;
using UnityEngine.UI;

public class PlayersUI : MonoBehaviour {

    public GameObject players;
    public GameObject playerPrefab;

    [HideInInspector]
    public PhotonPlayer selectedPlayer;

    void Start () {
        InvokeRepeating("RefreshPlayers",0f,5f);
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

}
