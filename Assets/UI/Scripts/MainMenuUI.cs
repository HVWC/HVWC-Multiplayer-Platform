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

public class MainMenuUI : MonoBehaviour{

    public Button startButton,createRoomButton;
    public GameObject avatarAndName, roomSelection;

    public GameObject rooms;
    public GameObject roomPrefab;

    public string firstScene;

    [HideInInspector]
	public string selectedAvatar;

    void Start() {
        InvokeRepeating("RefreshRooms",0f,5f);
        selectedAvatar = "Adam";
    }

	void SaveSettings(){
		PlayerPrefs.SetString("playerName", NetworkManager.Instance.PlayerName);
		PlayerPrefs.SetString("avatar",selectedAvatar);
	}

    public void SelectAvatar(string avatar) {
        selectedAvatar = avatar;
    }

    public void SetPlayerName(string name) {
        NetworkManager.Instance.PlayerName = name;
    }

    public void ValidateStartButton(string input) {
        startButton.interactable = input.Length > 0 && NetworkManager.Instance.Connected;
    }

    public void ValidateCreateRoomButton(string input) {
        createRoomButton.interactable = input.Length > 0;
    }

    public void StartGame() {
        avatarAndName.SetActive(false);
        roomSelection.SetActive(true);
    }

    public void RefreshRooms() {
        Debug.Log(PhotonNetwork.GetRoomList().Length);
        for(int i = 0; i < rooms.transform.childCount;i++ ) {
            rooms.transform.GetChild(i).transform.FindChild("JoinButton").GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(rooms.transform.GetChild(i).gameObject);
        }
        foreach(RoomInfo room in PhotonNetwork.GetRoomList()) {
            GameObject roomObj = Instantiate(roomPrefab);
            roomObj.transform.SetParent(rooms.transform);
            roomObj.transform.FindChild("RoomName").GetComponent<Text>().text = room.name;
            roomObj.transform.FindChild("RoomPopulation").GetComponent<Text>().text = room.playerCount+"/"+room.maxPlayers;
            roomObj.transform.FindChild("JoinButton").GetComponent<Button>().onClick.AddListener(() => NetworkManager.Instance.JoinRoom(room.name));
        }
    }

    public void CreateRoom(InputField input) {
        if(input.text == "") {
            return;
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.isVisible = true;
        roomOptions.isOpen = true;
        roomOptions.maxPlayers = (byte)10;
        TypedLobby typedLobby = new TypedLobby();
        PhotonNetwork.CreateRoom(input.text,roomOptions,typedLobby);
    }


    void OnGUI() {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    void SpawnPlayer(string avatarName, Vector3 spawnPosition, Quaternion spawnRotation, int group) {
        GameObject player = NetworkManager.Instance.Instantiate(avatarName, spawnPosition, spawnRotation, group);
        player.GetComponent<CustomPlayerController>().enabled = true;
        player.tag = "LocalPlayer";
        player.transform.FindChild("Camera").gameObject.SetActive(true);
        player.transform.FindChild("Canvas").gameObject.SetActive(false);
        player.GetComponent<ClickTeleport>().enabled = true;
        //WebWarpLocalPlayer.Instance.SetLocalPlayer(player);
        /*Transform icon = player.transform.FindChild("Icon");
        icon.GetComponent<Renderer>().material.color = Color.green;
        icon.transform.position += Vector3.up * .05f;*/
    }

    void OnJoinedRoom() {
        Close();
        SpawnPlayer(selectedAvatar, Vector3.zero, gameObject.transform.rotation, 0); //Spawn the selected avatar at the spawn position
        SceneChanger.Instance.LoadScene(firstScene);
    }

    /// <summary>
    /// A message called when the local player leaves a room.
    /// </summary>
    void OnLeftRoom() {
        Open();
    }

    public void Open() {
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    
    public void Close() {
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

}
