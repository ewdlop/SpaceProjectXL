using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed = 10.0f;
    public GameObject enemyTypeManger;
    public GameObject powerUpManger;
    public List <PowerUp> powerUpList = new List<PowerUp>();
    private Rigidbody2D rb2D;

    [Header("Shooting Controls")]
    public Transform leftFirePosition; 
    public Transform rightFirePosition;
    public float shotCoolDown = 0.2f;
    private List<GameObject> weaponList; 
    private float timeStamp; 

    void Start ()
    {
        rb2D = GetComponent<Rigidbody2D>();
        powerUpList = powerUpManger.GetComponent<PowerUpManger>().GetPowerUpList();
        weaponList = WeaponManager.playerWeaponList;
        timeStamp = Time.time; 
    }

	void Update ()
    {
        // Don't allow any ship actions if game is over
        if (GameController.instance.isGameOver) { return; }

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
            // Instantiate the weapon if it's unlocked
            if (weapon.GetComponent<Weapon>().isUnlocked)
            {
                weapon.GetComponent<Weapon>().Instantiate(this.gameObject.transform, leftFirePosition, rightFirePosition);
            }
        }
        timeStamp = Time.time + shotCoolDown;
    }

    void OnCollisionEnter2D(Collision2D collidedTarget)
    {
        // Collided object is a Powerup so activate the effect on our gameobject
        if (collidedTarget.gameObject.GetComponent<Powerup>() != null)
        {
            collidedTarget.gameObject.GetComponent<Powerup>().ActivateEffect(this.gameObject);
        }

        // Collided object is an enemy or enemy projectile, so reduce health
        
    }

    void OnTriggerStay2D(Collider2D collidedTarget)
    {
        if (collidedTarget.gameObject.tag=="PowerUp"|| DestructibleShip.isPlayerShipInvincible || GameController.instance.isGameOver)
        {
            return; // Ship is invincible so do nothing
        }
        else
        {
            foreach (GameObject enemy in enemyTypeManger.GetComponent<EnemyTypeManger>().enemyTypeList)
            {
                int index = enemyTypeManger.GetComponent<EnemyTypeManger>().enemyTypeList.IndexOf(enemy);
                if (collidedTarget.gameObject.name.Contains(enemyTypeManger.GetComponent<EnemyTypeManger>().enemyCloneName[index]))
                {
                    float damage = enemyTypeManger.GetComponent<EnemyTypeManger>().enemyTypeDamageOnPlayerSpaceShip[index];
                    gameObject.GetComponent<DestructibleShip>().DecreaseHealth(damage);
                    if (collidedTarget.gameObject.tag == "EP")/*EP=EnemyProjectiles*/
                    {
                        Instantiate(collidedTarget.gameObject.GetComponent<ProjectileController>().hiteffect, new Vector3(collidedTarget.gameObject.transform.position.x, collidedTarget.gameObject.transform.position.y, -0.01f), Quaternion.identity);
                        Destroy(collidedTarget.gameObject);
                    }

                    break;
                }
            }
        }
    }


}
