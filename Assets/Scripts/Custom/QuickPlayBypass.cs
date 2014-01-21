using UnityEngine;
using System.Collections;

public class QuickPlayBypass : MonoBehaviour {

	bool didFirstJoin;

	void OnJoinedLobby(){
		if(Application.isWebPlayer && !didFirstJoin){
			GetURL();
			didFirstJoin = true;
		}
	}

	void GetURL(){
		Application.ExternalEval("u.getUnity().SendMessage(\"" + name + "\", \"ReceiveURL\", document.URL);");
	}

	void ReceiveURL(string url){
		CheckURL(url);
	}

	void CheckURL(string url){
		string quickPlayString = url.Substring(url.LastIndexOf("?")+1);
//		if(quickPlayString=="quickplay"){
//			Debug.Log ("URL indicates Quickplay. Set up Quickplay.");
//			SetUpQuickPlay();
//		}
		if(url.Contains("?quickplay")||quickPlayString.Contains("&quickplay")){
			SetUpQuickPlay();
		}
	}

	void SetUpQuickPlay(){
		NetworkManager.Instance.JoinRandomRoom();
		NetworkManager.Instance.PlayerName = "Guest" + Random.Range(1, 9999);
	}

}
