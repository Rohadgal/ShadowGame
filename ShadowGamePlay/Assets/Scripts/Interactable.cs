using UnityEngine;

public class Interactable : MonoBehaviour {

    [SerializeField]
    float distanceInteractable = 4f;
    [SerializeField]
    public GameObject outlineMesh;

    void Start() {
        outlineMesh.SetActive(false);
    }

    public void Interact(RaycastHit hit) {
        bool isPickedUp = TogglePickupState();
        if (!isPickedUp) {
            Transform playerTransform = Camera.main.transform;
            transform.SetParent(playerTransform);
            transform.localPosition = Vector3.forward * distanceInteractable; // Adjust this position as needed
        } else {
            transform.SetParent(null, true);
        }
        Debug.Log("is picked up end: " + isPickedUp);
    }

    bool TogglePickupState() {
        Debug.Log("Interacting with 444: " + gameObject.name);
        // Toggle the pickup state and return the new state
        bool isPickedUp = transform.parent != null;
        return isPickedUp;
    }

}
