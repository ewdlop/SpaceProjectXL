using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefaultLaser : EnemyWeapon {

    // DefaultLaser: 
    // Shoots forward at the specified launch angle

    void Start()
    {
        hasCollided = false;
        Kinematics();
    }

    void Update()
    {
        //Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject leftProjectile;
        GameObject rightProjectile;  // Creating a temp object just to set the isRightSide variable
        
        // Offset to match the enemy ship sprite
        float rotationAngle = ship.transform.eulerAngles.z - 90.0f;

        if (leftFire != null)
        {
            leftProjectile=Instantiate(this.gameObject, leftFire.position,
                leftFire.rotation);          
            leftProjectile.GetComponent<Weapon>().launchAngle = rotationAngle;
        }

        if (rightFire != null)
        {
            rightProjectile = Instantiate(this.gameObject, rightFire.position,
                rightFire.rotation) as GameObject;
            rightProjectile.GetComponent<Weapon>().launchAngle = rotationAngle;  
        }

        SoundController.Play((int)SFX.ShipLaserFire, 0.2f);
    }

    public override void Kinematics()
    {
        //Debug.Log("launch angle " + this.GetComponent<Weapon>().launchAngle);
        //Debug.Log("RAD launch angle " + this.GetComponent<Weapon>().launchAngle * Mathf.Deg2Rad);
        float launchAngletoRad = this.GetComponent<Weapon>().launchAngle * Mathf.Deg2Rad;
        Vector2 relativeVelocity =
            speed * new Vector2(Mathf.Cos(launchAngletoRad),
            Mathf.Sin(launchAngletoRad));

        this.GetComponent<Rigidbody2D>().velocity = relativeVelocity;
    }

}
