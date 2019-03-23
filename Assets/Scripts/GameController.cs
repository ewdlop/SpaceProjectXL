using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Difficulty
{
    Easy, Medium, Hard
}

[System.Serializable]
public class ShipSprite
{
    public Sprite Icon;
    
    public ShipSprite(Sprite Icon)
    {
        this.Icon = Icon;
    }
}

public class GameController : MonoBehaviour {

    public static GameController instance;
    public static int playerScore;
    public static int playerHighScore;
    private static float gameTime;
    public bool isMenu;

    public static bool skipToBoss = true;

    [Header("UI Settings")]
    public Text difficultyText;
    public Text scoreText;
    public Text highScoreText;
    public Text retryText;
    public Text livesText;
    public Text timeText;
    public Text finalScoreText;
    public Text finalTimeText;

    private PlayVideo playVideo;

    [Header("Gameplay Settings")]
    public Difficulty debugDifficulty;   // Just for setting difficulty via the inspector for debugging
    public static Difficulty difficulty; // The static difficulty var referenced by other scripts
    public int difficultyInt;  // TODO, can remove this and just use the enum
    public float scrollSpeed = -5.0f;
    public Color hitColor; // Hit flash color
    public bool isGameOver;

    [Header("Start Menu")]
    public GameObject startGameMenu;
    public Text startGameMenuDiffucltyText;

    [Header("PlayerShipSprites")]
    public static int spriteInt;
    public List<ShipSprite> sprites;
    public Image shipSpriteImage;
    public GameObject shipSelectionPanel;
    public static Sprite inGameSprite;

    [Header("PlayerShipWeapon")]
    public static int mainWeaponInt;
    public static int supportWeaponInt;
    public Sprite[] mainWeaponSprites;
    public Sprite[] supportWeaponSprites;
    public Image mainWeaponImage;
    public Image supportWeaponImage;
    public GameObject weaponSelectionPanel;

    [Header("ArmoryMenu")]
    public GameObject armoryPanel;

    [Header("AchievementMenu")]
    public GameObject achieveMentPanel;

    [Header("SettingMenu")]
    public GameObject settingPanel;

    public GameObject winPanel;

    [Header("StageSelection")]
    public static int stage;
    public Image loadImage;

    private PlayerController player;

    public void ToggleSkipToBoss()
    {
        skipToBoss = !skipToBoss;
    }

    void Start() {
        gameTime = 0.0f;

        player = FindObjectOfType<PlayerController>();

        if (isMenu)
        {
            mainWeaponImage.sprite = mainWeaponSprites[mainWeaponInt];
            supportWeaponImage.sprite = supportWeaponSprites[supportWeaponInt];
            shipSpriteImage.sprite = sprites[spriteInt].Icon;
            inGameSprite = sprites[0].Icon;
            //difficulty = debugDifficulty;
            //difficultyInt = (int)difficulty;
            //startGameMenuDiffucltyText.text = "Difficulty: " + System.Enum.GetName(typeof(Difficulty), difficultyInt);
        }
        else
        {
            winPanel.SetActive(false);
            isGameOver = false;
            //difficultyInt = (int)difficulty;
            //difficultyText.text = "Difficulty: " + System.Enum.GetName(typeof(Difficulty), difficultyInt);
        }

        //singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == !this)
        {
            Destroy(gameObject);
        }

        /*
        // TODO move this into the GameController instead with some global shot deviation
        // Easy mode 
        if (GameController.difficulty == Difficulty.Easy)
            shotDeviation = shotdeviationArray[(int)Difficulty.Easy];
        // Medium mode
        else if (GameController.difficulty == Difficulty.Medium)
            shotDeviation = shotdeviationArray[(int)Difficulty.Medium];
        // Hard mode 
        else if (GameController.difficulty == Difficulty.Hard)
            shotDeviation = shotdeviationArray[(int)Difficulty.Hard];

        // Make value positive so we can use in Random.Range later on
        if (shotDeviation < 0)
            shotDeviation *= -1;
        */

        playVideo = FindObjectOfType<PlayVideo>();
    }

    void Update()
    {
        if (!isMenu)
        {
            if (isGameOver)
            {
                retryText.gameObject.SetActive(true);
                SetHighScore();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    RestartScene();
                }
            }
            else
            {
                gameTime += Time.deltaTime;
                scoreText.text = playerScore.ToString();
                highScoreText.text = playerHighScore.ToString();
                TimeSpan timeSpan = TimeSpan.FromSeconds(gameTime);
                timeText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            }
        }
    }

    public void Win()
    {
        StartCoroutine(WinRoutine(2.0f));     
    }

    IEnumerator WinRoutine(float seconds)
    {
        // Wait so that the win screen doesnt immediately popup
        yield return new WaitForSeconds(seconds);

        isMenu = true;
        winPanel.SetActive(true);
        finalScoreText.text = "Final Score: " + playerScore.ToString();
        TimeSpan timeSpan = TimeSpan.FromSeconds(gameTime);
        finalTimeText.text = "Time: " + string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    public void MoveShipToWin()
    {
        winPanel.SetActive(false);
        player.MoveShipToWin();
    }

    public void MoveToNextStage()
    {
        stage++;
        RestartScene();
    }

    public void SetHighScore()
    {
        if (playerScore > playerHighScore)
        {
            playerHighScore = playerScore;
        }
        scoreText.text = playerScore.ToString();
        highScoreText.text = playerHighScore.ToString();
    }

    public void ResetStaticVariables()
    {
        isGameOver = false;
        playerScore = 0;
    }

    public void RestartScene()
    {
        ResetStaticVariables();
        if (FindObjectOfType<EnemyBoss>() != null)
        {
            FindObjectOfType<BGM>().GetComponent<BGM>().SwapBGM(GameAudio.normal);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /***********************/
    // TODO: move this into a seperate menu script, or maybe into the current MenuScript
    public void LoadScenes(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        loadImage.gameObject.SetActive(true);
        ResetStaticVariables();
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    public void OpenStartGameMenu()
    {
        SoundController.Play((int)MSFX.menuButton);
        startGameMenu.SetActive(true);
    }
    public void CloseStartGameMenu()
    {
        SoundController.Play((int)MSFX.menuButton);
        startGameMenu.SetActive(false);
    }
    public void OpenShipSelectionPanel()
    {
        SoundController.Play((int)MSFX.menuButton);
        shipSelectionPanel.SetActive(true);
    }

    public void OpenWeaponSelectionPanel(int selectedStage)
    {
        SoundController.Play((int)MSFX.menuButton);
        weaponSelectionPanel.SetActive(true);
        stage = selectedStage;
    }

    public void CloseweaponSelectionPanel()
    {
        SoundController.Play((int)MSFX.menuButton);
        weaponSelectionPanel.SetActive(false);
    }

    public void CloseShipSelectionPanel()
    {
        SoundController.Play((int)MSFX.menuButton);
        shipSelectionPanel.SetActive(false);
    }
    public void OpenAchievementMenu()
    {
        achieveMentPanel.SetActive(true);
    }
    public void OpenArmoryMenu()
    {
        armoryPanel.SetActive(true);
    }
    public void CloseArmoryMenu()
    {
        armoryPanel.SetActive(false);
    }
    public void OpenSettingMenu()
    {
        SoundController.Play((int)MSFX.menuButton);
        settingPanel.SetActive(true);
    }
    public void CloseSettingMenu()
    {
        SoundController.Play((int)MSFX.menuButton);
        settingPanel.SetActive(false);
    }

    public void UpdateDifficultyText(int increament)
    {

        difficultyInt = (difficultyInt+increament)%3;
        if (difficultyInt < 0)
        {
            difficultyInt += 3;
        }
        difficulty = (Difficulty)difficultyInt;
        startGameMenuDiffucltyText.text = "Difficulty: " + System.Enum.GetName(typeof(Difficulty), difficultyInt);
    }

    public void UpdateMainWeaponSelection(int increament)
    {
        mainWeaponInt = (mainWeaponInt + increament) % 9;
        if (mainWeaponInt < 0)
            mainWeaponInt += 9;
        mainWeaponImage.sprite = mainWeaponSprites[mainWeaponInt];
    }

    public void UpdateSupportWeaponSelection(int increament)
    {
        supportWeaponInt = (supportWeaponInt + increament) % 9;
        if (supportWeaponInt < 0)
            supportWeaponInt += 9;
        supportWeaponImage.sprite = supportWeaponSprites[supportWeaponInt];
    }

    public void UpdateSpriteImage(int increament)
    {

        spriteInt = (spriteInt + increament) % 5;
        if (spriteInt < 0)
        {
            spriteInt += 5;
        }
        shipSpriteImage.GetComponent<Image>().sprite = sprites[spriteInt].Icon;
        inGameSprite = sprites[spriteInt].Icon;

        if (playVideo == null)
        {
            playVideo = FindObjectOfType<PlayVideo>();        
        }
                
        playVideo.Play(spriteInt);
        
    }
}
