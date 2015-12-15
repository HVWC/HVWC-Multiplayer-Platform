﻿// ----------------------------------------------------------------------------
// This source code is provided only under the Creative Commons licensing terms stated below.
// HVWC Multiplayer Platform alpha v1 by Institute for Digital Intermedia Arts at Ball State University \is licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
// Based on a work at https://github.com/HVWC/HVWC.
// Work URL: http://idialab.org/mellon-foundation-humanities-virtual-world-consortium/
// Permissions beyond the scope of this license may be available at http://idialab.org/info/.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/deed.en_US.
// ----------------------------------------------------------------------------
using UnityEngine;
using DrupalUnity;

public class AgentController : MonoBehaviour {

    public NavMeshAgent navMeshAgent;
    Vector3 destination;
	
    void OnEnable() {
        DrupalUnityIO.OnPlacardSelected += OnPlacardSelected;
    }

    void Update () {
        if(navMeshAgent.isOnNavMesh) {
            if(!navMeshAgent.pathPending) {
                navMeshAgent.SetDestination(destination);
                if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                    if(!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) {
                        navMeshAgent.Stop();
                    }
                }
            }
        }
	}

    void OnPlacardSelected(Placard placard) {
        destination = GeographicManager.Instance.GetPosition(placard.location.latitude, placard.location.longitude, placard.location.elevation);
        navMeshAgent.Resume();
    }

    void OnDisable() {
        DrupalUnityIO.OnPlacardSelected -= OnPlacardSelected;
    }
}
