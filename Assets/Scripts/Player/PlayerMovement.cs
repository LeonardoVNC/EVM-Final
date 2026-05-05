using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private float movementSpeed = 12f;
    private Vector2 movementInput;
    private Rigidbody rb;
    private float jumpForce = 7.5f;
    private float groundCheckDistance = 3.2f;
    public LayerMask groundLayer;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
    }

    void FixedUpdate() {
        ApplyMovement();
    }

    public void SetMovementInput(Vector2 input) {
        movementInput = input;
    }

    private void ApplyMovement() {
        Vector3 moveDir = transform.right * movementInput.x + transform.forward * movementInput.y;

        rb.linearVelocity = new Vector3(moveDir.x * movementSpeed, rb.linearVelocity.y, moveDir.z * movementSpeed);
    }

    public void Jump() {
        if (IsGrounded()) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded() {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}
