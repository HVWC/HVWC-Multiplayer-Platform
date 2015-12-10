using UnityEngine;

public class GeographicManager : MonoBehaviour {

    public static GeographicManager Instance { get; private set; }

    GeographicMarker geoMarker;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        geoMarker = FindObjectOfType<GeographicMarker>();
    }

    public Vector3 GetPosition(double latitude, double longitude, double elevation) {
        GeographicCoord geoCoord = new GeographicCoord(GeographicCoord.Mode.LatLongDecimalDegrees);
        geoCoord.text = latitude + ", " + longitude + ", " + elevation;
        return geoMarker.Translate(geoCoord.ToGeoPoint());
    }

}
