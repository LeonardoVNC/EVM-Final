using UnityEngine;
using UnityEngine.InputSystem;

public class OfficeState : IInputState {
    private InputManager ctx;

    public OfficeState(InputManager context) => ctx = context;

    public void OnPrimary() {
        if (ctx.flashlight != null) ctx.flashlight.Toggle();
    }

    public void OnSecondary() {
        //TEST temporal wiw
        //ctx.cameraManager.ToggleMonitor();
        //ctx.flashlight.ForceOff();
        //GameManager.Instance.SetFlashlightStatus(false);
        //ctx.SetState(new MonitorState(ctx));
        ctx.SetState(new BlackoutState(ctx));
    }

    public void OnInteract() {
        if (ctx.GetCurrentButton() != null) ctx.GetCurrentButton().Press();
    }

    public void OnLook(InputValue value) {
        ctx.playerLook.UpdateMouseInput(value.Get<Vector2>());
    }

    public void OnMove(InputValue value) {
        // Pass
    }
}
