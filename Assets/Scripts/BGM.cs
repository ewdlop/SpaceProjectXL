using UnityEngine;
using System.Collections;

public class BGM : MonoBehaviour {

	static bool isAudioOn = false;
	AudioSource audio; 

	// To keep BGM persistent when changing levels
	void Awake() {
		audio = GetComponent<AudioSource> ();
		if (!isAudioOn) { 
			audio.Play ();
			DontDestroyOnLoad (this.gameObject);
			isAudioOn = true;
		} else {
			audio.Stop ();
		}
	}

    void Update()
    {
        audio.volume = SoundController.bgmVolume * SoundController.masterVolume;
    }



}
