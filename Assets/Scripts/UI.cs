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
    public Slider bossHealth;

    public static int spriteInt;
    public List<ShipSprite> sprites;
    public Image shipSpriteImage;
    
    public Sprite inGameSprite;

    private EnemyShip boss;

    void Start()
    {
        boss = (EnemyShip)FindObjectOfType(typeof(EnemyShip));
    }

    void Update()
    {
        if (boss != null)
        {
            //Debug.Log(bossHealth.value);
            bossHealth.gameObject.SetActive(true);
            bossHealth.maxValue = boss.GetComponent<Enemy>().maxHealth;
            bossHealth.value = boss.GetComponent<Enemy>().getHealth();
        }
        else
        {
            bossHealth.gameObject.SetActive(false);
        }
    }

    public void UpdateDifficultyText()
    {
        // Change to use nonstatic!
        /*
        switch(GameController.instance.difficulty)
        {

        }
        */
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
