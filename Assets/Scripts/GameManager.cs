using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public float batteryLevel = 100f;
    public float baseDrain = 0.1f;
    public float unitDrain = 0.5f;
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
        }
    }

    void CalculateBattery() {
        int activeUnits = 0;
        if (isSecPanelOn) activeUnits+=2;
        if (isFlashlightOn) activeUnits+=1;
        if (isDoor1Closed) activeUnits+=1;
        if (isDoor2Closed) activeUnits+=1;

        float currentDrain = baseDrain + (activeUnits * unitDrain);
        batteryLevel -= currentDrain * Time.deltaTime;

        Debug.Log("Bateria:"+batteryLevel);
        Debug.Log("Consumo:"+currentDrain);

        if (batteryLevel <= 0) {
            batteryLevel = 0;
            PowerOut();
        }
    }

    void PowerOut() {
        hasPower = false;
        Debug.Log("Todo termino amigos");
    }

    public void SetPanelStatus(bool status) => isSecPanelOn = status;
    public void SetFlashlightStatus(bool status) => isFlashlightOn = status;
}
