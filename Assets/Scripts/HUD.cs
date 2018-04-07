using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour {

    public GameObject loseWindow;
    public GameObject winWindow;

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
}
