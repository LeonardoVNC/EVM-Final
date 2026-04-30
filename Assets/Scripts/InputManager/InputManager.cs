using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IInputState {
    public static InputManager Instance { get; private set; }
    
    private IInputState currentState;

    public PlayerLook playerLook;
    public CameraManager cameraManager;
    public Flashlight flashlight;
    public Camera playerCamera;

    private DoorButton currentButton = null;

    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start() {
        SetState(new OfficeState(this));
    }

    void Update() {
        if (playerCamera == null) return;
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        DoorButton newButton = null;

        if (Physics.Raycast(ray, out hit, 20f)) {
            newButton = hit.collider.GetComponent<DoorButton>();
        }

        if (newButton != currentButton) {
            if (currentButton != null) currentButton.SetHighlight(false);
            if (newButton != null) newButton.SetHighlight(true);
            currentButton = newButton;
        }
    }

    public void SetState(IInputState newState) {
        currentState = newState;
    }

    public void OnPrimary() => currentState?.OnPrimary();
    public void OnSecondary() => currentState?.OnSecondary();
    public void OnInteract() => currentState?.OnInteract();
    public void OnLook(InputValue value) => currentState?.OnLook(value);
    public void OnMove(InputValue value) => currentState?.OnMove(value);


    public DoorButton GetCurrentButton() => currentButton;
}
