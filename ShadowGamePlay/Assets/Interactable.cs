using UnityEngine;

public class Interactable : MonoBehaviour {

    //private Vector3 originalPosition;
    //private Quaternion originalRotation;

    void Start() {
        // Store the initial position and rotation
        //originalPosition = transform.position;
        //originalRotation = transform.rotation;
    }


    public void Interact(RaycastHit hit) {
        // Implement interaction logic here
        Debug.Log("Interacting with: " + gameObject.name);
        // Toggle the pickup state on the Interactable object
        bool isPickedUp = TogglePickupState();
        Debug.Log("is picked up beggin: " + isPickedUp);
        if (!isPickedUp) {
            // If picked up, move the object to a position relative to the player
            Transform playerTransform = Camera.main.transform;
            transform.SetParent(playerTransform);
            transform.localPosition = Vector3.forward * 4f; // Adjust this position as needed
        } else {
            transform.SetParent(null);
            transform.position = hit.point;
            //transform.position = originalPosition;
            //transform.rotation = originalRotation;
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
