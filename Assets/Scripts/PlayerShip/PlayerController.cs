using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed = 10.0f;

    [Header("Total lives")]
    public static int maxLives = 3;
    private int lives;

    [Header("Spawn Settings")]
    public Transform playerSpawn; // Where the player is respawned
    public float invincibleDuration = 1.2f;
    private bool isInvincible;
    public float deathDuration = 1.5f;
    private bool isDead;

    [Header("Shooting Controls")]
    public Transform leftFirePosition; 
    public Transform rightFirePosition;
    public float shotCoolDown = 0.2f;
    //private List<GameObject> weaponList; 
    public GameObject mainWeaponObject;
    public GameObject supportWeaponObject;
    private Weapon mainWeapon;
    private Weapon supportWeapon;

    [Header("Ultimate Controls")]
    public GameObject ultimateWeaponObject;
    private float ultimateProgress; // current progress of the ultimate bar
    private bool isDrainingProgess; // sets the ultimate bar to be drained
    public float chargingDuration;  // the amount the bar is charged per tick
    public float dischargingDuration;  // the amount the bar is charged per tick
    public float ultimateDuration;  // the maximum amount allowed of the ultimate bar
    private Weapon ultimateWeapon;  

    // For use in making ship flash upon receiving damage
    private new Renderer renderer;

    void Start ()
    {
        lives = maxLives;
        //weaponList = WeaponManager.playerWeaponList;
        renderer = GetComponent<Renderer>();

        mainWeapon = mainWeaponObject.GetComponent<Weapon>();
        mainWeapon.cooldownStamp = Time.time;
   
        supportWeapon = supportWeaponObject.GetComponent<Weapon>();
        supportWeapon.cooldownStamp = Time.time;

        ultimateWeapon = ultimateWeaponObject.GetComponent<Weapon>();
        ultimateProgress = 0.0f;
        ultimateWeapon.cooldownStamp = 0.0f;
    }

	void Update ()
    {
        if (!isDead)
        {
            Kinematics();
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
        // Fire the main weapon if off cooldown
        if ((Input.GetAxis("MainFire") > 0) && 
            (mainWeapon.cooldownStamp < Time.time))
        {
            mainWeapon.Shoot(this.gameObject.transform, leftFirePosition, rightFirePosition);
            mainWeapon.cooldownStamp = Time.time + mainWeapon.cooldown;
        }

        // Fire the support weapon if off cooldown
        if ((Input.GetAxis("SupportFire") > 0) &&
            (supportWeapon.cooldownStamp < Time.time))
        {
            supportWeapon.Shoot(this.gameObject.transform, leftFirePosition, rightFirePosition);
            supportWeapon.cooldownStamp = Time.time + supportWeapon.cooldown;
        }

        // Handle the ultimate 
        controlUltimate();
    }

    void controlUltimate()
    {
        //time += Time.deltaTime;

        // Fire the ultimate weapon if bar is full and we click the ultimate button
        if (Input.GetKeyDown("u") && ultimateProgress >= ultimateDuration)
        {
            isDrainingProgess = true;
            fillProgress(-1 * Time.deltaTime / ultimateDuration);
        }      
        else
        {
            // if ultimate was activated
            if (isDrainingProgess)
            {
                fillProgress(-1 * Time.deltaTime / dischargingDuration);
                // Instantiate the ultimate weapon while the bar is not yet 0
                if (ultimateProgress > 0.0f) 
                {
                    if (ultimateWeapon.cooldownStamp < Time.time)
                    {
                        ultimateWeapon.Shoot(this.gameObject.transform, leftFirePosition, rightFirePosition);
                        ultimateWeapon.cooldownStamp = Time.time + ultimateWeapon.cooldown;
                    }
                }
                // Ultimate ends when bar reaches 0
                else
                {
                    isDrainingProgess = false;
                }
            }
            else
            {
                // charging
                fillProgress(Time.deltaTime / chargingDuration);
            }
        }
    }

    public void fillProgress(float amount)
    {
        ultimateProgress = Mathf.Clamp(ultimateProgress + amount, 0f, ultimateDuration);
        //this.ultimateSlider.GetComponent<Slider>().value = progress;
    }

    public float getUltimateProgress()
    {
        return ultimateProgress;
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
            StartCoroutine(Death());
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
                StartCoroutine(Death());
                //StartCoroutine(InvincibilityFrames());
                //SoundController.Play((int)SFX.ShipDamage, damage);
            }   
        }  
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

    IEnumerator Death()
    {
        SoundController.Play((int)SFX.ShipDeath);
        renderer.enabled = false;
        isDead = true;
        isInvincible = true;
        yield return new WaitForSeconds(deathDuration);
        RespawnPlayer();  
    }

    public void RespawnPlayer()
    {
        lives--;
        GameController.instance.UpdateLivesText(lives);
        if (lives >= 0)
        {
            this.transform.position = playerSpawn.position;
            isDead = false;
            StartCoroutine(InvincibilityFrames());
        }
        else
        {
            GameController.instance.isGameOver = true;
            Destroy(this.gameObject);
        }
    }
}
