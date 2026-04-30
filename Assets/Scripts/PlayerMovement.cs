using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float movementSpeed = 10f;
    private Vector2 movementInput;
    private Rigidbody rb;

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
}
