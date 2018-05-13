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
        }

        if (rightFire != null)
        {
            rightProjectile = Instantiate(this.gameObject, rightFire.position,
                 rightFire.rotation) as GameObject;
            rightProjectile.GetComponent<CircleMissle>().isFiredFromRight = true;
            rightProjectile.GetComponent<CircleMissle>().ship = ship.gameObject;
        }
        SoundController.Play((int)SFX.ShipLaserFire, 0.3f);
    }

    public override void Kinematics()
    {
        time += Time.deltaTime;
        float angularSpeed = 5f;
        float phase = -1 * Mathf.PI / 2f;
        if (isFiredFromRight)
        {
            phase *= -1;
        }
        float speedX2 = -1 * angularSpeed * 10 * time * Mathf.Sin(angularSpeed * time + phase + Mathf.PI / 2) + 10 * Mathf.Cos(angularSpeed * time + phase + Mathf.PI / 2);
        float speedY2 = angularSpeed * 10 * time * Mathf.Cos(angularSpeed * time + phase + Mathf.PI / 2) + 10 * Mathf.Sin(angularSpeed * time + phase + Mathf.PI / 2);
        if (ship!=null)
        {
            float positionX = ship.transform.position.x + 10 * time * Mathf.Cos(angularSpeed * time + phase + Mathf.PI / 2);
            float positionY = ship.transform.position.y + 10 * time * Mathf.Sin(angularSpeed * time + phase + Mathf.PI / 2);
            gameObject.transform.position = new Vector2(positionX, positionY);
            velocityAngle = Mathf.Atan2(speedY2, speedX2);
            transform.eulerAngles = new Vector3(0f, 0f, velocityAngle * Mathf.Rad2Deg - 180f);
        }
        else
        {
            float positionX = ship.transform.position.x + 10 * time * Mathf.Cos(speedX2);
            float positionY = ship.transform.position.y + 10 * time * Mathf.Sin(speedY2);
            gameObject.transform.position = new Vector2(positionX, positionY);
        }
    }

}
