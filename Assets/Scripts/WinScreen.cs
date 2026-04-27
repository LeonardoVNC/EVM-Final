using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour {
    private bool isClicked = false;
    public AudioClip clickClip;

    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Reiniciar() {
        if (isClicked) return;

        isClicked = true;
        GlobalAudioManager.Instance.PlayGlobalSound(clickClip);
        SceneManager.LoadScene("MainMenu");
    }
}
