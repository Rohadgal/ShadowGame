using UnityEngine;

public class CharacterController : MonoBehaviour {

    [SerializeField]
    private float movementSpeed = 1.0f;

    [SerializeField]
    private float sensitivity = 1.0f;

    private void Update() {
        // Player Movement
        float horizontal = Input.GetAxis("Horizontal");
        float depth = Input.GetAxis("Vertical");
        if (horizontal != 0 || depth != 0) {
            Vector3 movement = new Vector3(horizontal, 0f, depth).normalized;
            transform.Translate(movement * movementSpeed * Time.deltaTime);
        }
        // Player Rotation
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        transform.Rotate(Vector3.up, mouseX);
    }
}