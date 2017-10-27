using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLaser : Weapon {

    // DefaultLaser: 
    // Shoots forward at the specified launch angle

    private bool isFiredFromRight; 


    void Start()
    {

    }

    void Update()
    {
        Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject leftProjectile;
        GameObject rightProjectile;  // Creating a temp object just to set the isRightSide variable
        if (leftFire != null)
        {
            leftProjectile=Instantiate(this.gameObject, leftFire.position,
                leftFire.rotation);
            leftProjectile.GetComponent<Weapon>().launchAngle = 45f;
            leftProjectile.transform.eulerAngles = new Vector3(0f, 0f,-45f);
            leftProjectile = Instantiate(this.gameObject, leftFire.position,
            leftFire.rotation);
            leftProjectile.GetComponent<Weapon>().launchAngle = 90f;
            leftProjectile = Instantiate(this.gameObject, leftFire.position,
            leftFire.rotation);
            leftProjectile.GetComponent<Weapon>().launchAngle = 135f;
            leftProjectile.transform.eulerAngles = new Vector3(0f, 0f, 45f);
        }

        if (rightFire != null)
        {
            rightProjectile = Instantiate(this.gameObject, rightFire.position,
                 rightFire.rotation) as GameObject;
            rightProjectile.GetComponent<Weapon>().launchAngle = 45f;
            rightProjectile.transform.eulerAngles = new Vector3(0f, 0f, -45f);
            rightProjectile.GetComponent<DefaultLaser>().isFiredFromRight = true;
            rightProjectile = Instantiate(this.gameObject, rightFire.position,
                rightFire.rotation) as GameObject;
            rightProjectile.GetComponent<Weapon>().launchAngle = 90f;
            rightProjectile.GetComponent<DefaultLaser>().isFiredFromRight = true;
            rightProjectile = Instantiate(this.gameObject, rightFire.position,
                rightFire.rotation) as GameObject;
            rightProjectile.GetComponent<Weapon>().launchAngle = 135f;
            rightProjectile.transform.eulerAngles = new Vector3(0f, 0f, 45f);
            rightProjectile.GetComponent<DefaultLaser>().isFiredFromRight = true;
        }

        SoundController.Play((int)SFX.ShipLaserFire, 0.3f);
    }

    public override void Kinematics()
    {
        float launchAngletoRad = this.GetComponent<Weapon>().launchAngle * Mathf.Deg2Rad;
        Vector2 relativeVelocity =
            speed * new Vector2(Mathf.Cos(launchAngletoRad),
            Mathf.Sin(launchAngletoRad));

        this.GetComponent<Rigidbody2D>().velocity = relativeVelocity;
    }
}
