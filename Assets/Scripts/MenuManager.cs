using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject subPausePanel;
    public static bool isPaused;

    void Start () {
        pausePanel.SetActive(false);
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

    public void GoToStartMenu()
    {
        if (FindObjectOfType<EnemyBoss>() != null)
        {
            FindObjectOfType<BGM>().GetComponent<BGM>().SwapBGM(GameAudio.normal);
        }
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        subPausePanel.SetActive(true);
        settingsPanel.SetActive(false);
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
        subPausePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
    }

    public void Restart()
    {
        GameController.playerScore = 0;
        if (FindObjectOfType<EnemyBoss>() != null)
        {
            FindObjectOfType<BGM>().GetComponent<BGM>().SwapBGM(GameAudio.normal);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
