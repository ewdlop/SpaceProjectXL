using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeaponFiringController : MonoBehaviour {

    public Transform leftFirePosition;
    public Transform rightFirePosition;
    public float shotCoolDown;

    private List<GameObject> weaponList;
    private bool canShoot;
    private float timeStamp;
    private Rigidbody2D shipRB; 
    
    void Start() 
    {
        canShoot = true;
        timeStamp = Time.time;
        shipRB = GetComponent<Rigidbody2D>();

        string names = gameObject.name;
        weaponList = WeaponManager.playerWeaponList;
    }

    void Update()
    {
        if (timeStamp <= Time.time)
        {
            canShoot = true;
        }

        if (!GameController.instance.isGameOver && canShoot)
        {
            //Debug.Log("WeaponList Size: " + weaponList.Count);
            foreach (GameObject weapon in weaponList)
            {
                if (weapon.GetComponent<Weapon>().isUnlocked)
                {
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
                    //Debug.Log(weapon.name);
                    /*
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
                            for(int i=0;i<10;i++)
                            {
                                GameObject SpreadLaser = Instantiate(weapon, new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,0.01f), leftFirePosition.rotation);

                                //final velocity=rotation matrix(shipfacingangle)*inital velocity //same as (cos(wA+fA), sin(wA+fA)) for this case 
                                Vector2 LaserVelocity = 5f* new Vector2(Mathf.Cos(135f/180f*Mathf.PI-10f/180f*Mathf.PI*i), Mathf.Sin(135f / 180f * Mathf.PI - 10f / 180f * Mathf.PI * i));
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
                */
                }
                canShoot = false;
                timeStamp = Time.time + shotCoolDown;
            }
        }
    }
}
