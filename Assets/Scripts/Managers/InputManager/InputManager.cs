using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IInputState {
    public static InputManager Instance { get; private set; }
    
    private IInputState currentState;

    public CameraManager cameraManager;
    public PlayerLook playerLook;
    public PlayerMovement playerMovement;
    public Camera playerCamera;
    public Flashlight flashlight;
    private DoorButton currentButton = null;

    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start() {
        SetState(new OfficeState(this));
    }

    void Update() {
        OnUpdate();
    }

    public void SetState(IInputState newState) {
        currentState = newState;
    }

    public void OnUpdate() => currentState?.OnUpdate();
    public void OnPrimary() => currentState?.OnPrimary();
    public void OnSecondary() => currentState?.OnSecondary();
    public void OnInteract() => currentState?.OnInteract();
    public void OnLook(InputValue value) => currentState?.OnLook(value);
    public void OnMove(InputValue value) => currentState?.OnMove(value);


    public DoorButton GetCurrentButton() => currentButton;
    public void SetCurrentButton(DoorButton newButton) => currentButton=newButton;
}
