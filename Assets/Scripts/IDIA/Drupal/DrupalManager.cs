using UnityEngine;
using LitJson;

namespace Drupal {

    [System.Serializable]
    public class Environment {
        public int id;
        public string title;
        public string description;
        public Location starting_location;
        public Tour[] tours;
    }

    [System.Serializable]
    public class Tour {
        public int id;
        public string title;
        public string description;
        public Placard[] placards;
    }

    [System.Serializable]
    public class Placard {
        public int id;
        public string title;
        public string description;
        public Location location;
    }

    [System.Serializable]
    public class Location {
        public double latitude;
        public double longitude;
        public double elevation;
        public double orientation;
    }

    public class DrupalManager : MonoBehaviour {

        #region Events
        public delegate void AddedP(bool added);
        public static event AddedP OnAddedPlacard;

        public delegate void GotCE(Environment currentEnvironment);
        public static event GotCE OnGotCurrentEnvironment;

        public delegate void GotCPId(int placardId);
        public static event GotCPId OnGotCurrentPlacardId;

        public delegate void GotCTId(int currentTourId);
        public static event GotCTId OnGotCurrentTourId;

        public delegate void GotE(Environment environment);
        public static event GotE OnGotEnvironment;

        public delegate void GotIWOs(Placard[] inWorldObjects);
        public static event GotIWOs OnGotInWorldObjects;

        public delegate void GotPs(Placard[] placards);
        public static event GotPs OnGotPlacards;

        public delegate void GotT(Tour tour);
        public static event GotT OnGotTour;

        public delegate void RegisteredPC(bool registered);
        public static event RegisteredPC OnRegisteredPlacardClick;
        #endregion

        #region Class Fields
        public Environment currentEnvironment;
        public int currentPlacardId;
        public int currentTourId;
        public Tour tour;
        public Placard[] inWorldObjects;
        #endregion

        #region Debug Fields
        bool debug;
        public TextAsset addPlacardText, currentEnvironmentText, currentPlacardIdText, currentTourIdText, environmentText, inWorldObjectsText, placardsText, tourText,  registerPlacardClickText;
        #endregion

        void Start() {
            GetCurrentEnvironment();
#if UNITY_EDITOR
            debug = true;
#else
            debug=false;
#endif
        }

#region External Calls
        public void AddPlacard(Placard placard) {
            if (debug) {
                AddedPlacard(addPlacardText.text);
                return;
            }
            string placard_json = JsonMapper.ToJson(placard);
            Application.ExternalCall("DrupalUnityInterface.addPlacard", gameObject.name, "AddedPlacard", placard_json); //external method: Interface.prototype.addPlacard = function(tour_id,placard,gameObject,method){...}
        }

        public void GetCurrentEnvironment() {
            if (debug) {
                GotCurrentEnvironment(currentEnvironmentText.text);
                return;
            }
            Application.ExternalCall("DrupalUnityInterface.getCurrentEnvironment", gameObject.name, "GotCurrentEnvironment"); //external method: Interface.prototype.getCurrentEnvironment = function(gameObject,method){...}
        }

        public void GetCurrentPlacardId() {
            if (debug) {
                Debug.LogWarning("GotCurrentPlacardId doesn't have a debug text yet!");
                return;
            }
            Application.ExternalCall("DrupalUnityInterface.getCurrentPlacardId", gameObject.name, "GotCurrentPlacardId"); //external method: Interface.prototype.getCurrentEnvironment = function(gameObject,method){...}
        }

        public void GetCurrentTourId() {
            if (debug) {
                Debug.LogWarning("GotCurrentTourId doesn't have a debug text yet!");
                return;
            }
            Application.ExternalCall("DrupalUnityInterface.getCurrentTourId", gameObject.name, "GotCurrentTourId"); //external method: Interface.prototype.getCurrentEnvironment = function(gameObject,method){...}
        }

        public void GetEnvironment(int environment_id) {
            if (debug) {
                Debug.LogWarning("GotCurrentTourId doesn't have a debug text yet!");
                return;
            }
            string environment_id_json = JsonMapper.ToJson(environment_id);
            Application.ExternalCall("DrupalUnityInterface.getEnvironment", gameObject.name, "GotEnvironment", environment_id_json); //external method: Interface.prototype.getCurrentEnvironment = function(gameObject,method){...}
        }

        public void GetInWorldObjects(int environment_id) {
            if (debug) {
                GotInWorldObjects(inWorldObjectsText.text);
                return;
            }
            string environment_id_json = JsonMapper.ToJson(environment_id);
            Application.ExternalCall("DrupalUnityInterface.getInWorldObjects", gameObject.name, "GotInWorldObjects", environment_id_json); //external method: Interface.prototype.getInWorldObjects = function(environment_id,gameObject,method){...}
        }

        public void GetPlacards(int[] placard_ids) {
            if (debug) {
                Debug.LogWarning("GotPlacards doesn't have a debug text yet!");
                return;
            }
            string placard_id_json = JsonMapper.ToJson(placard_ids);
            Application.ExternalCall("DrupalUnityInterface.getPlacards", gameObject.name, "GotPlacards", placard_id_json); //external method: Interface.prototype.getInWorldObjects = function(environment_id,gameObject,method){...}
        }

        public void GetTour(int tour_id) {
            if (debug) {
                GotTour(tourText.text);
                return;
            }
            string tour_id_json = JsonMapper.ToJson(tour_id);
            Application.ExternalCall("DrupalUnityInterface.getTour", gameObject.name, "GotTour", tour_id_json); //external method: Interface.prototype.getTour = function(gameObject,method,tour_id_json){...}
        }

        public void RegisterPlacardClick(int placard_id) {
            if (debug) {
                RegisteredPlacardClick(registerPlacardClickText.text);
                return;
            }
            string placard_id_json = JsonMapper.ToJson(placard_id);
            Application.ExternalCall("DrupalUnityInterface.registerPlacardClick", gameObject.name, "RegisteredPlacardClick", placard_id_json); //external method: Interface.prototype.registerPlacardClick = function(gameObject,method,placard_id_json){...}
        }
#endregion

#region Callbacks
        public void AddedPlacard(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,resultAsString);
            bool added;
            try {
                added = JsonMapper.ToObject<bool>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnAddedPlacard != null) {
                OnAddedPlacard(added);
            }
        }

        public void GotCurrentEnvironment(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,jsonResultAsString);
            try {
                currentEnvironment = JsonMapper.ToObject<Environment>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnGotCurrentEnvironment != null) {
                OnGotCurrentEnvironment(currentEnvironment);
            }
            GetTour(currentEnvironment.tours[0].id);
        }

        public void GotCurrentPlacardId(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,jsonResultAsString);
            try {
                currentPlacardId = JsonMapper.ToObject<int>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnGotCurrentPlacardId != null) {
                OnGotCurrentPlacardId(currentPlacardId);
            }
            GetTour(currentEnvironment.tours[0].id);
        }

        public void GotCurrentTourId(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,jsonResultAsString);
            try {
                currentTourId = JsonMapper.ToObject<int>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnGotCurrentTourId != null) {
                OnGotCurrentPlacardId(currentTourId);
            }
        }

        public void GotEnvironment(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,jsonResultAsString);
            Environment environment;
            try {
                environment = JsonMapper.ToObject<Environment>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnGotEnvironment != null) {
                OnGotEnvironment(environment);
            }
        }

        public void GotInWorldObjects(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,jsonResultAsString);
            try {
                inWorldObjects = JsonMapper.ToObject<Placard[]>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnGotInWorldObjects != null) {
                OnGotInWorldObjects(inWorldObjects);
            }
        }

        public void GotPlacards(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,jsonResultAsString);
            Placard[] placards;
            try {
                placards = JsonMapper.ToObject<Placard[]>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnGotPlacards != null) {
                OnGotPlacards(placards);
            }
        }

        public void GotTour(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,jsonResultAsString);
            try {
                tour = JsonMapper.ToObject<Tour>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnGotTour != null) {
                OnGotTour(tour);
            }
            GetInWorldObjects(currentEnvironment.id);
        }

        public void RegisteredPlacardClick(string json) {  //callback: u.getUnity().SendMessage(gameObject,method,resultAsString);
            bool registered;
            try {
                registered = JsonMapper.ToObject<bool>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnRegisteredPlacardClick != null) {
                OnRegisteredPlacardClick(registered);
            }
        }
#endregion

    }
}
