using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

	public GameObject StartPanel;  
	public GameObject InstructionsPanel;
	public GameObject CreditsPanel;

	// Use this for initialization
	void Start () {
		StartPanel.SetActive (true);		
		InstructionsPanel.SetActive (false);
		CreditsPanel.SetActive (false);
	}
	
	public void Instructions() { 
		StartPanel.SetActive (false);
		CreditsPanel.SetActive (false);
		InstructionsPanel.SetActive (true);

	}

	public void Credits() { 
		StartPanel.SetActive (false);
		InstructionsPanel.SetActive (false);
		CreditsPanel.SetActive (true);
	}

	public void Back() { 
		StartPanel.SetActive (true);		
		InstructionsPanel.SetActive (false);
		CreditsPanel.SetActive (false);
	}
}
