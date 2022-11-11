using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    public void goToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowOptions()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(3);
    }

   

    public void Quit()
    {
        Application.Quit();
    }
}
