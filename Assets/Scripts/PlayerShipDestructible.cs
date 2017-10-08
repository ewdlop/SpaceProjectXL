using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShipDestructible : MonoBehaviour
{
    public static bool isPlayerShipInvincible;
    public static bool isKillShip;
    private bool canMove;

    public float invincibleDuration;
    //public float timerForRedFlashes;

    public GameObject bigDebris1;
    public GameObject bigDebris2;
    public GameObject bigDebris3;
    public GameObject smallDebris1;
    public GameObject smallDebris2;
    public GameObject smallDebris3;
    public GameObject leftEngineFire;
    public GameObject rightEngineFire;

    public float currentHealth = 100f;
    public float initialMaxHealth = 100f;
    public float currentMaxHealth = 100f;
    public GameObject healthSlider;
    public GameObject enemyTypeManger;

    Rigidbody2D rb;
    //public Image RedFlashes;
    private Renderer renderer;
    private Color originalColor;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
        rb = GetComponent<Rigidbody2D>();
        isPlayerShipInvincible = false;
    }

    void Update()
    {
        if (isKillShip)
        {
            PlayerShipDeath();
            isKillShip = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collidedTarget)
    {

        if ((collidedTarget.gameObject.tag == "EP"|| collidedTarget.gameObject.tag == "Boss") && !isPlayerShipInvincible && !GameController.instance.isGameOver)
        {

            foreach (GameObject enemy in enemyTypeManger.GetComponent<EnemyTypeManger>().enemyTypeList)
            {
                
                int index = enemyTypeManger.GetComponent<EnemyTypeManger>().enemyTypeList.IndexOf(enemy);
                if (collidedTarget.gameObject.name.Contains(enemyTypeManger.GetComponent<EnemyTypeManger>().enemyCloneName[index]))
                {
                    float damage = enemyTypeManger.GetComponent<EnemyTypeManger>().enemyTypeDamageOnPlayerSpaceShip[index];
                    DecreaseHealth(damage);
                    if (collidedTarget.gameObject.tag == "EP")
                    {
                        Instantiate(collidedTarget.gameObject.GetComponent<ProjectileController>().hiteffect, new Vector3(collidedTarget.gameObject.transform.position.x, collidedTarget.gameObject.transform.position.y, -0.02f), Quaternion.identity);
                        Destroy(collidedTarget.gameObject);
                    }
                    break;
                }
            }
        }
    }
    public void DecreaseHealth(float damage)
    {

        SoundController.Play((int)SFX.ShipDamage, damage);
        
        currentHealth -= damage;
        float healthPercentage = Mathf.Clamp(currentHealth / currentMaxHealth, 0f, 1f);
        healthSlider.GetComponent<Slider>().value = healthPercentage;

        if (currentHealth <= 0)
        {
            isKillShip = true;
        }
        else
        {
            StartCoroutine(InvincibleAfterTakeDamage());
        }
    }

    public void IncreaseHealth(float health)
    {
        SoundController.Play((int)SFX.PickupHealth);
        currentHealth += health;
        currentHealth = Mathf.Clamp(currentHealth, 0.0f, currentMaxHealth);
        float healthPercentage = Mathf.Clamp(currentHealth / currentMaxHealth, 0f, 1f);
        healthSlider.GetComponent<Slider>().value = healthPercentage;
    }

    IEnumerator InvincibleAfterTakeDamage()
    {
        isPlayerShipInvincible = true;
        StartCoroutine(HitFlash());
        yield return new WaitForSeconds(invincibleDuration);
        isPlayerShipInvincible = false;
    }

    IEnumerator HitFlash()
    {
        while (isPlayerShipInvincible)
        {
            this.GetComponent<Renderer>().material.color = GameController.instance.hitColor;
            yield return new WaitForSeconds(0.05f);
            this.GetComponent<Renderer>().material.color = originalColor;
        }
    }

    void PlayerShipDeath()
    {
        canMove = false;
        SoundController.Play((int)SFX.ShipDeath);

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;

        GameController.instance.isGameOver = true;
        GameController.isPlayerShipDead = true;

        leftEngineFire.SetActive(false);
        rightEngineFire.SetActive(false);
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
    }
}
