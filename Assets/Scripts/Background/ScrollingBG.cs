using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBG : MonoBehaviour {

    private Rigidbody2D rb2d;
    GameController test; 
	// Use this for initialization
	void Start ()
    {
        test = GameController.instance;
        rb2d = this.GetComponent<Rigidbody2D>();
        //rb2d.velocity = new Vector2(0.0f, GameController.instance.scrollSpeed);
        rb2d.velocity = new Vector2(0.0f, -5.0f);
    }
	
	// Update is called once per frame
	void Update () {
		if (GameController.instance.isGameOver == true)
        {
            rb2d.velocity = Vector2.zero;
        }
	}
}
