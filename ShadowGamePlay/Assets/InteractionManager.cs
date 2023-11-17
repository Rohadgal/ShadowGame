using UnityEngine;

public class InteractionManager : MonoBehaviour {
    public float interactionRange = 3f;
    public LayerMask interactableLayer;

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            TryInteract();
        }
    }

    void TryInteract() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer)) {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null) {
                // Call the interact method on the interactable object

                Debug.Log("name d: " + interactable.name);
                Debug.Log("name j: " + hit);
                interactable.Interact(hit);
            }
        }
    }
}
