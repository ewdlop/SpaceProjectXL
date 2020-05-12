using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMissle : Weapon {

    // CircleMissle: 
    // Missile spins around the player

    private float time;
    private float velocityAngle;
    private bool isFiredFromRight;
    private GameObject ship;

    void Start()
    {
        velocityAngle = 0.0f;
        time = 1.0f / 60.0f;
        Destroy(gameObject, 2f);
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
            leftProjectile = Instantiate(this.gameObject, leftFire.position,
                leftFire.rotation);
            leftProjectile.GetComponent<CircleMissle>().ship = ship.gameObject;
            leftProjectile.GetComponent<Weapon>().launchAngle = ship.GetComponent<PlayerController>().cannonAngle;
        }

        if (rightFire != null)
        {
            rightProjectile = Instantiate(this.gameObject, rightFire.position,
                 rightFire.rotation);
            rightProjectile.GetComponent<CircleMissle>().isFiredFromRight = true;
            rightProjectile.GetComponent<CircleMissle>().ship = ship.gameObject;
            rightProjectile.GetComponent<Weapon>().launchAngle = ship.GetComponent<PlayerController>().cannonAngle;
        }
        SoundController.Play((int)SFX.ShipLaserFire, 0.3f);
    }

    public override void Kinematics()
    {
        float launchAngletoRad = launchAngle * Mathf.PI / 180;
        time += Time.deltaTime;
        float angularSpeed = speed;
        float phase = -1 * Mathf.PI / 2f;
        if (isFiredFromRight)
        {
            phase *= -1;
        }
        //derivative
        float speedX2 = -1 * angularSpeed * 10 * time * Mathf.Sin(angularSpeed * time + phase + launchAngletoRad) + 10 * Mathf.Cos(angularSpeed * time + phase + launchAngletoRad);
        float speedY2 = angularSpeed * 10 * time * Mathf.Cos(angularSpeed * time + phase + launchAngletoRad) + 10 * Mathf.Sin(angularSpeed * time + phase + launchAngletoRad);
        if (ship!=null)
        {
            float positionX = ship.transform.position.x + 10 * time * Mathf.Cos(angularSpeed * time + phase + launchAngletoRad);
            float positionY = ship.transform.position.y + 10 * time * Mathf.Sin(angularSpeed * time + phase + launchAngletoRad);
            transform.position = new Vector2(positionX, positionY);
            velocityAngle = Mathf.Atan2(speedY2, speedX2);
            transform.eulerAngles = new Vector3(0f, 0f, velocityAngle * Mathf.Rad2Deg - 180f);
        }
        else
        {
            float positionX = transform.position.x + 10 * time * Mathf.Cos(velocityAngle);
            float positionY = transform.position.y + 10 * time * Mathf.Sin(velocityAngle);
            transform.position = new Vector2(positionX, positionY);
        }
    }

}
