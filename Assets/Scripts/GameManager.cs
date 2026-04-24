using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public TextMeshProUGUI batteryText;
    public Image consumptionDisplay;
    public Sprite[] consumptionSprites;

    public float batteryLevel = 100f;
    public float baseDrain = 0.1f;
    public float unitDrain = 0.25f;
    public bool hasPower = true;

    public bool isSecPanelOn = false;
    public bool isFlashlightOn = false;
    public bool isDoor1Closed = false;
    public bool isDoor2Closed = false;

    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update() {
        if (hasPower) {
            CalculateBattery();
            UpdateUI();
            UpdateAtmosphere();
        }
    }

    void CalculateBattery() {
        int consumptionUnits = 0;
        
        if (isSecPanelOn) consumptionUnits += 2;
        if (isFlashlightOn) consumptionUnits += 1;
        if (isDoor1Closed) consumptionUnits += 1;
        if (isDoor2Closed) consumptionUnits += 1;

        float currentDrain = baseDrain + (consumptionUnits * unitDrain);
        batteryLevel -= currentDrain * Time.deltaTime;
        if (batteryLevel <= 0) {
            batteryLevel = 0;
            PowerOut();
        }
    }

    void UpdateUI() {
        if (batteryText != null) {
            batteryText.text = "Batería: " + Mathf.CeilToInt(batteryLevel) + "%";
        }

        if (consumptionDisplay != null && consumptionSprites.Length > 0) {
            float usageLevel = (isSecPanelOn ? 2 : 0) + (isFlashlightOn ? 1 : 0) + (isDoor1Closed ? 1 : 0) + (isDoor2Closed ? 1 : 0);
            int spriteIndex = Mathf.Clamp((int)usageLevel, 0, consumptionSprites.Length - 1);
            consumptionDisplay.sprite = consumptionSprites[spriteIndex];
        }
    }

    void UpdateAtmosphere() {
        if (FogManager.Instance == null) return;

        if (isSecPanelOn) {
            FogManager.Instance.ChangeState(FogManager.FogState.Camera);
        } 
        else if (isFlashlightOn) {
            FogManager.Instance.ChangeState(FogManager.FogState.Flashlight);
        } 
        else {
            FogManager.Instance.ChangeState(FogManager.FogState.Default);
        }
    }

    void PowerOut() {
        hasPower = false;
        if (batteryText != null) batteryText.text = "Batería: 0%";
        FogManager.Instance.ChangeState(FogManager.FogState.PowerOut);
        GoToGameOverScreen();
    }

    public void SetPanelStatus(bool status) => isSecPanelOn = status;
    public void SetFlashlightStatus(bool status) => isFlashlightOn = status;

    public void GoToWinScreen() {
        SceneManager.LoadScene("WinScreen");
    }

    public void GoToGameOverScreen() {
        SceneManager.LoadScene("GameOverScreen");
    }
}
