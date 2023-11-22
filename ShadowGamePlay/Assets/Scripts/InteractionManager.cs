using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour {
    public float interactionRange = 3f;
    [SerializeField]
    public LayerMask interactableLayer;
    bool isObjectGrabbed;
    Interactable interactable;
    RaycastHit hit;


    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            grabReleaseObject();
        }
    }

    void grabReleaseObject() {
        if (!isObjectGrabbed) {
            TryInteract();
            return;
        }
        isObjectGrabbed = false;
        if (interactable != null) { }
        interactable.Interact(hit);
    }

    void TryInteract() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer)) {
            interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null) {
                interactable.Interact(hit);
                isObjectGrabbed = true;
            }
        }
    }
}