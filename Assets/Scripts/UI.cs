using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles all UI related functions
// TODO extract UI element from the GameController
public class UI : MonoBehaviour {

    public Text difficultyText;
    public Text scoreText;
    public Text highScoreText;
    public Text retryText;

    public static int spriteInt;
    public List<ShipSprite> sprites;
    public Image shipSpriteImage;
    
    public Sprite inGameSprite;

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
