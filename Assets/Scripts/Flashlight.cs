using UnityEngine;

public class Flashlight : MonoBehaviour {
    private Light lightComponent;
    private bool isOn = false;

    void Awake() {
        lightComponent = GetComponent<Light>();
        lightComponent.enabled = false;
    }

    public void Toggle() {
        if (!GameManager.Instance.hasPower) return;

        isOn = !isOn;
        lightComponent.enabled = isOn;
        GameManager.Instance.SetFlashlightStatus(isOn);
    }

    public void ForceOff() {
        isOn = false;
        lightComponent.enabled = false;
        GameManager.Instance.SetFlashlightStatus(false);
    }
}
