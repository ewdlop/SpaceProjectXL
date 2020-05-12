using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleShot: Weapon {


    void Start()
    {
        Destroy(gameObject, 15f);
    }

    void Update()
    {
        Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject blackHole = Instantiate(gameObject, ship.position, Quaternion.identity);
        float launchAngletoRad = ship.gameObject.GetComponent<PlayerController>().cannonAngle * Mathf.Deg2Rad;
        Vector2 relativeVelocity =
           speed * new Vector2(Mathf.Cos(launchAngletoRad),
           Mathf.Sin(launchAngletoRad));
        blackHole.GetComponent<Rigidbody2D>().velocity = relativeVelocity;
    }

    public override void Kinematics()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        EnemyWeapon[] enemyWeapons = FindObjectsOfType<EnemyWeapon>();
        foreach (Enemy enemy in enemies)
        {
            if(enemy.GetComponent<Rigidbody2D>()!=null)
            {
                float radian = Mathf.Atan2(transform.position.y - enemy.transform.position.y, transform.position.x - enemy.transform.position.x);
                enemy.transform.position = enemy.transform.position + (0.35f * new Vector3(Mathf.Cos(radian),Mathf.Sin(radian),0f));
            }
        }
        foreach (EnemyWeapon enemyWeapon in enemyWeapons)
        {
            if (enemyWeapon.GetComponent<Rigidbody2D>() != null)
            {
                float radian = Mathf.Atan2(transform.position.y - enemyWeapon.transform.position.y, transform.position.x - enemyWeapon.transform.position.x);
                enemyWeapon.transform.position += (0.35f * new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f));
                if(Vector2.Distance(enemyWeapon.transform.position, transform.position) <= 0.1f)
                {
                    Destroy(enemyWeapon.gameObject);
                }
            }
        }
    }

}
