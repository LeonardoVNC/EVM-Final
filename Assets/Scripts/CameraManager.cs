using UnityEngine;
using UnityEngine.UI;

using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public GameObject[] securityCameras;
    public GameObject playerCamera;
    public GameObject monitorCanvasPanel;
    public GameObject screenImage;

    private bool isMonitorOpen = false;

    void Start() {
        ShowPlayerView();
        monitorCanvasPanel.SetActive(false);
        screenImage.SetActive(false);
    }

    void Update() {
        //Test

        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            ToggleMonitor();
        }
    }

    public void ShowPlayerView() {
        foreach (GameObject cam in securityCameras) {
            cam.SetActive(false);
        }
        playerCamera.SetActive(true);
    }

    public void SwitchToCamera(int index) {
        if (index < 0 || index >= securityCameras.Length) return;

        playerCamera.SetActive(false);
        foreach (GameObject cam in securityCameras) {
            cam.SetActive(false);
        }
        securityCameras[index].SetActive(true);
    }

    public void ToggleMonitor() {
        isMonitorOpen = !isMonitorOpen;

        monitorCanvasPanel.SetActive(isMonitorOpen);
        screenImage.SetActive(isMonitorOpen);

        if (isMonitorOpen) {
            SwitchToCamera(0); 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            ShowPlayerView();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
