using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    private bool isPause = false;
    private bool isClicked = false;
    private bool returnWithClick = false;
    public AudioClip clickClip;

    void Awake() {
        this.gameObject.SetActive(false);
    }

    public void SetPause(bool returnWithClick) {
        this.returnWithClick = returnWithClick;

        isPause = true;
        this.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isClicked = false;

        InputManager.Instance.SetBlockInput(true);
        Time.timeScale = 0f;
    }

    public void Continue () {
        if (isClicked) return;

        isClicked = true;
        GlobalAudioManager.Instance.PlayGlobalSound(clickClip);

        if (returnWithClick) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;;
        }

        InputManager.Instance.SetBlockInput(false);
        Time.timeScale = 1f;
        
        isPause = false;
        this.gameObject.SetActive(false);
    }

    public void Quit() {
        if (isClicked) return;

        isClicked = true;
        GlobalAudioManager.Instance.PlayGlobalSound(clickClip);
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu");
    }
}
