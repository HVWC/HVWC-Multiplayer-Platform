using UnityEngine;
using SimpleJSON;

namespace Drupal {

    [System.Serializable]
    public class DrupalEnvironment {
        public int id;
        public string title;
        public int currentTourID;
    }

    [System.Serializable]
    public class DrupalTour {
        public string title;
        public int id;
        public DrupalPlacard[] placards;
    }

    [System.Serializable]
    public class DrupalPlacard {
        public int id;
        public string title;
        public float latitude;
        public float longitude;
        public float elevation;
        public float orientation;
    }

    public class DrupalManager : MonoBehaviour {

        #region Events
        public delegate void GotCE(string json);
        public static event GotCE OnGotCurrentEnvironment;

        public delegate void GotT(string json);
        public static event GotT OnGotTour;

        public delegate void GotIWO(string json);
        public static event GotIWO OnGotInWorldObjects;

        public delegate void RegisteredPC(string result);
        public static event RegisteredPC OnRegisteredPlacardClick;

        public delegate void AddedP(string result);
        public static event AddedP OnAddedPlacard;
        #endregion

        #region Class Fields
        public DrupalEnvironment environment;
        public DrupalTour tour;
        public DrupalPlacard[] inWorldObjects;
        #endregion

        #region External Calls
        public void GetCurrentEnvironment() {
            Application.ExternalCall("DrupalUnityInterface.getCurrentEnvironment", gameObject.name, "GotCurrentEnvironment"); //external method: Interface.prototype.getCurrentEnvironment = function(gameObject,method){...}
        }

        public void GetTour() {
            Application.ExternalCall("DrupalUnityInterface.getTour", environment.currentTourID, gameObject.name, "GotTour"); //external method: Interface.prototype.getTour = function(tour_id,gameObject,method){...}
        }

        public void GetInWorldObjects() {
            Application.ExternalCall("DrupalUnityInterface.getInWorldObjects", environment.id, gameObject.name, "GotInWorldObjects"); //external method: Interface.prototype.getInWorldObjects = function(environment_id,gameObject,method){...}
        }

        public void RegisterPlacardClick(int placard_id) {
            Application.ExternalCall("DrupalUnityInterface.registerPlacardClick", placard_id, gameObject.name, "RegisteredPlacardClick"); //external method: Interface.prototype.registerPlacardClick = function(placard_id,gameObject,method){...}
        }
        
        public void AddPlacard(DrupalPlacard placard) {
            string placard_json = "{\"id\":" + placard.id + ",\"title\":\"" + placard.title + "\",\"latitude\":" + placard.latitude + ",\"longitude\":" + placard.longitude + ",\"elevation\":" + placard.elevation + ",\"orientation\":" + placard.orientation + "}";
            Application.ExternalCall("DrupalUnityInterface.addPlacard", environment.currentTourID, placard_json, gameObject.name, "AddedPlacard"); //external method: Interface.prototype.addPlacard = function(tour_id,placard,gameObject,method){...}
        }
        #endregion

        #region Callbacks
        public void GotCurrentEnvironment(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,jsonResultAsString);
            if(OnGotCurrentEnvironment != null) {
                OnGotCurrentEnvironment(json);
            }
            JSONNode node = JSON.Parse(json);
            environment = new DrupalEnvironment();
            environment.id = node["id"].AsInt;
            environment.title = node["title"];
            environment.currentTourID = node["current_tour_id"].AsInt;
        }

        public void GotTour(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,jsonResultAsString);
            if(OnGotTour != null) {
                OnGotTour(json);
            }
            JSONNode node = JSON.Parse(json);
            tour = new DrupalTour();
            tour.title = node["title"];
            tour.id = node["id"].AsInt;
            tour.placards = new DrupalPlacard[node["placards"].AsArray.Count];
            for(int i = 0; i < node["placards"].AsArray.Count; i++) {
                tour.placards[i].id = node["placards"][i]["id"].AsInt;
                tour.placards[i].title = node["placards"][i]["title"];
                tour.placards[i].latitude = node["placards"][i]["latitude"].AsFloat;
                tour.placards[i].longitude = node["placards"][i]["longitude"].AsFloat;
                tour.placards[i].elevation = node["placards"][i]["elevation"].AsFloat;
                tour.placards[i].orientation = node["placards"][i]["orientation"].AsFloat;
            }
        }

        public void GotInWorldObjects(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,jsonResultAsString);
            if(OnGotInWorldObjects != null) {
                OnGotInWorldObjects(json);
            }
            JSONNode node = JSON.Parse(json);
            inWorldObjects = new DrupalPlacard[node.AsArray.Count];
            for(int i = 0; i < node.AsArray.Count; i++) {
                inWorldObjects[i].id = node[i]["id"].AsInt;
                inWorldObjects[i].title = node[i]["title"];
                inWorldObjects[i].latitude = node[i]["latitude"].AsFloat;
                inWorldObjects[i].longitude = node[i]["longitude"].AsFloat;
                inWorldObjects[i].elevation = node[i]["elevation"].AsFloat;
                inWorldObjects[i].orientation = node[i]["orientation"].AsFloat;
            }
        }

        public void RegisteredPlacardClick(string result) {  //callback: u.getUnity().SendMessage(gameObject,method,resultAsString);
            if(OnRegisteredPlacardClick != null) {
                OnRegisteredPlacardClick(result);
            }
        }

        public void AddedPlacard(string result) {  //callback: u.getUnity().SendMessage(gameObject,method,resultAsString);
            if(OnAddedPlacard != null) {
                OnAddedPlacard(result);
            }
        }
        #endregion

    }
}
