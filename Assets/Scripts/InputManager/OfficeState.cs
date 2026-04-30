using UnityEngine;
using UnityEngine.InputSystem;

public class OfficeState : IInputState {
    private InputManager ctx;

    public OfficeState(InputManager context) => ctx = context;

    public void OnPrimary() {
        if (ctx.flashlight != null) ctx.flashlight.Toggle();
    }

    public void OnSecondary() {
        ctx.cameraManager.ToggleMonitor();
        ctx.flashlight.ForceOff();
        GameManager.Instance.SetFlashlightStatus(false);
        ctx.SetState(new MonitorState(ctx));
    }

    public void OnInteract() {
        if (ctx.GetCurrentButton() != null) ctx.GetCurrentButton().Press();
    }

    public void OnLook(InputValue value) {
        ctx.playerLook.UpdateMouseInput(value.Get<Vector2>());
    }
}
