using UnityEngine;

public class CharacterController : MonoBehaviour {
    [SerializeField]
    float movementSpeed = 1.0f;
    [SerializeField]
    float sensitivity = 1.0f;

    void Update() {
        // Player Movement
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            float horizontal = Input.GetAxis("Horizontal");
            float depth = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(horizontal, 0f, depth).normalized;
            transform.Translate(movement * movementSpeed * Time.deltaTime);
        }
        // Player Rotation
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        transform.Rotate(Vector3.up, mouseX);
    }
}
