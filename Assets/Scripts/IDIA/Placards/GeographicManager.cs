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

    public void SetObjectCoordinates(Transform t,string positionData) {
        GeographicCoord geoCoord = new GeographicCoord(GeographicCoord.Mode.LatLongDecimalDegrees);
        geoCoord.text = positionData;
        Vector3 p = geoMarker.Translate(geoCoord.ToGeoPoint());
        SetPosition(t,p);
    }

    public void SetPosition(Transform t, Vector3 position) {
        t.position = position;
    }

}
