using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    private List<SecurityCamera> securityCameras = new List<SecurityCamera>();
    public Camera playerCamera;
    public GameObject monitorCanvasPanel;
    public GameObject screenImage;

    private bool isMonitorOpen = false;

    void Awake() {
        foreach (Transform child in transform) {
            SecurityCamera sCam = child.GetComponent<SecurityCamera>();
            if (sCam != null) {
                securityCameras.Add(sCam);
            }
        }
    }

    void Start() {
        ShowPlayerView();
        monitorCanvasPanel.SetActive(false);
        screenImage.SetActive(false);
    }

    void Update() {
        if (Keyboard.current.spaceKey.wasPressedThisFrame) {
            ToggleMonitor();
        }
    }

    public void ShowPlayerView() {
        foreach (var cam in securityCameras) {
            cam.SetState(false);
        }
        playerCamera.enabled = true;
    }

    public void SwitchToCamera(int index) {
        if (index < 0 || index >= securityCameras.Count) return;

        playerCamera.enabled = false;
        for (int i = 0; i < securityCameras.Count; i++) {
            securityCameras[i].SetState(i == index);
        }
    }

    public void ToggleMonitor() {
        isMonitorOpen = !isMonitorOpen;

        monitorCanvasPanel.SetActive(isMonitorOpen);
        screenImage.SetActive(isMonitorOpen);

        if (isMonitorOpen) {
            SwitchToCamera(0); 
            Cursor.lockState = CursorLockMode.None;
        } else {
            ShowPlayerView();
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = isMonitorOpen;
    }
}
