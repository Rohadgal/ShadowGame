using UnityEngine;

/// <summary>
/// Controls the 2D movement and interactions of the character.
/// </summary>
public class CharacterController2D : MonoBehaviour {

    #region Public

    /// <summary>
    /// Layer mask to determine what is considered as ground.
    /// </summary>
    public LayerMask whatIsGround;

    #endregion Public

    #region Serialize Fields

    [SerializeField] private float xSpeed, jumpForce, footRadius;
    [SerializeField] private Transform footPosition;

    #endregion Serialize Fields

    #region Private

    private Rigidbody m_rb;
    private bool m_isFacingRight, m_isGrounded;

    #endregion Private

    private void Awake() {
        m_isFacingRight = true;
        m_isGrounded = false;
    }

    private void Start() {
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (Input.GetButtonDown("Jump")) {
            jump();
        }
    }

    private void FixedUpdate() {
        m_isGrounded = Physics.CheckSphere(footPosition.position, footRadius, whatIsGround) &&
                   m_rb.velocity.y < 0.1f;

        horizontalMovement();
        verticalMovement();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(footPosition.position, footRadius);
    }

    /// <summary>
    /// Handles horizontal movement, flipping the character, and additional actions based on input.
    /// </summary>
    private void horizontalMovement() {
        float xMove = Input.GetAxisRaw("Horizontal");
        m_rb.velocity = new Vector2(xMove * xSpeed, m_rb.velocity.y);

        if ((xMove < 0 && m_isFacingRight) || (xMove > 0 && !m_isFacingRight)) {
            flip();
        }

        if (m_isGrounded) {
            if (xMove != 0) {
                PlayerManager.instance.changePlayerSate(PlayerState.Running);
            } else if (xMove == 0) {
                PlayerManager.instance.changePlayerSate(PlayerState.Idle);
            }
        }
    }

    /// <summary>
    /// Handles vertical movement and additional actions based on the vertical velocity.
    /// </summary>
    private void verticalMovement() {
        if (m_isGrounded) {
            return;
        }
        if (m_rb.velocity.y >= 0.1f) {
            PlayerManager.instance.changePlayerSate(PlayerState.Jump);
        } else if (m_rb.velocity.y < -0.1f) {
            PlayerManager.instance.changePlayerSate(PlayerState.JumpFall);
        }
    }

    /// <summary>
    /// Initiates a jump if the character is grounded.
    /// </summary>
    private void jump() {
        if (!m_isGrounded) {
            return;
        }
        m_rb.velocity = new Vector2(m_rb.velocity.x, jumpForce);
    }

    /// <summary>
    /// Flips the character's direction.
    /// </summary>
    private void flip() {
        transform.Rotate(0, 180, 0);
        m_isFacingRight = !m_isFacingRight;
    }
}