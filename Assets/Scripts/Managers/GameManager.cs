using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public Sprite[] consumptionSprites;
    private float batteryLevel = 100f;
    private float baseDrain = 0.1f;
    private float unitDrain = 0.25f;
    private bool isSecPanelOn = false;
    private bool isFlashlightOn = false;
    private bool isDoor1Closed = false;
    private bool isDoor2Closed = false;
    private bool isPoweroutActive = false;
    private bool hasPower = true;
    private bool isAlive = true;
    private bool[] callTriggered = new bool[4];
    private float introDuration = 5f;

    public AudioClip powerout;
    public AudioSource callAudioSource;
    public DoorController doorLeft;
    public DoorController doorRight;
    public PauseMenu pauseMenu;
    public AudioClip[] calls;
    public GameObject introCameraObj;
    public GameObject playerObj;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }

    void Start() {
        int difficulty = MainMenu.difficulty;

        if (difficulty == 1) {
            baseDrain = 0.06f;
            unitDrain = 0.12f;
        }else if (difficulty == 2) {
            baseDrain = 0.08f;
            unitDrain = 0.18f;
        }else if (difficulty == 3) {
            baseDrain = 0.1f;
            unitDrain = 0.24f;
        }
        StartCoroutine(StartIntroSequence());
    }

    void Update() {
        if (hasPower) {
            CalculateBattery();
            UpdateAtmosphere();
            SyncUI();
            CheckCallTriggers();
        }
    }

    void CalculateBattery() {
        int consumptionUnits = 0;
        if (isSecPanelOn) consumptionUnits += 2;
        if (isFlashlightOn) consumptionUnits += 1;
        if (isDoor1Closed) consumptionUnits += 1;
        if (isDoor2Closed) consumptionUnits += 1;

        float currentDrain = (isPoweroutActive? 0:baseDrain) + (consumptionUnits * unitDrain);
        batteryLevel -= currentDrain * Time.deltaTime;
        if (batteryLevel <= 0) {
            batteryLevel = 0;
            PowerOut();
        }
    }
    
    void PowerOut() {
        if (isPoweroutActive) {
            hasPower = false;
            isFlashlightOn = false;
            return;
        }

        isPoweroutActive = true;
        batteryLevel = 25f;
        InputManager.Instance.cameraManager.ForceCloseMonitor();
        SetPanelStatus(false);
        isDoor1Closed = false;
        isDoor2Closed = false;
        if (doorLeft != null) doorLeft.ForceOpen();
        if (doorRight != null) doorRight.ForceOpen();

        InputManager.Instance.SetState(new BlackoutState(InputManager.Instance));
        UIManager.Instance.DisableAllUI();
        GlobalAudioManager.Instance.PlayGlobalSound(powerout);
        PlayCall(3);
    }

    // Llamadas
    void CheckCallTriggers() {
        if (batteryLevel <= 60f && !callTriggered[1]) PlayCall(1);
        if (batteryLevel <= 20f && !callTriggered[2]) PlayCall(2);
    }

    void PlayCall(int index) {
        if (calls == null || index >= calls.Length || calls[index] == null) return;
    
        callTriggered[index] = true;
        callAudioSource.clip = calls[index];
        callAudioSource.Play();
    }

    public void StopCall() {
        if (callAudioSource != null && callAudioSource.isPlaying) {
            callAudioSource.Stop();
        }
    }

    // Control de la UI
    void SyncUI() {
        float usage = (isSecPanelOn ? 2 : 0) + (isFlashlightOn ? 1 : 0) + (isDoor1Closed ? 1 : 0) + (isDoor2Closed ? 1 : 0);
        int spriteIndex = Mathf.Clamp((int)usage, 0, consumptionSprites.Length - 1);
        
        UIManager.Instance.UpdateBatteryUI(batteryLevel, consumptionSprites[spriteIndex]);
    }

    void UpdateAtmosphere() {
        if (FogManager.Instance == null) return;

        if (isFlashlightOn)
            FogManager.Instance.ChangeState(FogManager.FogState.Flashlight);
        else if (isSecPanelOn)
            FogManager.Instance.ChangeState(FogManager.FogState.Camera);
        else if (isPoweroutActive) 
            FogManager.Instance?.ChangeState(FogManager.FogState.PowerOut);
        else
            FogManager.Instance.ChangeState(FogManager.FogState.Default);
    }

    public void SetPanelStatus(bool status) {
        isSecPanelOn = status;
        UIManager.Instance.SetSecurityPanelActive(status);
    }

    public void SetFlashlightStatus(bool status) => isFlashlightOn = status;

    // Navegación entre scenes
    public void Win() {
        GoToWinScreen();
    }

    public void GoToWinScreen() {
        StopCall();
        Instance = null;
        SceneManager.LoadScene("WinScreen");
    }

    public void GoToGameOverScreen() {
        StopCall();
        Instance = null;
        SceneManager.LoadScene("GameOverScreen");
    }

    // Pausa
    public void OnPause() {
        if (isAlive) pauseMenu.SetPause(returnWithClick);
    }

    private bool returnWithClick => isSecPanelOn;

    // Introduccion
    IEnumerator StartIntroSequence() {
        if (introCameraObj != null) introCameraObj.SetActive(true);
        if (playerObj != null) playerObj.SetActive(false);
        InputManager.Instance.SetIntroMode(true);
        FogManager.Instance.ChangeState(FogManager.FogState.Intro);

        yield return new WaitForSeconds(introDuration);

        if (introCameraObj != null) introCameraObj.SetActive(false);
        if (playerObj != null) playerObj.SetActive(true);
    
        InputManager.Instance.SetIntroMode(false); 
        FogManager.Instance.ChangeState(FogManager.FogState.Default);
    
        PlayCall(0); 
    }

    // Getters
    public float BatteryLevel => batteryLevel;
    public bool HasPower => hasPower;
    public bool IsPoweroutActive => isPoweroutActive;
    // Setters
    public void ToogleDoor1(bool closed) => isDoor1Closed = closed;
    public void ToogleDoor2(bool closed) => isDoor2Closed = closed;
    public void SetAlive(bool alive) => isAlive = alive;
}