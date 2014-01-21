using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class ProximityTrigger : MonoBehaviour {
	
	public string url;

	void Awake(){
		gameObject.layer = 2; //We must put this object on the IgnoreRaycast layer, so we do not block clicks
	}

	void OnTriggerEnter(Collider c){
		if(c.tag=="LocalPlayer"){
			if(Application.isWebPlayer){
				Application.ExternalEval("window.open('"+url+"','_blank')");
			}else{
				Application.OpenURL(url);
			}
		}
	}

}
