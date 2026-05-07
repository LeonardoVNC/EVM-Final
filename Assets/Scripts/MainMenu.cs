using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    private bool isStarting = false;
    public static int difficulty = 1;

    public AudioClip clickClip;
    public GameObject firstScreen;
    public GameObject secondScreen;

    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        showFirst(true);
    }

    // 1st Screen
    public void startPlay() {
        showFirst(false);
        clickSound();
    }

    public void quit() {
        clickSound();
        Application.Quit();
    }

    // 2nd Screen
    public void play(int level) {
        if (isStarting) return;

        isStarting = true;
        clickSound();
        difficulty = level;
        SceneManager.LoadScene("Night1");
    }
    
    public void goBack() {
        if (isStarting) return;
        showFirst(true);
        clickSound();
    }

    // Otros
    public void clickSound() => GlobalAudioManager.Instance.PlayGlobalSound(clickClip);
    public void showFirst(bool show) {
        firstScreen?.SetActive(show);
        secondScreen?.SetActive(!show);
    }
}