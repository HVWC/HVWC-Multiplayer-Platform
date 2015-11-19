﻿using UnityEngine;
using LitJson;

namespace DrupalUnity {

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

    [System.Serializable]
    public class Status {
        public bool success;
    }

    public class DrupalUnityIO : MonoBehaviour {

        #region Events
        public delegate void AddedP(Status added);
        public static event AddedP OnAddedPlacard;

        public delegate void GotCE(Environment currentEnvironment);
        public static event GotCE OnGotCurrentEnvironment;

        public delegate void GotCPId(int placardId);
        public static event GotCPId OnGotCurrentPlacardId;

        public delegate void GotCTId(int currentTourId);
        public static event GotCTId OnGotCurrentTourId;

        public delegate void GotE(Environment environment);
        public static event GotE OnGotEnvironment;

        public delegate void GotPs(Placard[] placards);
        public static event GotPs OnGotPlacards;

        public delegate void GotT(Tour tour);
        public static event GotT OnGotTour;

        public delegate void PlacardS(Placard placard);
        public static event PlacardS OnPlacardSelected;

        public delegate void BuildingS(int buildingId);
        public static event BuildingS OnBuildingSelected;
        #endregion

        #region Debug Fields
#if UNITY_EDITOR
        bool debug = true;
#else
        bool debug=false;
#endif
        public TextAsset addPlacardText, currentEnvironmentText, currentPlacardIdText, currentTourIdText, environmentText, inWorldObjectsText, placardsText, tourText, registerPlacardClickText;
        #endregion

        void Start() {
            AddEventListener(gameObject.name, "PlacardSelected", "placard_selected");
            AddEventListener(gameObject.name, "BuildingSelected", "building_selected");
        }

        #region External Calls
        public void AddPlacard(Placard placard) {
            if (debug) {
                AddedPlacard(addPlacardText.text);
                return;
            }
            string placard_json = JsonMapper.ToJson(placard);
            Application.ExternalCall("DrupalUnityInterface.addPlacard", gameObject.name, "AddedPlacard", placard_json);
        }

        public void GetCurrentEnvironment() {
            if (debug) {
                GotCurrentEnvironment(currentEnvironmentText.text);
                return;
            }
            Application.ExternalCall("DrupalUnityInterface.getCurrentEnvironment", gameObject.name, "GotCurrentEnvironment");
        }

        public void GetCurrentPlacardId() {
            if (debug) {
                Debug.LogWarning("GotCurrentPlacardId doesn't have a debug text yet!");
                return;
            }
            Application.ExternalCall("DrupalUnityInterface.getCurrentPlacardId", gameObject.name, "GotCurrentPlacardId");
        }

        public void GetCurrentTourId() {
            if (debug) {
                Debug.LogWarning("GotCurrentTourId doesn't have a debug text yet!");
                return;
            }
            Application.ExternalCall("DrupalUnityInterface.getCurrentTourId", gameObject.name, "GotCurrentTourId");
        }

        public void GetEnvironment(int environment_id) {
            if (debug) {
                Debug.LogWarning("GotEnvironment doesn't have a debug text yet!");
                return;
            }
            string environment_id_json = JsonMapper.ToJson(environment_id);
            Application.ExternalCall("DrupalUnityInterface.getEnvironment", gameObject.name, "GotEnvironment", environment_id_json);
        }

        public void GetPlacards(int[] placard_ids) {
            if (debug) {
                Debug.LogWarning("GotPlacards doesn't have a debug text yet!");
                return;
            }
            string placard_id_json = JsonMapper.ToJson(placard_ids);
            Application.ExternalCall("DrupalUnityInterface.getPlacards", gameObject.name, "GotPlacards", placard_id_json);
        }

        public void GetTour(int tour_id) {
            if (debug) {
                GotTour(tourText.text);
                return;
            }
            string tour_id_json = JsonMapper.ToJson(tour_id);
            Application.ExternalCall("DrupalUnityInterface.getTour", gameObject.name, "GotTour", tour_id_json);
        }

        public void SelectPlacard(Placard placard) {
            if (debug) {
                return;
            }
            string placard_json = JsonMapper.ToJson(placard);
            TriggerEvent("placard_selected",placard_json);
        }

        public void SelectBuilding(int buildingId) {
            if (debug) {
                return;
            }
            string building_id_json = JsonMapper.ToJson(buildingId);
            TriggerEvent("building_selected", building_id_json);
        }

        public void AddEventListener(string gameObjectName, string callback, string eventName) {
            if (debug) {
                return;
            }
            Application.ExternalCall("DrupalUnityInterface.addEventListener", gameObjectName, callback, eventName);
        }

        public void TriggerEvent(string eventName, string jsonArgs) {
            if (debug) {
                return;
            }
            Application.ExternalCall("DrupalUnityInterface.triggerEvent", eventName, jsonArgs);
        }
        #endregion

        #region Callbacks
        public void AddedPlacard(string json) {  
            Status added;
            try {
                added = JsonMapper.ToObject<Status>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnAddedPlacard != null) {
                OnAddedPlacard(added);
            }
        }

        public void GotCurrentEnvironment(string json) { 
            Environment currentEnvironment;
            try {
                currentEnvironment = JsonMapper.ToObject<Environment>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnGotCurrentEnvironment != null) {
                OnGotCurrentEnvironment(currentEnvironment);
            }
        }

        public void GotCurrentPlacardId(string json) {
            int currentPlacardId;
            try {
                currentPlacardId = JsonMapper.ToObject<int>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnGotCurrentPlacardId != null) {
                OnGotCurrentPlacardId(currentPlacardId);
            }
        }

        public void GotCurrentTourId(string json) {
            int currentTourId;
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

        public void GotEnvironment(string json) {
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

        public void GotPlacards(string json) {
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

        public void GotTour(string json) {
            Tour tour;
            try {
                tour = JsonMapper.ToObject<Tour>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnGotTour != null) {
                OnGotTour(tour);
            }
        }

        public void PlacardSelected(string json) {
            Placard placard;
            try {
                placard = JsonMapper.ToObject<Placard>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnPlacardSelected != null) {
                OnPlacardSelected(placard);
            }
        }

        public void BuildingSelected(string json) {
            int buildingId;
            try {
                buildingId = JsonMapper.ToObject<int>(json);
            } catch {
                Debug.LogError("Unable to map JSON to object.");
                return;
            }
            if (OnBuildingSelected != null) {
                OnBuildingSelected(buildingId);
            }
        }
        #endregion

    }
}