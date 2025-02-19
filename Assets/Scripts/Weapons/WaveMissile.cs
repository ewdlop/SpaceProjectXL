using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMissile : Weapon {

    // WaveMissle: 
    // Has a periodic wave motion 

    private float time;
    private float velocityAngle;
    private bool isFiredFromRight;

	void Start ()
    {
        velocityAngle = 0.0f;
        time = 1.0f / 60.0f; 
	}

    void Update ()
    {
        Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject leftrightProjectile;
        GameObject rightProjectile;  // Creating a temp object just to set the isRightSide variable
        if (leftFire != null)
        {
            leftrightProjectile = Instantiate(this.gameObject, leftFire.position,
                leftFire.rotation);
            leftrightProjectile.GetComponent<Weapon>().launchAngle = ship.GetComponent<PlayerController>().cannonAngle;
        }

        if (rightFire != null)
        {
            rightProjectile = Instantiate(this.gameObject, rightFire.position,
                 rightFire.rotation) as GameObject;
            rightProjectile.GetComponent<WaveMissile>().isFiredFromRight = true;
            rightProjectile.GetComponent<Weapon>().launchAngle = ship.GetComponent<PlayerController>().cannonAngle;
        }
        SoundController.Play((int)SFX.ShipLaserFire, 0.3f);
    }

    public override void Kinematics()
    {
        time += Time.deltaTime;
        float speedY = speed * 3 * Mathf.Cos(speed * time);
        if (isFiredFromRight)
        {
            speedY *= -1;
        }
        float launchAngletoRad = launchAngle * Mathf.PI/180f;
        gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(speed * Mathf.Cos(launchAngletoRad) - speedY * Mathf.Sin(launchAngletoRad), speed * Mathf.Sin(launchAngletoRad) + speedY * Mathf.Cos(launchAngletoRad));
        velocityAngle = Mathf.Atan2(GetComponent<Rigidbody2D>().linearVelocity.y, gameObject.GetComponent<Rigidbody2D>().linearVelocity.x);
        transform.eulerAngles = new Vector3(0f, 0f, velocityAngle * Mathf.Rad2Deg - 180f);
    }
}
