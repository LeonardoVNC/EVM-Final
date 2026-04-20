using UnityEngine;

using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public GameObject[] securityCameras;
    public GameObject playerCamera;

    void Start() {
        ShowPlayerView();
    }

    void Update() {
        //Test
        var keyboard = Keyboard.current;

        if (keyboard.digit1Key.wasPressedThisFrame) SwitchToCamera(0);
        if (keyboard.digit2Key.wasPressedThisFrame) SwitchToCamera(1);
        if (keyboard.digit3Key.wasPressedThisFrame) SwitchToCamera(2);
        if (keyboard.spaceKey.wasPressedThisFrame) ShowPlayerView();
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
}
