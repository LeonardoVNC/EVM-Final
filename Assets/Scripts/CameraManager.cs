using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {
    private List<SecurityCamera> securityCameras = new List<SecurityCamera>();
    public Camera playerCamera;
    public PlayerLook playerLookScript;

    public RawImage staticRawImage;
    public VideoPlayer staticVideoPlayer;
    public AudioSource staticAudio;

    private bool isMonitorOpen = false;

    void Awake() {
        foreach (Transform child in transform) {
            SecurityCamera sCam = child.GetComponent<SecurityCamera>();
            if (sCam != null) securityCameras.Add(sCam);
        }
    }

    void Start() {
        ShowPlayerView();
        
        if (staticRawImage != null) {
            staticRawImage.gameObject.SetActive(false);
            staticRawImage.raycastTarget = false;
        }
        
        if (staticVideoPlayer != null) staticVideoPlayer.Prepare();
    }

    void Update() {
        if (isMonitorOpen && staticVideoPlayer != null && staticVideoPlayer.isPlaying) {
            if (staticRawImage != null) staticRawImage.texture = staticVideoPlayer.texture;
        }
    }

    public void ShowPlayerView() {
        foreach (var cam in securityCameras) cam.SetState(false);
        playerCamera.enabled = true;
    }

    public void SwitchToCamera(int index) {
        if (index < 0 || index >= securityCameras.Count) return;
        
        RestartStaticEffects();
        StartCoroutine(CameraFlashEffect());
        
        playerCamera.enabled = false;
        for (int i = 0; i < securityCameras.Count; i++) {
            securityCameras[i].SetState(i == index);
        }
    }

    public void ToggleMonitor() {
        if (!GameManager.Instance.hasPower) return;

        isMonitorOpen = !isMonitorOpen;

        GameManager.Instance.SetPanelStatus(isMonitorOpen);

        if (isMonitorOpen) {
            SwitchToCamera(0);
            if (staticAudio != null) staticAudio.Play();
            if (staticVideoPlayer != null) staticVideoPlayer.Play();
        }
        else {
            ShowPlayerView();
            if (staticAudio != null) staticAudio.Stop();
            if (staticVideoPlayer != null) staticVideoPlayer.Stop();
        }

        if (staticRawImage != null) staticRawImage.gameObject.SetActive(isMonitorOpen);

        Cursor.lockState = isMonitorOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isMonitorOpen;
    }

    public void ForceCloseMonitor() {
        isMonitorOpen = false;
        ShowPlayerView();
        if (staticAudio != null) staticAudio.Stop();
        if (staticVideoPlayer != null) staticVideoPlayer.Stop();
        if (staticRawImage != null) staticRawImage.gameObject.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void RestartStaticEffects() {
        if (staticVideoPlayer != null && staticVideoPlayer.isPlaying) staticVideoPlayer.time = 0;
        if (staticAudio != null && staticAudio.isPlaying) staticAudio.time = 0;
    }

    private System.Collections.IEnumerator CameraFlashEffect() {
        if (staticRawImage != null) {
            staticRawImage.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.1f);
            staticRawImage.color = new Color(1, 1, 1, 0.2f);
        }
    }
}
