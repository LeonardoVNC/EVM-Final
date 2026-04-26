using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimeManager : MonoBehaviour {
    public float realSecondsPerGameHour = 90f;
    private float currentHour = 0f;
    private bool gameWon = false;

    public float CurrentHour => currentHour;

    void Update() {
        if (gameWon || !GameManager.Instance.hasPower) return;

        currentHour += Time.deltaTime / realSecondsPerGameHour;
        UIManager.Instance.UpdateTimeText(GetFormattedTime());

        if (currentHour >= 6f) {
            gameWon = true;
            SceneManager.LoadScene("WinScreen"); 
        }
    }

    public string GetFormattedTime() {
        int hour = Mathf.FloorToInt(currentHour);
        int minutes = Mathf.FloorToInt((currentHour - hour) * 60f);
        int displayHour = (hour == 0) ? 12 : hour;
        return string.Format("{0}:{1:00} AM", displayHour, minutes);
    }
}
