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
    public static float playerScore;
    public static float playerHighScore;
    public static bool isPlayerShipDead;
    public bool isMenu;
    public Text difficultyText;
    public Text scoreText;
    public Text highScoreText;
    public Text retryText;
    public GameObject playerShip;
    public Difficulty debugDifficulty;       // Just for setting difficulty via the inspector for debugging
    public static Difficulty difficulty;     // The static difficulty var referenced by other scripts
    public int difficultyInt;  // TODO, can remove this and just use the enum
    public float scrollSpeed = -5.0f;
    public bool isGameOver;
    public Color hitColor;

    /**************/
    public GameObject startGameMenu;
    public Text startGameMenuDiffucltyText;

    /************************/
    public static int spriteInt;
    public List<ShipSprite> sprites;
    public Image shipSpriteImage;
    public GameObject shipSelectionPanel;
    public static Sprite inGameSprite;

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
            //difficulty = debugDifficulty;
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

    }

    void Update()
    {
        if (!isMenu)
        {
            if (isPlayerShipDead)
            {
                retryText.gameObject.SetActive(true);
                SetHighScore();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    playerShip.GetComponent<ShipWeaponFiringController>().enabled = false;
                    ResetStaticVariables();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
            else
            {
                scoreText.text = "Score:" + Mathf.RoundToInt(playerScore).ToString();
                highScoreText.text = "HighScore:" + Mathf.RoundToInt(playerHighScore).ToString();
                playerScore += Time.deltaTime;
            }
        }
    }

    public void SetHighScore()
    {
        if (playerScore > playerHighScore)
        {
            playerHighScore = playerScore;
        }
        scoreText.text = "Score:" + Mathf.RoundToInt(playerScore).ToString();
        highScoreText.text = "HighScore:" + Mathf.RoundToInt(playerHighScore).ToString();
    }

    public void ResetStaticVariables()
    {
        isPlayerShipDead = false;
        playerScore = 0f;
        LaunchPlayerShip.isPlayerShipLaunched = false;
        LaunchPlayerShip.isPlayerShipDebugMode = false;
        DestructibleShip.isKillShip = false;
        DestructibleShip.isPlayerShipInvincible = false;
        PowerUpManger.weaponPanelOffset = 0;

        foreach(GameObject weapon in WeaponManager.playerWeaponList)
        {
            weapon.GetComponent<Weapon>().isUnlocked = false;
        }
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

    public void UpdateDiffcultyText(int increament)
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

        spriteInt = (spriteInt + increament) % 2;
        if (spriteInt < 0)
        {
            spriteInt += 2;
        }
        shipSpriteImage.GetComponent<Image>().sprite = sprites[spriteInt].Icon;
        inGameSprite = sprites[spriteInt].Icon;
    }
}
