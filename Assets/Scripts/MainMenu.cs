using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void reintentar()
    {
        Debug.Log("REINTENTAR PRESIONADO");
        SceneManager.LoadScene("SampleScene");
    }

    public void quit()
    {
        Application.Quit();
    }
}