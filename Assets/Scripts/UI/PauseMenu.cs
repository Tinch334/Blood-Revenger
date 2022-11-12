using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public static bool isPaused = false;
    public static KeyCode pauseKey = KeyCode.Escape;

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
