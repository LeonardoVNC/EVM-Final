using UnityEngine;
using UnityEngine.Events;

public class GameTimeManager : MonoBehaviour
{
   
    public float realSecondsPerGameHour = 90f; 

    public UnityEvent onSixAM; 

    private float currentHour = 0f;   
    private bool gameWon = false;

    public float CurrentHour => currentHour;

    void Update()
    {
        if (gameWon) return;

        currentHour += Time.deltaTime / realSecondsPerGameHour;

        if (currentHour >= 6f)
        {
            currentHour = 6f;
            gameWon = true;
            onSixAM?.Invoke();
            Debug.Log("¡Sobreviviste! Son las 6:00 AM");
        }
    }

    
    public string GetFormattedTime()
    {
        int hour = Mathf.FloorToInt(currentHour);
        int minutes = Mathf.FloorToInt((currentHour - hour) * 60f);

        int displayHour = (hour == 0) ? 12 : hour;
        return string.Format("{0}:{1:00} AM", displayHour, minutes);
    }
}