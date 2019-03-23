using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLaser : Weapon {

    // DefaultLaser: 
    // Shoots forward at the specified launch angle



    void Start()
    {
        DestroyObject(gameObject, 2f);
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
            float cannonAngle = ship.gameObject.GetComponent<PlayerController>().cannonAngle;
            leftProjectile.GetComponent<Weapon>().launchAngle = cannonAngle - 45f;
            leftProjectile.transform.eulerAngles = new Vector3(0f, 0f, cannonAngle - 135f);
            leftProjectile = Instantiate(this.gameObject, leftFire.position,
            leftFire.rotation);
            leftProjectile.GetComponent<Weapon>().launchAngle = cannonAngle;
            leftProjectile.transform.eulerAngles = new Vector3(0f, 0f, cannonAngle - 90f);
            leftProjectile = Instantiate(this.gameObject, leftFire.position,
            leftFire.rotation);
            leftProjectile.GetComponent<Weapon>().launchAngle = cannonAngle + 45f;
            leftProjectile.transform.eulerAngles = new Vector3(0f, 0f, cannonAngle - 45f);
        }

        if (rightFire != null)
        {
            float cannonAngle = ship.gameObject.GetComponent<PlayerController>().cannonAngle;
            rightProjectile = Instantiate(this.gameObject, rightFire.position,
                 rightFire.rotation) as GameObject;
            rightProjectile.GetComponent<Weapon>().launchAngle = cannonAngle - 45f;
            rightProjectile.transform.eulerAngles = new Vector3(0f, 0f, -cannonAngle - 135f);
            rightProjectile = Instantiate(this.gameObject, rightFire.position,
                rightFire.rotation) as GameObject;
            rightProjectile.GetComponent<Weapon>().launchAngle = cannonAngle;
            rightProjectile.transform.eulerAngles = new Vector3(0f, 0f, cannonAngle - 90f);
            rightProjectile = Instantiate(this.gameObject, rightFire.position,
                rightFire.rotation) as GameObject;
            rightProjectile.GetComponent<Weapon>().launchAngle = cannonAngle + 45f;
            rightProjectile.transform.eulerAngles = new Vector3(0f, 0f, cannonAngle - 45f);
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
