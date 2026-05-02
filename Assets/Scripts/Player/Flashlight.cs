using UnityEngine;

public class Flashlight : MonoBehaviour {
    private Light lightComponent;
    private bool isOn = false;

    public float thresholdStartTicker = 15f; 
    private float tickerTimer = 0f;
    private float tickerSpeed = 0.07f;
    public AudioClip flickerSound;

    void Awake() {
        lightComponent = GetComponent<Light>();
        lightComponent.enabled = false;
    }

    void Update() {
        if (isOn) {
            HandleTicker();
        }
    }

    void HandleTicker() {
        float currentBattery = GameManager.Instance.BatteryLevel;

        if (currentBattery > thresholdStartTicker) {
            if (!lightComponent.enabled) SetLightState(true);
            return;
        }

        if (currentBattery <= 0) {
            ForceOff();
            return;
        }

        tickerTimer += Time.deltaTime;
        if (tickerTimer >= tickerSpeed) {
            tickerTimer = 0;

            float failureChance = 1f - (currentBattery / thresholdStartTicker);
            bool shouldBeOn = Random.value > failureChance;
            
            if (lightComponent.enabled != shouldBeOn) {
                SetLightState(shouldBeOn);
            }
        }
    }

    private void SetLightState(bool active) {
        if (isOn && !active && flickerSound != null) {
            float randomAudio = Random.Range(1.00f, 11.00f);

            GlobalAudioManager.Instance.PlaySoundSegment(flickerSound, randomAudio, 0.15f, 0.75f);
        }
        lightComponent.enabled = active;
        GameManager.Instance.SetFlashlightStatus(active);
    }

    public void Toggle() {
        if (GameManager.Instance.BatteryLevel <= 0) return;

        isOn = !isOn;
        SetLightState(isOn);
    }

    public void ForceOff() {
        isOn = false;
        SetLightState(false);
    }
}
