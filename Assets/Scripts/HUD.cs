using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode { Play, Pause}

public class HUD : MonoBehaviour {

    public GameObject loseWindow;
    public GameObject winWindow;
    public GameObject pauseWindow;
    static public GameMode gameMode;

    static private HUD instance;

    static public HUD Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        gameMode = GameMode.Play;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameMode == GameMode.Play)
            {
                ShowPauseWindow();
            }
            else
            {
                HidePauseWindow();
            }
        }
    }



    public void ShowLoseWindow()
    {
        loseWindow.GetComponent<Animator>().SetBool("windowEnabled", true);
    }

    public void HideLoseWindow()
    {
        loseWindow.GetComponent<Animator>().SetBool("windowEnabled", false);
    }

    public void ShowWinWindow()
    {
        winWindow.GetComponent<Animator>().SetBool("windowEnabled", true);
    }

    public void HideWinWindow()
    {
        winWindow.GetComponent<Animator>().SetBool("windowEnabled", false);
    }

    public void OnRestartClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  
    }

    public void OnNextClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnResumeClick()
    {
        HidePauseWindow();
    }

    public void ShowPauseWindow()
    {
        pauseWindow.GetComponent<Animator>().SetBool("windowEnabled", true);
        gameMode = GameMode.Pause;
        Time.timeScale = 0;
    }

    public void HidePauseWindow()
    {
        pauseWindow.GetComponent<Animator>().SetBool("windowEnabled", false);
        gameMode = GameMode.Play;
        Time.timeScale = 1f;
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
