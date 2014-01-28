using UnityEngine;
using System.Collections;

/// <summary>
/// This class allows for hyperlinking clicks on colliders.
/// </summary>
public class WebLink : MonoBehaviour {

	#region Fields
	/// <summary>
	/// The URL.
	/// </summary>
	public string url;
	#endregion

	#region Unity Messages
	/// <summary>
	/// A message called when the object is clicked.
	/// </summary>
	void OnMouseDown(){
		if(Application.isWebPlayer){ //If this is a web player
			Application.ExternalEval("window.open('"+url+"','_blank')"); //Open the URL in a new tab
		}else{ //If this is not a web player
			Application.OpenURL(url); //Just open the URL in the browser
		}
	}
	#endregion
	
}
