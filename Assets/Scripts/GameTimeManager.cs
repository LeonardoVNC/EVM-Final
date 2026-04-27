using UnityEngine;

public class GameTimeManager : MonoBehaviour {
    public float realSecondsPerGameHour = 90f;
    private float currentHour = 0f;
    private int lastHourTracked = 0;
    private bool timeStopped = false;

    public AudioClip clockClip;
    public AudioClip clockWinClip;

    public float CurrentHour => currentHour;

    void Update() {
        if (timeStopped || !GameManager.Instance.hasPower) return;

        currentHour += Time.deltaTime / realSecondsPerGameHour;
        
        int hourAsInt = Mathf.FloorToInt(currentHour);
        if (hourAsInt > lastHourTracked) {
            OnHourPassed(hourAsInt);
        }

        UIManager.Instance.UpdateTimeText(GetFormattedTime());
    }

    void OnHourPassed(int newHour) {
        lastHourTracked = newHour;

        if (newHour >= 6) {
            timeStopped = true;
            if (clockWinClip != null) GlobalAudioManager.Instance.PlayGlobalSound(clockWinClip);
            
            Invoke("NotifyWin", 4f); 
        } else {
            if (clockClip != null) GlobalAudioManager.Instance.PlayGlobalSound(clockClip);
        }
    }

    void NotifyWin() => GameManager.Instance.Win();

    public string GetFormattedTime() {
        int hour = Mathf.FloorToInt(currentHour);
        int displayHour = (hour == 0) ? 12 : (hour > 6 ? 6 : hour);
        return string.Format("{0} AM", displayHour);
    }
}
