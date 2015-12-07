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
