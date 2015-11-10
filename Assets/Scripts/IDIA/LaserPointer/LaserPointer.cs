using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
public class LaserPointer : MonoBehaviour {

    public Transform root;
    LineRenderer lr;
    bool laserPointerEnabled = false;

    void Start() {
        lr = GetComponent<LineRenderer>();
        lr.SetVertexCount(2);
        lr.SetPosition(0, root.position);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.L) && !EventSystem.current.currentSelectedGameObject) {
            laserPointerEnabled = !laserPointerEnabled;
        }
        if (laserPointerEnabled) {
            lr.SetPosition(0, root.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Camera.main.farClipPlane)) {
                lr.SetPosition(1, hit.point);
            } else {
                lr.SetPosition(1, ray.direction* Camera.main.farClipPlane);
            }
        } else {
            lr.SetPosition(0, root.position);
            lr.SetPosition(1, root.position);
        }
    }

}
