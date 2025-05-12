using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("PoliceStation1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void goToSettings()
    {
        SceneManager.LoadScene("Settings");
    }
    public void goToMainmenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void goToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

}
