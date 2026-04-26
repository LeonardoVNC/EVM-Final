using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerLook playerLook;
    public CameraManager cameraManager;
    public Flashlight flashlight;
    public Camera playerCamera;

    private bool isMonitorOpen = false;
    private DoorButton currentButton = null;

    void Update()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position,
                          playerCamera.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red);
        RaycastHit hit;
        DoorButton newButton = null;


        if (Physics.Raycast(ray, out hit, 20f))
        {
            Debug.Log("Golpeó: " + hit.collider.gameObject.name);
            newButton = hit.collider.GetComponent<DoorButton>();
        }

        if (newButton != currentButton)
        {
            if (currentButton != null) currentButton.SetHighlight(false);
            if (newButton != null) newButton.SetHighlight(true);
            currentButton = newButton;
        }
    }

    public void OnLook(InputValue value)
    {
        if (!isMonitorOpen && playerLook != null)
            playerLook.UpdateMouseInput(value.Get<Vector2>());
        else if (playerLook != null)
            playerLook.UpdateMouseInput(Vector2.zero);
    }

    public void OnTooglePanel()
    {
        if (cameraManager != null)
        {
            cameraManager.ToggleMonitor();
            isMonitorOpen = !isMonitorOpen;
            GameManager.Instance.SetPanelStatus(isMonitorOpen);
            if (isMonitorOpen && flashlight != null)
            {
                flashlight.ForceOff();
                GameManager.Instance.SetFlashlightStatus(false);
            }
        }
    }

    public void OnAttack()
    {
        if (!isMonitorOpen && flashlight != null)
            flashlight.Toggle();
    }

    public void OnInteract()
    {
        Debug.Log("OnInteract llamado! currentButton: " + currentButton);
        if (currentButton != null)
            currentButton.Press();
    }
}