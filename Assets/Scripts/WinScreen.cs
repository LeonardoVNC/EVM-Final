using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour {
    public void Reiniciar() {
        SceneManager.LoadScene("MainMenu");
    }
}
