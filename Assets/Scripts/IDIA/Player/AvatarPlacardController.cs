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
using System.Collections;

public class AvatarPlacardController : MonoBehaviour {

    public float speed = 10f;

    void OnEnable() {
        DrupalUnityIO.OnPlacardSelected += OnPlacardSelected;
    }

    void OnPlacardSelected(Placard placard) {
        StartCoroutine(DoMove(GeographicManager.Instance.GetPosition(placard.location.latitude, placard.location.longitude, placard.location.elevation),placard.location.orientation));
    }

    void OnDisable() {
        DrupalUnityIO.OnPlacardSelected -= OnPlacardSelected;
    }

    void ToggleCollider(bool on) {
        Collider[] cs = transform.GetComponentsInChildren<Collider>();
        foreach(Collider c in cs) {
            c.enabled = on; //Turn on/off each collider on the player depending on their relative scene
        }
    }

    IEnumerator DoMove(Vector3 destination,double orientation) {
        ToggleCollider(false);
        float distance = Vector3.Distance(transform.position, destination);
        while(distance > 0) {
            distance = Vector3.Distance(transform.position, destination);
            transform.position = Vector3.Lerp(
            transform.position, destination,
            Time.deltaTime * (speed / distance));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f,(float)orientation,0f), Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }
        ToggleCollider(true);
    }

}
