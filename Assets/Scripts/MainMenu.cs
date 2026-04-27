using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    private bool isStarting = false;

    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void play() {
        if (isStarting) return;

        isStarting = true;
        GlobalAudioManager.Instance.PlayClickSound();
        SceneManager.LoadScene("SampleScene");
    }

    public void quit() {
        GlobalAudioManager.Instance.PlayClickSound();
        Application.Quit();
    }
}