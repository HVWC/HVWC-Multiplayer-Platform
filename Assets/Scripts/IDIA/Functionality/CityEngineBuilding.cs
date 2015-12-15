// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
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
        Application.ExternalCall("window.open", "http://mandala.shanti.virginia.edu/places/"+objID+"/overview/nojs", "_blank");
    }

}
