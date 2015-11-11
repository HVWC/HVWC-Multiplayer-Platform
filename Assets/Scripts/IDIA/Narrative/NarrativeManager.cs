using UnityEngine;
using System.Collections;
using Drupal;

public class NarrativeManager : MonoBehaviour {

    public Drupal.Placard[] placards;

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
