using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public Sprite[] consumptionSprites;
    public float batteryLevel = 100f;
    private float baseDrain = 0.1f;
    private float unitDrain = 0.25f;
    public bool hasPower = true;
    public AudioClip powerout;
    private bool isPoweroutActive = false;

    private bool isSecPanelOn = false;
    private bool isFlashlightOn = false;
    public bool isDoor1Closed = false;
    public bool isDoor2Closed = false;
    public DoorController doorLeft;
    public DoorController doorRight;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
    }

    void Update() {
        if (hasPower) {
            CalculateBattery();
            UpdateAtmosphere();
            SyncUI();
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
            SetFlashlightStatus(false);
            return;
        }

        UIManager.Instance.ShowBlackoutMessages();
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

    public void SetFlashlightStatus(bool status) {
        isFlashlightOn = status;
        UIManager.Instance.SetFlashlightIcon(status);
    }

    // Navegación entre scenes
    public void Win() {
        GoToWinScreen();
    }

    public void GoToWinScreen() {
        Instance = null;
        SceneManager.LoadScene("WinScreen");
    }

    public void GoToGameOverScreen() {
        Instance = null;
        SceneManager.LoadScene("GameOverScreen");
    }
}