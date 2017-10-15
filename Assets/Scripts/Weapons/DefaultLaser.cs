using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLaser : Weapon {

    // DefaultLaser: 
    // Shoots forward at the specified launch angle

    private float launchAngleRad;
    private float unitVectorX, unitVectorY;

    private bool isFiredFromRight; 

    void Start()
    {
        launchAngleRad = launchAngle * Mathf.Deg2Rad;
        if (isFiredFromRight)
        {
            unitVectorX = Mathf.Cos(-1 * launchAngleRad);
            unitVectorY = Mathf.Sin(-1 * launchAngleRad);
        }
        else
        {
            unitVectorX = Mathf.Cos(launchAngleRad);
            unitVectorY = Mathf.Sin(launchAngleRad);
        }    
    }

    void Update()
    {
        Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject rightProjectile;  // Creating a temp object just to set the isRightSide variable
        if (leftFire != null)
        {
            Instantiate(this.gameObject, leftFire.position,
                leftFire.rotation);
        }

        if (rightFire != null)
        {
            rightProjectile = Instantiate(this.gameObject, rightFire.position,
                 rightFire.rotation) as GameObject;
            rightProjectile.GetComponent<DefaultLaser>().isFiredFromRight = true;
        }

        SoundController.Play((int)SFX.ShipLaserFire, 0.3f);
    }

    public override void Kinematics()
    {
        Vector2 relativeVelocity =
            speed * new Vector2(unitVectorX * Mathf.Cos(Mathf.PI / 2) - unitVectorY * Mathf.Sin(Mathf.PI / 2),
            unitVectorX * Mathf.Sin(Mathf.PI / 2) + unitVectorY * Mathf.Cos(Mathf.PI / 2));

        this.GetComponent<Rigidbody2D>().velocity = relativeVelocity;
    }
}
