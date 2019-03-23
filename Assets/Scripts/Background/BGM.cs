using UnityEngine;
using System.Collections;

public enum GameAudio
{
    normal,
    bossTransition,
    bossBattle
}

public class BGM : MonoBehaviour {

    // TODO combine this with the SoundController
    public AudioClip normalBGMClip;
    public AudioClip bossTransitionClip;
    public AudioClip[] bossBattleClips;
    public AudioClip bossBattleClip;

    static bool isAudioOn;
    static BGM instance;
    
	new AudioSource audio;

    private EnemyBoss boss;

	// To keep BGM persistent when changing levels
	void Awake()
    {
        audio = GetComponent<AudioSource>();

        if (instance == null) {
            instance = this;            
            audio.Play ();
			DontDestroyOnLoad (gameObject);
			isAudioOn = true;            
        } else if (instance != this) {
			audio.Stop ();
            Destroy(gameObject);
		}       
       
	}

    public void SwapBGM(GameAudio selection)
    {
        switch (selection)
        {
            case GameAudio.normal:             
                audio.clip = normalBGMClip;
                break;
            case GameAudio.bossTransition:
                audio.clip = bossTransitionClip;
                break;
            case GameAudio.bossBattle:                
                audio.clip = bossBattleClips[GameController.stage];
                break;
            default:
                audio.clip = normalBGMClip;
                break;                       
        }

        audio.Play();
    }

    private void Update()
    {
        if(audio!=null)
        {
            audio.volume = SoundController.bgmVolume;
        }
    }
}
