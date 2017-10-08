using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeaponFiringController : MonoBehaviour {

    public List<GameObject> weaponProjectileList;
    public GameObject weaponManager;
    public Transform leftFirePosition;
    public Transform rightFirePosition;
    public float shotCoolDown;
    public float projectileSpeed = 25.0f;
    public float projectileDuration = 1.0f;
    public List<Weapon> weaponList;
    private bool canShoot;
    private float timeStamp;
    private Rigidbody2D shipRB; 
    
    void Start() 
    {
        canShoot = true;
        timeStamp = Time.time;
        shipRB = GetComponent<Rigidbody2D>();

        string names = gameObject.name;
        switch(names)
        {
            case "PlayerShip_Green":
                weaponList = WeaponManager.playerWeaponList;
                break;
            case "LeftCorvette":
                weaponList = WeaponManager.leftCorvetteWeaponList;
                break;
            case "RightCorvette":
                weaponList = WeaponManager.rightCorvetteWeaponList;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (timeStamp <= Time.time)
        { 
            canShoot = true;
        }

        if (!GameController.instance.gameOver && canShoot && LaunchPlayerShip.isPlayerShipLaunched)
        {
            foreach (Weapon weapon in weaponList)
            {
                if (weapon.isUnlocked)
                {
                    GameObject leftProjectile=null;
                    GameObject rightProjectile=null;
                    int index = WeaponManager.playerWeaponList.IndexOf(weapon);
                    GameObject prefab = weapon.prefab;
                    float weaponAngletoRad = weapon.launchAngle * Mathf.Deg2Rad;
                    if (leftFirePosition != null)
                    {
                        leftProjectile = Instantiate(prefab, leftFirePosition.position, leftFirePosition.rotation) as GameObject;
                    }
                    if (rightFirePosition != null)
                    {
                        rightProjectile = Instantiate(prefab, rightFirePosition.position, rightFirePosition.rotation) as GameObject;
                        rightProjectile.GetComponent<ProjectileController>().isFireFromRight = true;
                    }


                    switch (weapon.name)
                    {
                        case "SineMissiles":
                            break;

                        case "CircleMissiles":
                            leftProjectile.GetComponent<ProjectileController>().playerSpaceShip = gameObject;
                            rightProjectile.GetComponent<ProjectileController>().playerSpaceShip = gameObject;
                            break;
                        case "ChasingMissiles":
                            DestroyObject(leftProjectile);
                            DestroyObject(rightProjectile);
                            Destructibles[] enemies = FindObjectsOfType<Destructibles>();
                            
                            for (int i = 0; i < 3; i++)
                            {
                                leftProjectile = Instantiate(prefab, leftFirePosition.position, leftFirePosition.rotation) as GameObject;
                                rightProjectile = Instantiate(prefab, rightFirePosition.position, rightFirePosition.rotation) as GameObject;

                            }
                            break;
                        case "SpreadLaser":
                            DestroyObject(leftProjectile);
                            DestroyObject(rightProjectile);
                            for(int i=0;i<10;i++)
                            {
                                GameObject SpreadLaser = Instantiate(prefab, new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,0.01f), leftFirePosition.rotation);

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
                                 totalSpeed = shipRB.velocity.magnitude + weapon.speed;
                            }
                            else
                            {
                                totalSpeed = weapon.speed;

                            }
                            float leftWeaponUnitVectorX = Mathf.Cos(weaponAngletoRad);
                            float leftWeaponUnitVecotrY = Mathf.Sin(weaponAngletoRad);
                            //final velocity=rotation matrix(shipfacingangle)*inital velocity //same as (cos(wA+fA), sin(wA+fA)) for this case 
                            Vector2 leftLaserRelativeVelocity = totalSpeed * new Vector2(leftWeaponUnitVectorX * Mathf.Cos(Mathf.PI / 2) - leftWeaponUnitVecotrY * Mathf.Sin(Mathf.PI / 2)
                                , leftWeaponUnitVectorX * Mathf.Sin(Mathf.PI / 2) + leftWeaponUnitVecotrY * Mathf.Cos(Mathf.PI / 2));
                            if (leftFirePosition != null)
                                leftProjectile.GetComponent<Rigidbody2D>().velocity = leftLaserRelativeVelocity;
                            //------------------------------------------------------------------------------//
                            float rightWeaponUnitVectorX = Mathf.Cos(-1 * weaponAngletoRad);
                            float rightWeaponUnitVecotrY = Mathf.Sin(-1 * weaponAngletoRad);
                            Vector2 rightLaserRelativeVelocity = totalSpeed * new Vector2(rightWeaponUnitVectorX * Mathf.Cos(Mathf.PI / 2) - rightWeaponUnitVecotrY * Mathf.Sin(Mathf.PI / 2)
                                , rightWeaponUnitVectorX * Mathf.Sin(Mathf.PI / 2) + rightWeaponUnitVecotrY * Mathf.Cos(Mathf.PI / 2));
                            if (rightFirePosition != null)
                                rightProjectile.GetComponent<Rigidbody2D>().velocity = rightLaserRelativeVelocity;

                            SoundController.Play((int)SFX.ShipLaserFire, 0.1f);
                            break;
                    }
                }
            }
            canShoot = false;
            timeStamp = Time.time + shotCoolDown;
        }   
    }
   
    void DestroyShip() 
    {
        Destroy(this.gameObject); 
    }
}
