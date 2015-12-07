using UnityEngine;
using DrupalUnity;
using UnityEngine.Events;

public class BuildingObject : MonoBehaviour {

    public int[] validPlacardIDs;
    public UnityEvent OnValidPlacard,OnInvalidPlacard;

    void OnEnable() {
        DrupalUnityIO.OnPlacardSelected += OnPlacardSelected;
    }

    void OnPlacardSelected(Placard placard) {
        if (IsValidPlacardID(placard.id)) {
            OnValidPlacard.Invoke();
        } else {
            OnInvalidPlacard.Invoke();
        }
    }

    bool IsValidPlacardID(int id) {
        return validPlacardIDs.Contains(id);
    }

    void OnDisable() {
        DrupalUnityIO.OnPlacardSelected -= OnPlacardSelected;
    }
}
