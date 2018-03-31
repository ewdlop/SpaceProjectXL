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
    //public static bool isPlayerShipDead;
    public bool isMenu;

    [Header("UI Settings")]
    public Text difficultyText;
    public Text scoreText;
    public Text highScoreText;
    public Text retryText;
    public Text livesText;

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

    [Header("ArmoryMenu")]
    public GameObject armoryPanel;

    [Header("AchievementMenu")]
    public GameObject achieveMentPanel;


    void Start() {
        if (isMenu)
        {
            inGameSprite = sprites[0].Icon;
            difficulty = debugDifficulty;
            difficultyInt = (int)difficulty;
            startGameMenuDiffucltyText.text = "Difficulty: " + System.Enum.GetName(typeof(Difficulty), difficultyInt);
        }
        else
        {
            UpdateLivesText(PlayerController.maxLives);
            isGameOver = false;
            difficultyInt = (int)difficulty;
            difficultyText.text = "Difficulty: " + System.Enum.GetName(typeof(Difficulty), difficultyInt);
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
                    ResetStaticVariables();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
            else
            {
                scoreText.text = "Score:" + playerScore.ToString();
                highScoreText.text = "HighScore:" + playerHighScore.ToString();
            }
        }
    }

    public void SetHighScore()
    {
        if (playerScore > playerHighScore)
        {
            playerHighScore = playerScore;
        }
        scoreText.text = "Score:" + playerScore.ToString();
        highScoreText.text = "HighScore:" + playerHighScore.ToString();
    }

    public void ResetStaticVariables()
    {
        isGameOver = false;
        playerScore = 0;
    }

    /***********************/
    // TODO: move this into a seperate menu script, or maybe into the current MenuScript
    public void LoadScenes(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        ResetStaticVariables();
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    public void OpenStartGameMenu()
    {
        startGameMenu.SetActive(true);
    }
    public void CloseStartGameMenu()
    {
        startGameMenu.SetActive(false);
    }
    public void OpenShipSelectionPanel()
    {
        shipSelectionPanel.SetActive(true);
    }

    public void CloseShipSelectionPanel()
    {
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

    public void UpdateSpriteImage(int increament)
    {

        spriteInt = (spriteInt + increament) % 5;
        if (spriteInt < 0)
        {
            spriteInt += 5;
        }
        shipSpriteImage.GetComponent<Image>().sprite = sprites[spriteInt].Icon;
        inGameSprite = sprites[spriteInt].Icon;
    }

    public void UpdateLivesText(int lives)
    {
        if (lives < 0)
            lives = 0; // Prevent displaying negative lives 

        livesText.text = "x" + lives.ToString();
    }
}
