using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void play() {
        SceneManager.LoadScene("SampleScene");
    }

    public void quit() {
        Application.Quit();
    }
}