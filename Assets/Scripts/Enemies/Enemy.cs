using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    public int maxHealth = 25;
    protected int health;
    public int score = 5;
    public int damage = 50;

    protected new Renderer renderer;
    protected Color originalColor;

    protected void Start()
    {
        health = maxHealth;
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    protected IEnumerator HitFlash()
    {
        renderer.material.color = GameController.instance.hitColor;
        yield return new WaitForSeconds(0.05f);
        renderer.material.color = originalColor;
    }
}
