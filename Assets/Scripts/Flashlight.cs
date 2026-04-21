using UnityEngine;

public class Flashlight : MonoBehaviour {
    private Light lightComponent;
    private bool isOn = false;

    void Awake() {
        lightComponent = GetComponent<Light>();
        lightComponent.enabled = false;
    }

    public void Toggle() {
        isOn = !isOn;
        lightComponent.enabled = isOn;
        Debug.Log("Ayu" + isOn);
    }

    public void ForceOff() {
        isOn = false;
        lightComponent.enabled = false;
    }
}
