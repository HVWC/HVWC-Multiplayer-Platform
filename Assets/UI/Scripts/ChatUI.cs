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

public class ChatUI : MonoBehaviour {

    public GameObject messages;
    public GameObject messagePrefab;

    public InputField chatInput;

    Chat chat;
    PlayersUI playersUI;

    void OnEnable() {
        Chat.OnGotChat += OnGotChat;
    }

    void Start() {
        chat = FindObjectOfType<Chat>();
        playersUI = FindObjectOfType<PlayersUI>();
    }

    public void SendChat() {
        if (chatInput.text.Length > 0) {
            if (playersUI.selectedPlayer!=null) {
                chat.SendChat(playersUI.selectedPlayer, chatInput.text);
            } else {
                chat.SendChat(PhotonTargets.All, chatInput.text);
            }
        }
        ClearInput();
    }

    void ClearInput() {
        chatInput.text = "";
    }

    public void RefreshChat() {
        for (int i = 0; i < messages.transform.childCount; i++) {
            Destroy(messages.transform.GetChild(i).gameObject);
        }
        foreach (string message in Chat.Messages) {
            GameObject messageObj = Instantiate(messagePrefab);
            messageObj.transform.SetParent(messages.transform);
            messageObj.GetComponent<Text>().text = message;
        }
    }

    private void OnGotChat(string message) {
        RefreshChat();
    }

    void OnDisable() {
        Chat.OnGotChat -= OnGotChat;
    }

}
