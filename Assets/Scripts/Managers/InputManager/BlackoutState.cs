using UnityEngine;
using UnityEngine.InputSystem;

public class BlackoutState : IInputState {
    private InputManager ctx;

    public BlackoutState(InputManager context) => ctx = context;

    public void OnUpdate() {
        //Pass
    }

    public void OnPrimary() {
        if (ctx.flashlight != null) ctx.flashlight.Toggle();
    }

    public void OnSecondary() {
        // Pass
    }

    public void OnInteract() {
        // Pass
    }

    public void OnLook(InputValue value) {
        ctx.playerLook.UpdateMouseInput(value.Get<Vector2>());
    }

    public void OnMove(InputValue value) {
        ctx.playerMovement.SetMovementInput(value.Get<Vector2>());
    }
}

