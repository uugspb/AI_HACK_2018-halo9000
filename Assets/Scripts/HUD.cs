using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameMode { Play, Pause}

public class HUD : MonoBehaviour {

    public GameObject loseWindow;
    public GameObject winWindow;
    public GameObject pauseWindow;
    static public GameMode gameMode;
    public Text zombiesLeftText;
    static public int zombiesLeftValue;

    static public int ZombiesLeftValue
    {
        get
        {
            return zombiesLeftValue;
        }
        set
        {
            zombiesLeftValue = value;
            if (zombiesLeftValue < 0)
                zombiesLeftValue = 0;
        }
    }

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

    private void Start()
    {
        ZombiesLeftValue = GameObject.FindGameObjectsWithTag("Leming").Length / 2;
        UpdateZombiesLeftValue(ZombiesLeftValue);
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

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SceneManager.LoadScene(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SceneManager.LoadScene(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SceneManager.LoadScene(3);
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
        gameMode = GameMode.Play;
        Time.timeScale = 1f;
    }

    public void OnNextClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        gameMode = GameMode.Play;
        Time.timeScale = 1f;
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

    public void UpdateZombiesLeftValue(int value)
    {
        zombiesLeftText.text = "Zombies Left: " + value.ToString();
    }
}
