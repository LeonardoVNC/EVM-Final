using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour {
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI batteryText;
    public Image consumptionDisplay;
    public GameObject flashlightIcon;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI temporalText;

    public GameObject securityUI;
    public GameObject gameUI;

    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start() {
        StartCoroutine(TutorialSequence());
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

    IEnumerator ShowMessage(string message, float delay) {
        temporalText.text = message;
        yield return new WaitForSeconds(delay);
        temporalText.text = "";
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator TutorialSequence() {
        if (temporalText == null) yield break;

        yield return ShowMessage("Sobrevive hasta las 6:00 AM", 4f);
        yield return ShowMessage("Vigila las cámaras para mantenerte a salvo", 4f);
        yield return ShowMessage("Usa [ESPACIO] para abrir el monitor", 4f);
        yield return ShowMessage("Usa [CLICK] para encender la linterna", 4f);
        yield return ShowMessage("Usa [E] y apunta a los botones para cerrar las puertas", 4f);
        yield return ShowMessage("Buena suerte", 4f);
    }
}
