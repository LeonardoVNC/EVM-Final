using UnityEngine;
using TMPro;

public class GameTimeUI : MonoBehaviour
{
    public GameTimeManager gameTimeManager;
    public TextMeshProUGUI timeText;

    void Update()
    {
        if (gameTimeManager != null && timeText != null)
        {
            timeText.text = gameTimeManager.GetFormattedTime();
        }
    }
}