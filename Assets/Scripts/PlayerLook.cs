using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour {
    public Transform playerCamera;

    private float xRotation = 0f;
    private float mouseSensibility = 15f;
    private Vector2 mouseInput;
    private bool isActive = true;

    void Start() {
        
    }

    void Update() {
        if (isActive) {
            LookAround();
        }
    }

    public void OnLook(InputValue data) {
        if (!isActive) {
            mouseInput = Vector2.zero;
            return;
        }
        mouseInput = data.Get<Vector2>();
    }

    public void setActive (bool active) {
        isActive = active;
    }

    public void UpdateMouseInput(Vector2 newInput) {
        mouseInput = newInput;
    }

    public void LookAround() {
        float mouseX = mouseInput.x * mouseSensibility * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensibility * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
