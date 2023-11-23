using UnityEngine;

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
        setOutline();
    }

    void grabReleaseObject() {
       // setOutline();
        if (!isObjectGrabbed) {
            isObjectGrabbed = true;
            TryInteract();
            //setOutline();
            return;
        }
        isObjectGrabbed = false;
        if (interactable != null) {
            // soltar objeto
            interactable.Interact(hit);
        }
    }

    void TryInteract() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer)) {
            interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null) {
                interactable.Interact(hit); 
            }
        }
    }

    void setOutline() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer)) {
            interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null) {
                interactable.outlineMesh.SetActive(true);
            }
            return;
        }
        if(interactable != null) {
            interactable.outlineMesh.SetActive(false);
        }
        
    }
}