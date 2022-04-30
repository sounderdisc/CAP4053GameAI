using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] PlayerControls controls;
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Menues.pause.started += ctx => Debug.Log("input detected");
    }

    public void wraper()
    {
        Debug.Log("in wrapper");
        if (GameIsPaused)
        {
            Debug.Log("in first if");
            Resume();
        }
        else
        {
            Debug.Log("in second if");
            Pause();
        }
    }
    public void Resume()
    {
        Debug.Log("in Resume()");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        Debug.Log("in pause");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
