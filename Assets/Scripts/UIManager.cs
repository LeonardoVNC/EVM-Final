using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI batteryText;
    public Image consumptionDisplay;
    public GameObject flashlightIcon;
    public TextMeshProUGUI timeText;

    public GameObject securityUI;
    public GameObject gameUI;

    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateBatteryUI(float battery, Sprite consumptionSprite) {
        if (batteryText != null) batteryText.text = "Batería: " + Mathf.CeilToInt(battery) + "%";
        if (consumptionDisplay != null) consumptionDisplay.sprite = consumptionSprite;
    }

    public void SetFlashlightIcon(bool active) => flashlightIcon?.SetActive(active);

    public void UpdateTimeText(string formattedTime) {
        if (timeText != null) timeText.text = formattedTime;
    }

    public void SetSecurityPanelActive(bool active) => securityUI?.SetActive(active);

    public void DisableAllUI() {
        securityUI?.SetActive(false);
        gameUI?.SetActive(false);
    }
}
