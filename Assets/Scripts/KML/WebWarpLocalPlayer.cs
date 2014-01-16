using UnityEngine;
using LitJson;
using System.Collections;

public class WebWarpLocalPlayer : Photon.MonoBehaviour
{
	internal class Location
	{
		public Location()
		{
			x = 0.0f;
			y = 0.0f;
			e = 0.0f;
		}
		
		public double x;
		public double y;
		public double e;
	}
	
	static GameObject player;

	public static GameObject Player {
		get {
			return player;
		}
	}

	GeographicMarker geoMarker;
	
	string decimalCoord = "41.892442, 12.48485, 40.0";
	string dmsCoord = "41 53'32.79\"N, 12 29'5.46\"E, 40.0";
	
	bool decimalCoords = false;
	bool showCoords = true;

	void Start(){
		geoMarker = (GeographicMarker)FindObjectOfType(typeof(GeographicMarker));
	}
		
	void OnGUI()
	{
		if (!showCoords){
			return;
		}
		
		GUILayout.BeginArea(new Rect(Screen.width*.1f, Screen.height - 65.0f, Screen.width*.82f, 30.0f));
		GUILayout.BeginHorizontal("box");
		
		if (decimalCoords = GUILayout.Toggle(decimalCoords, decimalCoords?"Decimal":"DMS", "button", GUILayout.Width(70.0f)))
		{
			GUILayout.Label("Coordinates", GUILayout.Width(70.0f));
			decimalCoord = GUILayout.TextField(decimalCoord);
			
			if (GUILayout.Button("Warp", GUILayout.Width(60.0f)))
			{
				/*GeographicCoord geoCoord = new GeographicCoord(GeographicCoord.Mode.LatLongDecimalDegrees);
				geoCoord.text = decimalCoord;
				geoCoord.elevation = System.Convert.ToSingle(decimalElevation);
				
				Vector3 p = geoMarker.Translate(geoCoord.ToGeoPoint());
				SetPlayerLocation(p);*/
				
				SetPlayerCoordinates(decimalCoord);
			}
		}
		else
		{
			GUILayout.Label("Coordinates", GUILayout.Width(70.0f));
			dmsCoord = GUILayout.TextField(dmsCoord);
			
			if (GUILayout.Button("Warp", GUILayout.Width(60.0f)))
			{
				GeographicCoord geoCoord = new GeographicCoord(GeographicCoord.Mode.LatLongDMS);
				geoCoord.text = dmsCoord;
				
				Vector3 p = geoMarker.Translate(geoCoord.ToGeoPoint());
				SetPlayerPosition(p);
			}
		}
		
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	public static void SetLocalPlayer(GameObject localPlayer){
		player = localPlayer;
		Application.ExternalCall(player!=null ? "EnableLinks" : "DisableLinks", player.name);
	}
	
	void SetPlayerCoordinates(string positionData){
		GeographicCoord geoCoord = new GeographicCoord(GeographicCoord.Mode.LatLongDecimalDegrees);
		geoCoord.text = positionData;
		
		Vector3 p = geoMarker.Translate(geoCoord.ToGeoPoint());
		
		SetPlayerPosition(p);
	}
	
	void SetPlayerPosition(Vector3 position){
		if (player == null){
			return;
		}
		SetPosition(position);
		Debug.DrawLine(position, position + Vector3.up * 20.0f, Color.red, 2.0f);
	}

	public void SetPosition(Vector3 position){
		transform.position = position;
	}

}
