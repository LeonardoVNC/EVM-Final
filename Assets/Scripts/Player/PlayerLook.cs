using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour {
    public Transform playerCamera;

    private float xRotation = 0f;
    private float mouseSensibility = 0.16f;
    private Vector2 mouseInput;
    private bool isActive = true;

    void Update() {
        if (isActive) {
            LookAround();
        }
    }

    public void setActive (bool active) {
        isActive = active;
    }

    public void UpdateMouseInput(Vector2 newInput) {
        mouseInput = newInput;
    }

    public void LookAround() {
        float mouseX = mouseInput.x * mouseSensibility;
        float mouseY = mouseInput.y * mouseSensibility;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
