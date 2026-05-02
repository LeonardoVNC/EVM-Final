using UnityEngine;
using UnityEngine.InputSystem;

public class OfficeState : IInputState {
    private InputManager ctx;

    public OfficeState(InputManager context) => ctx = context;

    public void OnUpdate() {
        if (ctx.playerCamera == null) return;
        Ray ray = new Ray(ctx.playerCamera.transform.position, ctx.playerCamera.transform.forward);
        RaycastHit hit;
        DoorButton newButton = null;
        DoorButton currentButton = ctx.GetCurrentButton();

        if (Physics.Raycast(ray, out hit, 20f)) {
            newButton = hit.collider.GetComponent<DoorButton>();
        }

        if (newButton != currentButton) {
            if (currentButton != null) currentButton.SetHighlight(false);
            if (newButton != null) newButton.SetHighlight(true);
            ctx.SetCurrentButton(newButton);
        }
    }

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

    public void OnMove(InputValue value) {
        // Pass
    }
}
