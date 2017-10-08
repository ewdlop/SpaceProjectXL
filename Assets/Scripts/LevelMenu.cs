using UnityEngine;
using System.Collections;

public class LevelMenu : MonoBehaviour {

	public GameObject pausePanel; 
	public GameObject pauseButton;

	void Start () {
		pauseButton.SetActive (true);
		pausePanel.SetActive (false);
		Time.timeScale = 1.0f;							// Resume game when level is loaded. 
	}

	public void Title() {
		Application.LoadLevel (0);
	}

	public void Pause() { 
		Time.timeScale = 0.0f;
		pausePanel.SetActive (true);
		pauseButton.SetActive (false);
	}

	public void Resume() { 
		Time.timeScale = 1.0f;
		pausePanel.SetActive (false);
		pauseButton.SetActive (true);
	}

	public void RestartSingle() {
		Application.LoadLevel (1);
	}

	public void RestartTwo() { 
		Application.LoadLevel (2);
	}
}
