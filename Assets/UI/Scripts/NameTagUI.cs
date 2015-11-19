using UnityEngine;
using UnityEngine.UI;

public class NameTagUI : MonoBehaviour {

    public PhotonView photonView;
    public Text nameTagText;

	// Use this for initialization
	void Awake () {
        nameTagText.enabled = !photonView.isMine;
        nameTagText.text = photonView.owner.name;
	}

}
