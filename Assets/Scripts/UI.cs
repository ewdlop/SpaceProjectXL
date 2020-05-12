using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles all UI related functions
// TODO extract UI element from the GameController
public class UI : MonoBehaviour {

    [Header("UI Settings")]
    public Text difficultyText;
    public Text scoreText;
    public Text highScoreText;
    public Text retryText;
    public Text livesText;
    public Slider bossHealth;

    public static int spriteInt;
    public List<ShipSprite> sprites;
    public Image shipSpriteImage;
    
    public Sprite inGameSprite;

    private EnemyBoss boss;
    private PlayerController player;

    void Start()
    {
    }

    void Update()
    {
        if(player == null)
            player = FindObjectOfType<PlayerController>();
        if (boss == null)
            boss = FindObjectOfType<EnemyBoss>();

        UpdateAllText();

        if (boss != null)
        {
            if (boss.GetComponent<EnemyBoss>().BossIsActive())
            {
                //Debug.Log(bossHealth.value);
                bossHealth.gameObject.SetActive(true);
                bossHealth.maxValue = boss.GetComponent<EnemyBoss>().maxHealth;
                bossHealth.value = boss.GetComponent<EnemyBoss>().getHealth();
            }
        }
        else
        {
            bossHealth.gameObject.SetActive(false);       
        }
    }

    public void UpdateAllText()
    {
        UpdateLivesText();
    }

    public void UpdateLivesText()
    {
        if (player)
        {
            livesText.text = "x" + player.GetLives().ToString();
        }
        else
        {
            livesText.text = "x0"; 
        }
    }

    public void UpdateSpriteImage(int increment)
    {
        // TODO what is this modulo for?
        // Can we just map the ship sprites with a plain index?
        spriteInt = (spriteInt + increment) % 2;
        if (spriteInt < 0)
        {
            spriteInt += 2;
        }
        shipSpriteImage.GetComponent<Image>().sprite = sprites[spriteInt].Icon;
        inGameSprite = sprites[spriteInt].Icon;
    }
}
