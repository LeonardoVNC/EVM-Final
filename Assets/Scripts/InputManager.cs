using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    public PlayerLook playerLook;
    public CameraManager cameraManager;

    private bool isMonitorOpen = false;

    public void OnLook(InputValue value) {
        if (!isMonitorOpen && playerLook != null) {
            playerLook.UpdateMouseInput(value.Get<Vector2>());
        } else if (playerLook != null) {
            playerLook.UpdateMouseInput(Vector2.zero);
        }
    }

    public void OnTooglePanel() {
        if (cameraManager != null) {
            cameraManager.ToggleMonitor();
            isMonitorOpen = !isMonitorOpen;
        }
    }
}
