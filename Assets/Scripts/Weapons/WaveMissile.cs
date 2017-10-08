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

    public override void Instantiate(Transform ship, Transform leftFire, Transform rightFire)
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
            rightProjectile.GetComponent<WaveMissile>().isFiredFromRight = true;
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
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speed * Mathf.Cos(Mathf.PI / 2) - speedY * Mathf.Sin(Mathf.PI / 2), speed * Mathf.Sin(Mathf.PI / 2) + speedY * Mathf.Cos(Mathf.PI / 2));
        velocityAngle = Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, gameObject.GetComponent<Rigidbody2D>().velocity.x);
        transform.eulerAngles = new Vector3(0f, 0f, velocityAngle * Mathf.Rad2Deg - 180f);
    }
}
