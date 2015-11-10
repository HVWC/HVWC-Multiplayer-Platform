using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour {

    public GameObject messages;
    public GameObject messagePrefab;

    public InputField chatInput;

    Chat chat;
    PlayersUI playersUI;

    void Start() {
        chat = FindObjectOfType<Chat>();
        playersUI = FindObjectOfType<PlayersUI>();
        InvokeRepeating("RefreshChat", 0f, 5f);
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
        foreach (string message in chat.Messages) {
            GameObject messageObj = Instantiate(messagePrefab);
            messageObj.transform.SetParent(messages.transform);
            messageObj.GetComponent<Text>().text = message;
        }
    }

}
