using UnityEngine;

public class CharacterController2D : MonoBehaviour {

    #region Public

    /// <summary>
    /// Instancia singleton de PlayerController
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

    // Start is called before the first frame update
    private void Start() {
        m_rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        m_isGrounded = Physics.CheckSphere(footPosition.position, footRadius, whatIsGround) &&
                   m_rb.velocity.y < 0.1f;

        horizontalMovement();
        verticalMovement();
    }

    private void horizontalMovement() {
        float xMove = Input.GetAxisRaw("Horizontal");
        m_rb.velocity = new Vector2(xMove * xSpeed, m_rb.velocity.y);
        if ((xMove < 0 && m_isFacingRight) || (xMove > 0 && !m_isFacingRight)) {
            flip();
        }
        if (m_isGrounded) {
            if (xMove != 0) {
            } else if (xMove == 0) {
            }
        }
    }

    private void verticalMovement() {
        if (m_isGrounded) {
            return;
        }
        if (m_rb.velocity.y >= 0.1f) {
        } else if (m_rb.velocity.y < -0.1f) {
        }
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetButtonDown("Jump")) {
            jump();
        }
    }

    private void jump() {
        if (!m_isGrounded) {
            return;
        }
        m_rb.velocity = new Vector2(m_rb.velocity.x, jumpForce);
    }

    private void flip() {
        transform.Rotate(0, 180, 0);
        m_isFacingRight = !m_isFacingRight;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(footPosition.position, footRadius);
    }
}