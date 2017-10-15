using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed = 10.0f;

    [Header("Health/Damage Settings")]
    public int maxHealth = 100;
    private int health;
    public float invincibleDuration = 1.2f;
    private bool isInvincible;

    [Header("Shooting Controls")]
    public Transform leftFirePosition; 
    public Transform rightFirePosition;
    public float shotCoolDown = 0.2f;
    private List<GameObject> weaponList; 

    private float timeStamp;

    // For use in making ship flash upon receiving damager
    private new Renderer renderer;

    void Start ()
    {
        health = maxHealth;
        weaponList = WeaponManager.playerWeaponList;
        timeStamp = Time.time;
        renderer = GetComponent<Renderer>();
    }

	void Update ()
    {
        // Don't allow any ship actions if game is over
        if (GameController.instance.isGameOver) { return; }

        if (health <= 0)
        {
            Death();
        }

        Kinematics(); 
        if (timeStamp <= Time.time)
        {
            Shoot();
        }
    }

    // Controls player movement
    void Kinematics()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        gameObject.transform.position = gameObject.transform.position + new Vector3(deltaX, 0f, 0f);

        float deltaY = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        gameObject.transform.position = gameObject.transform.position + new Vector3(0f, deltaY, 0f);
    }

    // Controls shooting 
    void Shoot()
    {
        foreach (GameObject weapon in weaponList)
        {
            // Shoot the weapon if it's unlocked
            if (weapon.GetComponent<Weapon>().isUnlocked)
            {
                weapon.GetComponent<Weapon>().Shoot(this.gameObject.transform, leftFirePosition, rightFirePosition);
            }
        }
        timeStamp = Time.time + shotCoolDown;
    }

    void OnCollisionStay2D(Collision2D collidedTarget)
    {
        // Collided object is a Powerup so activate the effect on our gameobject
        if (collidedTarget.gameObject.GetComponent<Powerup>() != null)
        {
            collidedTarget.gameObject.GetComponent<Powerup>().ActivateEffect(this.gameObject);
        }

        // Ship physically collided with an enemy
        if (collidedTarget.gameObject.GetComponent<Enemy>() != null && !isInvincible)
        {
            // Set ship to invincible
            StartCoroutine(InvincibilityFrames());
            int damage = collidedTarget.gameObject.GetComponent<Enemy>().damage;
            health -= damage;
            SoundController.Play((int)SFX.ShipDamage, damage);
            //Debug.Log("Player Health:" + health);
        }
    }

    void OnTriggerStay2D(Collider2D collidedTarget)
    {
        // Ship collided with enemy projectile
        if (collidedTarget.gameObject.GetComponent<EnemyWeapon>() != null)           
        {
            int damage = collidedTarget.GetComponent<EnemyWeapon>().damage;
            Instantiate(collidedTarget.GetComponent<EnemyWeapon>().hiteffect,
                new Vector3(collidedTarget.gameObject.transform.position.x, collidedTarget.gameObject.transform.position.y, -0.01f), 
                Quaternion.identity);
            Destroy(collidedTarget.gameObject);
            // Set ship to invincible if it's not current invincible
            if (!isInvincible)
            {
                StartCoroutine(InvincibilityFrames());
                health -= damage;
                SoundController.Play((int)SFX.ShipDamage, damage);
            }   
        }  
    }

    public List<GameObject> getWeaponList()
    {
        return weaponList;
    }

    public void unlockWeapon(int index)
    {
        weaponList[index].GetComponent<Weapon>().isUnlocked = true;
    }

    IEnumerator HitFlash()
    {
        // Flicker effect
        while (isInvincible)
        {
            this.renderer.enabled = false;
            yield return new WaitForSeconds(0.05f);          
            this.renderer.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        // Make the ship flash while ship is invincible
        StartCoroutine(HitFlash());
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    public void AddHealth(int amount)
    {
        health += amount;
        Mathf.Clamp(health, 0, maxHealth);
    }

    private void Death()
    {
        SoundController.Play((int)SFX.ShipDeath);
        Destroy(this.gameObject);
        GameController.instance.isGameOver = true;

        /*
        rb.velocity = new Vector2(0, 0);
        Camera.main.GetComponent<FollowPlayerShip>().enabled = false;

        bigDebris1.SetActive(true);
        bigDebris2.SetActive(true);
        bigDebris3.SetActive(true);

        smallDebris1.SetActive(true);
        smallDebris2.SetActive(true);
        smallDebris3.SetActive(true);

        float bigDebrisX = 0.1f;
        float bigDebrisY = 0.1f;
        float smallDebrisX = 0.5f;
        float smallDebrisY = 0.5f;

        bigDebris1.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * (bigDebrisX + Random.Range(0f, 1f)), -1 * (bigDebrisY + Random.Range(0f, 1f)));
        bigDebris1.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f) * 5 + Random.Range(1f, 2f), ForceMode2D.Impulse);
        bigDebris2.GetComponent<Rigidbody2D>().velocity = new Vector2((bigDebrisX + Random.Range(0f, 1f)), -1 * (bigDebrisY + Random.Range(0f, 1f)));
        bigDebris1.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f) * 5 + Random.Range(1f, 2f), ForceMode2D.Impulse);
        bigDebris3.GetComponent<Rigidbody2D>().velocity = new Vector2((bigDebrisX + Random.Range(0f, 1f)), (bigDebrisY + Random.Range(0f, 1f)));
        bigDebris1.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f) * 5 + Random.Range(1f, 2f), ForceMode2D.Impulse);

        smallDebris1.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * (smallDebrisX + Random.Range(1f, 2f)), -1 * (smallDebrisY + Random.Range(1f, 2f)));
        smallDebris1.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f) * 10 + Random.Range(1f, 10f), ForceMode2D.Impulse);
        smallDebris2.GetComponent<Rigidbody2D>().velocity = new Vector2((smallDebrisX + Random.Range(1f, 2f)), -1 * (smallDebrisY + Random.Range(1f, 2f)));
        smallDebris2.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f) * 10 + Random.Range(1f, 10f), ForceMode2D.Impulse);
        smallDebris3.GetComponent<Rigidbody2D>().velocity = new Vector2((smallDebrisX + Random.Range(1f, 2f)), (smallDebrisY + Random.Range(1f, 2f)));
        smallDebris3.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f) * 10 + Random.Range(1f, 10f), ForceMode2D.Impulse);
        */
    }

}
