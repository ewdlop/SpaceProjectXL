using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateManger : MonoBehaviour {

    public GameObject ultimateSlider;
    public GameObject playerShip;
    public GameObject playerShipIcon;
    public GameObject[] playerShipUltimate;
    public Sprite defaultSprite;
    public GameObject defaultUlimtate;
    public float time;

    void Awake() {
        if (GameController.inGameSprite!=null)
            playerShip.GetComponent<SpriteRenderer>().sprite = GameController.inGameSprite;
        else
            playerShip.GetComponent<SpriteRenderer>().sprite = defaultSprite;
        
        //reset collider size/shape after we change sprite
        foreach (PolygonCollider2D cols in playerShip.GetComponents<PolygonCollider2D>())
        {
            Destroy(cols);
        }
        playerShip.AddComponent<PolygonCollider2D>();
        if (GameController.inGameSprite != null)
            playerShipIcon.GetComponent<Image>().sprite = GameController.inGameSprite;
        else
            playerShipIcon.GetComponent<Image>().sprite = defaultSprite;
        if (playerShipUltimate[GameController.spriteInt] != null)
            playerShip.GetComponent<PlayerController>().ultimateWeaponObject = playerShipUltimate[GameController.spriteInt];
        else
            playerShip.GetComponent<PlayerController>().ultimateWeaponObject = defaultUlimtate;
    }
	
	void Update ()
    {
        //update ultimateSlider to the progress value
        if (playerShip != null)
            this.ultimateSlider.GetComponent<Slider>().value = playerShip.GetComponent<PlayerController>().getUltimateProgress();
    }
    
}
