using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    private bool isStarting = false;
    public AudioClip clickClip;

    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void play() {
        if (isStarting) return;

        isStarting = true;
        GlobalAudioManager.Instance.PlayGlobalSound(clickClip);
        SceneManager.LoadScene("SampleScene");
    }

    public void quit() {
        GlobalAudioManager.Instance.PlayGlobalSound(clickClip);
        Application.Quit();
    }
}