using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    [SerializeField]
    Handle[] handles;
    [SerializeField]
    Vector3 inputMultiplier;

    bool dragging = false;
    GameObject currentGO;

    void Update() {
        HandleMouseInput();
    }

    void HandleMouseInput() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Collider col = hit.collider;
                if (col != null) {
                    currentGO = col.gameObject;
                }
            }
        }
        else if (Input.GetMouseButton(0) && currentGO != null) {
            foreach (Handle h in handles) {
                if (h.gameObject == currentGO) {
                    UpdateHandle(h);
                }
            }
        }
        else {
            currentGO = null;
        }
    }

    void UpdateHandle(Handle handle) {
        Vector3 rawPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = new Vector3(rawPos.x * inputMultiplier.x, rawPos.y * inputMultiplier.y, rawPos.z * inputMultiplier.z);
        handle.UpdatePosition(newPos);
    }
}
