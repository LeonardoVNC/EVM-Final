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
    private bool blockInput = false;

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

    public void OnUpdate() {if (!blockInput) currentState?.OnUpdate();}
    public void OnPrimary() {if (!blockInput) currentState?.OnPrimary();}
    public void OnSecondary() {if (!blockInput) currentState?.OnSecondary();}
    public void OnInteract() {if (!blockInput) currentState?.OnInteract();}
    public void OnLook(InputValue value) {if (!blockInput) currentState?.OnLook(value);}
    public void OnMove(InputValue value) {if (!blockInput) currentState?.OnMove(value);}

    public void OnPause() => GameManager.Instance.OnPause();

    public DoorButton GetCurrentButton() => currentButton;
    public void SetCurrentButton(DoorButton newButton) => currentButton=newButton;
    public void SetBlockInput(bool block) => blockInput=block;
}
