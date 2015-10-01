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
        public DrupalEnvironment currentEnvironment;
        public DrupalTour tour;
        public DrupalPlacard[] inWorldObjects;
        #endregion

        #region Debug Fields
        public bool debug;
        public TextAsset currentEnvironmentText, tourText, inWorldObjectsText, registerPlacardClickText, addPlacardText;
        #endregion

        void Start() {
            GetCurrentEnvironment();
        }

        #region External Calls
        public void GetCurrentEnvironment() {
            if (debug) {
                GotCurrentEnvironment(currentEnvironmentText.text);
                return;
            }
            Application.ExternalCall("DrupalUnityInterface.getCurrentEnvironment", gameObject.name, "GotCurrentEnvironment"); //external method: Interface.prototype.getCurrentEnvironment = function(gameObject,method){...}
        }

        public void GetTour() {
            if (debug) {
                GotTour(tourText.text);
                return;
            }
            Application.ExternalCall("DrupalUnityInterface.getTour", currentEnvironment.currentTourID, gameObject.name, "GotTour"); //external method: Interface.prototype.getTour = function(tour_id,gameObject,method){...}
        }

        public void GetInWorldObjects() {
            if (debug) {
                GotInWorldObjects(inWorldObjectsText.text);
                return;
            }
            Application.ExternalCall("DrupalUnityInterface.getInWorldObjects", currentEnvironment.id, gameObject.name, "GotInWorldObjects"); //external method: Interface.prototype.getInWorldObjects = function(environment_id,gameObject,method){...}
        }

        public void RegisterPlacardClick(int placard_id) {
            if (debug) {
                RegisteredPlacardClick(registerPlacardClickText.text);
                return;
            }
            Application.ExternalCall("DrupalUnityInterface.registerPlacardClick", placard_id, gameObject.name, "RegisteredPlacardClick"); //external method: Interface.prototype.registerPlacardClick = function(placard_id,gameObject,method){...}
        }
        
        public void AddPlacard(DrupalPlacard placard) {
            if (debug) {
                AddedPlacard(addPlacardText.text);
                return;
            }
            string placard_json = "{\"id\":" + placard.id + ",\"title\":\"" + placard.title + "\",\"latitude\":" + placard.latitude + ",\"longitude\":" + placard.longitude + ",\"elevation\":" + placard.elevation + ",\"orientation\":" + placard.orientation + "}";
            Application.ExternalCall("DrupalUnityInterface.addPlacard", currentEnvironment.currentTourID, placard_json, gameObject.name, "AddedPlacard"); //external method: Interface.prototype.addPlacard = function(tour_id,placard,gameObject,method){...}
        }
        #endregion

        #region Callbacks
        public void GotCurrentEnvironment(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,jsonResultAsString);
            JSONNode node = JSON.Parse(json);
            currentEnvironment = new DrupalEnvironment();
            currentEnvironment.id = node["id"].AsInt;
            currentEnvironment.title = node["title"];
            currentEnvironment.currentTourID = node["current_tour_id"].AsInt;
            if (OnGotCurrentEnvironment != null) {
                OnGotCurrentEnvironment(json);
            }
            GetTour();
        }

        public void GotTour(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,jsonResultAsString);
            JSONNode node = JSON.Parse(json);
            tour = new DrupalTour();
            tour.title = node["title"];
            tour.id = node["id"].AsInt;
            tour.placards = new DrupalPlacard[node["placards"].AsArray.Count];
            for(int i = 0; i < node["placards"].AsArray.Count; i++) {
                tour.placards[i] = new DrupalPlacard();
                tour.placards[i].id = node["placards"][i]["id"].AsInt;
                tour.placards[i].title = node["placards"][i]["title"];
                tour.placards[i].latitude = node["placards"][i]["latitude"].AsFloat;
                tour.placards[i].longitude = node["placards"][i]["longitude"].AsFloat;
                tour.placards[i].elevation = node["placards"][i]["elevation"].AsFloat;
                tour.placards[i].orientation = node["placards"][i]["orientation"].AsFloat;
            }
            if (OnGotTour != null) {
                OnGotTour(json);
            }
            GetInWorldObjects();
        }

        public void GotInWorldObjects(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,jsonResultAsString);
            JSONNode node = JSON.Parse(json);
            inWorldObjects = new DrupalPlacard[node["placards"].AsArray.Count];
            for(int i = 0; i < node["placards"].AsArray.Count; i++) {
                inWorldObjects[i] = new DrupalPlacard();
                inWorldObjects[i].id = node["placards"][i]["id"].AsInt;
                inWorldObjects[i].title = node["placards"][i]["title"];
                inWorldObjects[i].latitude = node["placards"][i]["latitude"].AsFloat;
                inWorldObjects[i].longitude = node["placards"][i]["longitude"].AsFloat;
                inWorldObjects[i].elevation = node["placards"][i]["elevation"].AsFloat;
                inWorldObjects[i].orientation = node["placards"][i]["orientation"].AsFloat;
            }
            if (OnGotInWorldObjects != null) {
                OnGotInWorldObjects(json);
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
