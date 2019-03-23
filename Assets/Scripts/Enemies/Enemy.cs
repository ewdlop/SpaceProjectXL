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

    // Control object's movement
    protected abstract void Kinematics();
    protected bool hasCollided = false;

    protected void Start()
    {
        hasCollided = false;
        health = maxHealth;
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    protected void LateUpdate()
    {
        hasCollided = false;
    }

    protected IEnumerator HitFlash()
    {
        if(renderer!=null)
            renderer.material.color = GameController.instance.hitColor;
        yield return new WaitForSeconds(0.05f);
        if (renderer != null)
            renderer.material.color = originalColor;
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            if (!hasCollided)
            {
                hasCollided = true;
                Instantiate(other.gameObject.GetComponent<Weapon>().hiteffect,
                   new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, -0.01f),
                   Quaternion.identity);
                //need to tune damage
                health -= other.gameObject.GetComponent<Weapon>().damage;
                //0.5f so it is not so "cracked"
                //renderer.material.SetFloat("_OcclusionStrength", 0.5f*(1.0f - healthPercentage));
                StartCoroutine(HitFlash());
            }
        }
    }

    public int getHealth()
    {
        return health;
    }
}
