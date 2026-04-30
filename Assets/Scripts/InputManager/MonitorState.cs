using UnityEngine;
using UnityEngine.InputSystem;

public class MonitorState : IInputState {
    private InputManager ctx;

    public MonitorState(InputManager context) => ctx = context;

    public void OnPrimary() {
        // Pass
    }

    public void OnSecondary() {
        ctx.cameraManager.ToggleMonitor();
        ctx.SetState(new OfficeState(ctx));
    }

    public void OnInteract() { 
        // Pass
    }

    public void OnLook(InputValue value) {
        ctx.playerLook.UpdateMouseInput(Vector2.zero);
    }
}
