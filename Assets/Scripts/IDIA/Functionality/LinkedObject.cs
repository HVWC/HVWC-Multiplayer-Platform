using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class LinkedObject : MonoBehaviour {

    public float distance;
    public Color hoverColor;

    [System.Serializable]
    public class LinkedObjectClickEvent : UnityEvent { }

    [SerializeField]
    private LinkedObjectClickEvent m_onLinkClick = new LinkedObjectClickEvent();

    public LinkedObjectClickEvent onLinkClick {
        get {
            return m_onLinkClick;
        }
        set {
            m_onLinkClick = value;
        }
    }

    List<List<Color>> originalColors;

    void Start() {
        originalColors = new List<List<Color>>();
        if (!EventSystem.current.IsPointerOverGameObject()) {
            GetOriginalObjectColors(transform);
        }
    }

    void OnMouseEnter() {
        if (!EventSystem.current.IsPointerOverGameObject() && IsWithinDistance(distance)) {
            SetObjectColors(transform, hoverColor);
        }
    }

    void OnMouseOver() {
        if (!EventSystem.current.IsPointerOverGameObject() && IsWithinDistance(distance)) {
            SetObjectColors(transform, hoverColor);
        } else {
            SetObjectColors(transform, originalColors);
        }
    }

    void OnMouseDown() {
        if (!EventSystem.current.IsPointerOverGameObject() && IsWithinDistance(distance)) {
            m_onLinkClick.Invoke();
            SetObjectColors(transform, originalColors);
        }
    }

    void OnMouseExit() {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            SetObjectColors(transform, originalColors);
        }
    }

    void SetObjectColors(Transform t, Color c) {
        MeshRenderer[] renderers = t.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++) {
            for (int j = 0; j < renderers[i].materials.Length; j++) {
                renderers[i].materials[j].color = c;
            }
        }
    }

    void SetObjectColors(Transform t, List<List<Color>> c) {
        MeshRenderer[] renderers = t.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++) {
            for (int j = 0; j < renderers[i].materials.Length; j++) {
                renderers[i].materials[j].color = c[i][j];
            }
        }
    }

    void GetOriginalObjectColors(Transform t) {
        MeshRenderer[] renderers = t.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++) {
            originalColors.Add(new List<Color>());
            for (int j = 0; j < renderers[i].materials.Length; j++) {
                originalColors[i].Add(renderers[i].materials[j].color);
            }
        }
    }

    bool IsWithinDistance(float _distance) {
        if (!Camera.main) {
            return false;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, _distance);
    }

}
