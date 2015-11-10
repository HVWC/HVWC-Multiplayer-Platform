using UnityEngine;
using System.Collections;
using Drupal;
using SimpleJSON;

public class NarrativeManager : MonoBehaviour {

    public DrupalPlacard[] placards;

    Canvas canvas;

    void Start() {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    void Update() {
        if (!canvas.worldCamera) {
            canvas.worldCamera = Camera.main;
        }
    }


}
