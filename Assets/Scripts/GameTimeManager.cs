using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimeManager : MonoBehaviour {
    public float realSecondsPerGameHour = 90f;
    private float currentHour = 0f;
    private bool gameWon = false;

    private int lastHourTracked = 0;
    public AudioClip clockClip;
    public AudioClip clockWinClip;

    public float CurrentHour => currentHour;

    void Update() {
        if (gameWon || !GameManager.Instance.hasPower) return;

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
            gameWon = true;
            PlayVictorySequence();
        } else {
            if (clockClip != null) {
                GlobalAudioManager.Instance.PlayGlobalSound(clockClip);
            }
        }
    }

    void PlayVictorySequence() {
        if (clockWinClip != null) {
            GlobalAudioManager.Instance.PlayGlobalSound(clockWinClip);
        }
        
        Invoke("LoadWinScene", 4f); 
    }

    void LoadWinScene() {
        SceneManager.LoadScene("WinScreen");
    }

    public string GetFormattedTime() {
        int hour = Mathf.FloorToInt(currentHour);
        int displayHour = (hour == 0) ? 12 : hour;
        
        if (hour >= 6) return "6 AM";
        
        return string.Format("{0} AM", displayHour);
    }
}
