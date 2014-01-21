using UnityEngine;
using System.Collections;

public class WebLink : MonoBehaviour {

	public string url;

	void OnMouseDown(){
		if(Application.isWebPlayer){
			Application.ExternalEval("window.open('"+url+"','_blank')");
		}else{
			Application.OpenURL(url);
		}
	}
	
}
