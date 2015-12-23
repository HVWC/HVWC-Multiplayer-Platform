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

/// <summary>
/// This script handles avatar movement when a placard is selected.
/// </summary>
public class AvatarPlacardController : MonoBehaviour {

    float destinationElevation;

    #region Fields
    /// <summary>
    /// The speed of movement.
    /// </summary>
    public float speed = 10f;
    #endregion

    #region Unity Messages
    /// <summary>
    /// A message called when this script is enabled.
    /// </summary>
    void OnEnable() {
        DrupalUnityIO.OnPlacardSelected += OnPlacardSelected;
    }
    /// <summary>
    /// A message called when this script is disabled.
    /// </summary>
    void OnDisable() {
        DrupalUnityIO.OnPlacardSelected -= OnPlacardSelected;
    }
    #endregion

    #region Callbacks
    /// <summary>
    /// A callback called when a placard is selected in the Drupal Unity Interface.
    /// </summary>
    /// <param name="placard">
    /// The selected placard.
    /// </param>
    void OnPlacardSelected(Placard placard) {
        StartCoroutine(DoMove(GeographicManager.Instance.GetPosition(placard.location.latitude, placard.location.longitude, placard.location.elevation),placard.location.orientation));
    }
    #endregion

    #region Methods
    /// <summary>
    /// A method to toggle the collider on or off.
    /// </summary>
    /// <param name="on">
    /// Toggle the collider on or off?
    /// </param>
    void ToggleCollider(bool on) {
        Collider[] cs = transform.GetComponentsInChildren<Collider>();
        foreach(Collider c in cs) {
            c.enabled = on; //Turn on/off each collider on the player depending on their relative scene
        }
    }
    /// <summary>
    /// A coroutine to move the avatar to the destination and at the orientation.
    /// </summary>
    /// <param name="destination">
    /// The destination.
    /// </param>
    /// <param name="orientation">
    /// The orientation.
    /// </param>
    /// <returns>IEnumerator</returns>
    IEnumerator DoMove(Vector3 destination,double orientation) {
        destinationElevation = destination.y;
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
    #endregion

}
