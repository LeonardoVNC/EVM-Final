using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour {
    private bool isClicked = false;

    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Reiniciar() {
        if (isClicked) return;

        isClicked = true;
        GlobalAudioManager.Instance.PlayClickSound();
        SceneManager.LoadScene("MainMenu");
    }
}
