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

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            health -= other.gameObject.GetComponent<Weapon>().damage;
            float healthPercentage = Mathf.Clamp((float)health / (float)maxHealth, 0.0f, 1.0f);
            renderer.material.SetFloat("_OcclusionStrength", 1.0f - healthPercentage);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            Instantiate(other.gameObject.GetComponent<Weapon>().hiteffect,
               new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, -0.01f),
               Quaternion.identity);

            Destroy(other.gameObject);
            StartCoroutine(HitFlash());
            health--;
        }
    }
}
