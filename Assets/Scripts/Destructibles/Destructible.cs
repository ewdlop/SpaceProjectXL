﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

    public int maxHealth;
    protected int health;

    public int score;

    protected Renderer renderer;
    protected Color originalColor;

    protected void Start()
    {
        health = maxHealth;
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    protected IEnumerator HitFlash()
    {
        this.renderer.material.color = GameController.instance.hitColor;
        yield return new WaitForSeconds(0.05f);
        this.renderer.material.color = originalColor;
    }
}