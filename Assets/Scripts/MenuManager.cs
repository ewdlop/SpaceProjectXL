using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject settingsPanel; 

    public static bool isPaused; 

    void Start () {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1.0f;   // Resume game when level is loaded. 
    }

    void Update() { 
        if (Input.GetButtonDown("Cancel"))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    } 

    public void GoToStartMenu() {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0.0f;
        menuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void Restart()
    {
        GameController.playerScore = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
