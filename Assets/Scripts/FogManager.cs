using UnityEngine;

public class FogManager : MonoBehaviour {
    public static FogManager Instance { get; private set; }

    public enum FogState { Default, Flashlight, Camera, PowerOut }
    private FogState currentState = FogState.Default;

    private float defaultDensity = 0.088f;
    private float flashlightDensity = 0.042f;
    private float cameraDensity = 0.026f;
    private float powerOutDensity = 0.15f;

    private float transitionSpeed = 2.9f;
    private float targetDensity;

    void Awake() {
        if (Instance == null) Instance = this;
        targetDensity = defaultDensity;
    }

    void Update() {
        RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetDensity, Time.deltaTime * transitionSpeed);
    }

    public void ChangeState(FogState newState) {
        currentState = newState;

        switch (currentState) {
            case FogState.Default:
                targetDensity = defaultDensity;
                break;
            case FogState.Flashlight:
                targetDensity = flashlightDensity;
                break;
            case FogState.Camera:
                targetDensity = cameraDensity;
                break;
            case FogState.PowerOut:
                targetDensity = powerOutDensity;
                break;
        }
    }
}
