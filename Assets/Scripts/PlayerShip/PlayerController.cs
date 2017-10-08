using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed = 10.0f;
    public GameObject enemyTypeManger;
    public GameObject powerUpManger;
    public List <PowerUp> powerUpList = new List<PowerUp>();

    private Rigidbody2D shipRB;
    // For controlling ship shooting 
    public Transform leftFirePosition; 
    public Transform rightFirePosition;
    public float shotCoolDown = 0.2f;
    private List<GameObject> weaponList; 
    private float timeStamp; 

    void Start ()
    {
        shipRB = GetComponent<Rigidbody2D>();
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
            // Do nothing if the weapon is not unlocked
            if (!weapon.GetComponent<Weapon>().isUnlocked) { continue; }  

            GameObject leftProjectile = null;
            GameObject rightProjectile = null;
            float weaponAngletoRad = weapon.GetComponent<Weapon>().launchAngle * Mathf.Deg2Rad;

            if (leftFirePosition != null)
            {
                leftProjectile = Instantiate(weapon, leftFirePosition.position, leftFirePosition.rotation) as GameObject;
            }
            if (rightFirePosition != null)
            {
                rightProjectile = Instantiate(weapon, rightFirePosition.position, rightFirePosition.rotation) as GameObject;
            }

            switch (weapon.name)
            {
                case "WaveMissile":
                    if (rightProjectile != null)
                    {
                        rightProjectile.GetComponent<WaveMissile>().isFiredFromRight = true;
                    }
                    break;

                case "CircleMissile":
                    if (rightProjectile != null)
                    {
                        rightProjectile.GetComponent<CircleMissle>().isFiredFromRight = true;
                    }
                    break;

                case "ChasingMissiles":
                    DestroyObject(leftProjectile);
                    DestroyObject(rightProjectile);
                    Destructibles[] enemies = FindObjectsOfType<Destructibles>();

                    for (int i = 0; i < 3; i++)
                    {
                        leftProjectile = Instantiate(weapon, leftFirePosition.position, leftFirePosition.rotation) as GameObject;
                        rightProjectile = Instantiate(weapon, rightFirePosition.position, rightFirePosition.rotation) as GameObject;

                    }
                    break;

                case "SpreadLaser":
                        DestroyObject(leftProjectile);
                        DestroyObject(rightProjectile);
                        for (int i = 0; i < 10; i++)
                        {
                            GameObject SpreadLaser = Instantiate(weapon, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0.01f), leftFirePosition.rotation);

                            //final velocity=rotation matrix(shipfacingangle)*inital velocity //same as (cos(wA+fA), sin(wA+fA)) for this case 
                            Vector2 LaserVelocity = 5f * new Vector2(Mathf.Cos(135f / 180f * Mathf.PI - 10f / 180f * Mathf.PI * i), Mathf.Sin(135f / 180f * Mathf.PI - 10f / 180f * Mathf.PI * i));
                            SpreadLaser.GetComponent<Rigidbody2D>().velocity = LaserVelocity;

                        }
                        SoundController.Play((int)SFX.ShipLaserFire, 0.1f);
                        break;

                default:
                    float totalSpeed = 0;
                    if (shipRB != null)
                    {
                        totalSpeed = shipRB.velocity.magnitude + weapon.GetComponent<Weapon>().speed;
                    }
                    else
                    {
                        totalSpeed = weapon.GetComponent<Weapon>().speed;

                    }
                    float leftWeaponUnitVectorX = Mathf.Cos(weaponAngletoRad);
                    float leftWeaponUnitVecotrY = Mathf.Sin(weaponAngletoRad);

                    Vector2 leftLaserRelativeVelocity = totalSpeed * new Vector2(leftWeaponUnitVectorX * Mathf.Cos(Mathf.PI / 2) - leftWeaponUnitVecotrY * Mathf.Sin(Mathf.PI / 2)
                        , leftWeaponUnitVectorX * Mathf.Sin(Mathf.PI / 2) + leftWeaponUnitVecotrY * Mathf.Cos(Mathf.PI / 2));
                    if (leftProjectile != null)
                    {
                        leftProjectile.GetComponent<Rigidbody2D>().velocity = leftLaserRelativeVelocity;
                    }
                    //------------------------------------------------------------------------------//
                    float rightWeaponUnitVectorX = Mathf.Cos(-1 * weaponAngletoRad);
                    float rightWeaponUnitVecotrY = Mathf.Sin(-1 * weaponAngletoRad);
                    Vector2 rightLaserRelativeVelocity = totalSpeed * new Vector2(rightWeaponUnitVectorX * Mathf.Cos(Mathf.PI / 2) - rightWeaponUnitVecotrY * Mathf.Sin(Mathf.PI / 2)
                        , rightWeaponUnitVectorX * Mathf.Sin(Mathf.PI / 2) + rightWeaponUnitVecotrY * Mathf.Cos(Mathf.PI / 2));
                    if (rightProjectile != null)
                    {
                        rightProjectile.GetComponent<Rigidbody2D>().velocity = rightLaserRelativeVelocity;
                    }
                    SoundController.Play((int)SFX.ShipLaserFire, 0.1f);
                    break;  
            }
        }
        timeStamp = Time.time + shotCoolDown;
    }

    void OnCollisionEnter2D(Collision2D collidedTarget)
    {
        if (collidedTarget.gameObject.tag == "PowerUp")
        {  
            foreach (PowerUp powerUp in powerUpList)
            {
                int index =powerUpList.IndexOf(powerUp);

                if (collidedTarget.gameObject.name.Contains(powerUp.powerUpName))
                {
                    float healthRepair = powerUp.healthRepair;

                    gameObject.GetComponent<PlayerShipDestructible>().IncreaseHealth(healthRepair);
                    switch (powerUp.playerWeaponIndex)
                    {
                        case 100://health powerup
                            DestroyObject(collidedTarget.gameObject);
                            break;
                        case 101://shield powerup
                            PowerUpManger.weaponPanelOffset += 1f;
                            //TODO unlock using the name
                            //WeaponManager.playerWeaponList[1].isUnlocked = true;
                            PowerUpManger.shieldOn = true;
                            powerUp.IncreaseDuration();//increase the duration
                            powerUp.SetProgress();//update the bar
                            powerUp.weaponPanelBar.SetActive(true);
                            DestroyObject(collidedTarget.gameObject);
                            //*powerUp.weaponPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(-80f, -38.4f + 102.4f * PowerUpManger.weaponPanelOffset);

                            break;
                        default:
                            int weaponIndex =powerUp.playerWeaponIndex;
                            //WeaponManager.playerWeaponList[weaponIndex].isUnlocked = true;
                            PowerUpManger.weaponPanelOffset += 1f;
                            powerUp.IncreaseDuration();
                            powerUp.SetProgress();
                            powerUp.weaponPanelBar.SetActive(true);
                            DestroyObject(collidedTarget.gameObject);
                            //*powerUp.weaponPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(-80f, -38.4f + 102.4f * PowerUpManger.weaponPanelOffset);

                            break;
                    }

                    break;
                }
            }
        }
    }


    void OnTriggerStay2D(Collider2D collidedTarget)
    {
        if (collidedTarget.gameObject.tag=="PowerUp"|| PlayerShipDestructible.isPlayerShipInvincible || GameController.instance.isGameOver)
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
                    gameObject.GetComponent<PlayerShipDestructible>().DecreaseHealth(damage);
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
