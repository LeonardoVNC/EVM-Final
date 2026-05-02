using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public DoorController doorController;
    public Color normalColor = Color.red;
    public Color highlightColor = Color.yellow;
    public Color closedColor = Color.green;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
        
        UpdateColor(false);
    }

    public void SetHighlight(bool highlighted)
    {
        UpdateColor(highlighted);
    }

    void UpdateColor(bool highlighted)
    {
        if (rend == null) return;

        if (doorController != null && doorController.IsClosed())
            rend.material.color = closedColor;
        else
            rend.material.color = highlighted ? highlightColor : normalColor;
    }

    public void Press()
    {
        if (doorController != null)
        {
            doorController.ToggleDoor();
            UpdateColor(false);
        }
    }
}