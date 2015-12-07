using UnityEngine;
using DrupalUnity;

[RequireComponent(typeof(Collider))]
public class CityEngineBuilding : MonoBehaviour {

    int objID;
    DrupalUnityIO drupalIO;

    void Start() {
        drupalIO = FindObjectOfType<DrupalUnityIO>();
        if(!int.TryParse(gameObject.name.Substring(1, 5), out objID)) {
            Debug.LogError("Wasn't able to parse City Engine object name!");
        }
    }

    void OnMouseDown() {
        drupalIO.SelectBuilding(objID);
    }

}
