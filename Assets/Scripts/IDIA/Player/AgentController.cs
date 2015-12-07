using UnityEngine;
using DrupalUnity;
using System.Collections;

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
