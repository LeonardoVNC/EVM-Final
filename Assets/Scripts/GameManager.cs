using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public TextMeshProUGUI batteryText; 
    public Image consumptionImage;

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

        if (consumptionImage != null) {
            float usageLevel = (isSecPanelOn ? 2 : 0) + (isFlashlightOn ? 1 : 0) +  (isDoor1Closed ? 1 : 0) + (isDoor2Closed ? 1 : 0) + 1;

            consumptionImage.fillAmount = usageLevel / 6f;

            if (usageLevel <= 2) consumptionImage.color = Color.green;
            else if (usageLevel <= 4) consumptionImage.color = Color.yellow;
            else consumptionImage.color = Color.red;
        }
    }

    void PowerOut() {
        hasPower = false;
        if (batteryText != null) batteryText.text = "Batería: 0%";
        Debug.Log("Todo termino amigos");
    }

    public void SetPanelStatus(bool status) => isSecPanelOn = status;
    public void SetFlashlightStatus(bool status) => isFlashlightOn = status;
}
