using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {
    private List<SecurityCamera> floor1Cameras = new List<SecurityCamera>();
    private List<SecurityCamera> floor2Cameras = new List<SecurityCamera>();
    private List<SecurityCamera> currentActiveList;
    private bool isMonitorOpen = false;
    private int currentFloor = 1;

    public List<Sprite> panelBackgrounds;
    public GameObject buttonsFloor1Panel;
    public GameObject buttonsFloor2Panel;
    public Camera playerCamera;
    public RawImage staticRawImage;
    public VideoPlayer staticVideoPlayer;
    public AudioSource staticAudio;

    void Awake() {
        Transform f1 = transform.Find("1Floor");
        if (f1 != null) FillCameraList(f1, floor1Cameras);
        Transform f2 = transform.Find("2Floor");
        if (f2 != null) FillCameraList(f2, floor2Cameras);
        currentActiveList = floor1Cameras;
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

    void FillCameraList(Transform parent, List<SecurityCamera> list) {
        foreach (Transform child in parent) {
            SecurityCamera sCam = child.GetComponent<SecurityCamera>();
            if (sCam != null) list.Add(sCam);
        }
    }

    //Control de vista de seguridad
    public void ShowPlayerView() {
        foreach (var cam in floor1Cameras) cam.SetState(false);
        foreach (var cam in floor2Cameras) cam.SetState(false);
        playerCamera.enabled = true;
    }

    public void ToggleMonitor() {
        if (!GameManager.Instance.HasPower) return;

        isMonitorOpen = !isMonitorOpen;
        GameManager.Instance.SetPanelStatus(isMonitorOpen);

        if (isMonitorOpen) {
            SwitchToFloor(1);
            if (staticAudio != null) staticAudio.Play();
            if (staticVideoPlayer != null) staticVideoPlayer.Play();
        } else {
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

    //Ambientación
    private void RestartStaticEffects() {
        if (staticVideoPlayer != null && staticVideoPlayer.isPlaying) staticVideoPlayer.time = 0;
        if (staticAudio != null && staticAudio.isPlaying) staticAudio.time = 0;
    }

    private System.Collections.IEnumerator CameraFlashEffect() {
        if (staticRawImage != null) {
            staticRawImage.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.1f);
            staticRawImage.color = new Color(1, 1, 1, 0.65f);
        }
    }

    //Cambio de piso y cámara
    public void SwitchToCamera(int index) {
        if (index < 0 || index >= currentActiveList.Count) return;
        
        RestartStaticEffects();
        StartCoroutine(CameraFlashEffect());
        
        playerCamera.enabled = false;

        foreach (var cam in floor1Cameras) cam.SetState(false);
        foreach (var cam in floor2Cameras) cam.SetState(false);

        currentActiveList[index].SetState(true);
    }

    public void SwitchToFloor(int floor) {
        currentFloor = floor;
        currentActiveList = (floor == 1) ? floor1Cameras : floor2Cameras;

        if (buttonsFloor1Panel != null) buttonsFloor1Panel.SetActive(floor == 1);
        if (buttonsFloor2Panel != null) buttonsFloor2Panel.SetActive(floor == 2);

        int index = floor-1;
        if (index >= 0 && index < panelBackgrounds.Count)
            UIManager.Instance.SetSecurityPanelBackground(panelBackgrounds[index]);
        else
            Debug.Log("Te falto asignar el fondo para este piso moyai");
        
        SwitchToCamera(0);
    }
}
