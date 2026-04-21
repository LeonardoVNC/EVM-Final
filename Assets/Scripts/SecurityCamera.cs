using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    private Quaternion initialRotation;
    private Camera cam;

    private float rotationRange = 10f;
    private float rotationSpeed = 0.2f;

    void Awake() {
        cam = GetComponent<Camera>();
        initialRotation = transform.localRotation;
    }

    void Update() {
        float angle = Mathf.Sin(Time.time * rotationSpeed) * rotationRange;
        transform.localRotation = initialRotation * Quaternion.Euler(0, angle, 0);
    }

    public void SetState(bool active) {
        if (cam != null) cam.enabled = active;
        this.enabled = active;
    }
}
