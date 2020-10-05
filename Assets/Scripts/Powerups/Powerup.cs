using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour {

    public abstract void ActivateEffect(GameObject touchedObject);

    protected Rigidbody2D rb2d;
    protected bool hasCollided = false;

    protected void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        rb2d.velocity = new Vector2(0.0f, -3.0f);
    }

    protected void LateUpdate()
    {
        hasCollided = false;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // Collided object is a Powerup so activate the effect on our gameobject
        if (collision.tag == "Player" && !hasCollided) {
            hasCollided = true;
            ActivateEffect(collision.gameObject);
        }
    }
}
