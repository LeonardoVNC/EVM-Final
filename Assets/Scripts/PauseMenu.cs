using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    private bool isClicked = false;
    private bool returnWithClick = false;
    public AudioClip clickClip;

    void Start() {
        this.gameObject.SetActive(false);
    }

    public void SetPause(bool returnWithClick) {
        this.returnWithClick = returnWithClick;

        this.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isClicked = false;

        //Aqui control del tiempo?? creo que el delta time se pone a 000000000000000000000000000000 xdxdx o como sea dx, habra metodo?
        InputManager.Instance.SetBlockInput(true);
        Time.timeScale = 0f;
    }

    public void Continue () {
        if (isClicked) return;

        isClicked = true;
        GlobalAudioManager.Instance.PlayGlobalSound(clickClip);

        if (returnWithClick) {
            //En vano creo, pero porsia
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;;
        }

        //Devolver el tiempo a la normalidad
        InputManager.Instance.SetBlockInput(false);
        Time.timeScale = 1f;
        
        this.gameObject.SetActive(false);
    }

    public void Quit() {
        if (isClicked) return;

        isClicked = true;
        GlobalAudioManager.Instance.PlayGlobalSound(clickClip);
        SceneManager.LoadScene("MainMenu");
    }
}
