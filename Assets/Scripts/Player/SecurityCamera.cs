using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    private Quaternion initialRotation;
    private Camera cam;

    private float rotationRange = 10f;
    private float rotationSpeed = 0.2f;

    private float localTime = 0f;
    private bool isActive = false;

    void Awake() {
        cam = GetComponent<Camera>();
        initialRotation = transform.localRotation;
        
        if (cam != null) cam.enabled = false;
        this.enabled = false; 
    }

    void Update() {
        if (isActive) {
            localTime += Time.deltaTime * rotationSpeed;
            
            float angle = Mathf.Sin(localTime) * rotationRange;
            transform.localRotation = initialRotation * Quaternion.Euler(0, angle, 0);
        } else {
            this.enabled = false;
        }
    }

    public void SetState(bool active) {
        isActive = active;
        if (cam != null) cam.enabled = active;
        
        this.enabled = true; 
        
        if(active) localTime = 0f; 
    }
}
