using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputState {
    void OnPrimary();
    void OnSecondary();
    void OnInteract();
    void OnLook(InputValue value);
    void OnMove(InputValue value);
}
