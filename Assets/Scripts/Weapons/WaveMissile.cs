using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMissile : Weapon {

    // WaveMissle: 
    // Has a periodic wave motion 
    public bool isFiredFromRight;

    private float time;
    private float velocityAngle;

	void Start ()
    {
        velocityAngle = 0.0f;
        time = 1.0f / 60.0f; 
	}

    void Update ()
    {
        Kinematics();
    }

    public override void Kinematics()
    {
        time += Time.deltaTime;
        // TODO replace with the speed from WeaponController
        float speedX = 10f;
        float speedY = speedX * 3 * Mathf.Cos(speedX * time);
        if (isFiredFromRight)
        {
            speedY *= -1;
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speedX * Mathf.Cos(Mathf.PI / 2) - speedY * Mathf.Sin(Mathf.PI / 2), speedX * Mathf.Sin(Mathf.PI / 2) + speedY * Mathf.Cos(Mathf.PI / 2));
        velocityAngle = Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, gameObject.GetComponent<Rigidbody2D>().velocity.x);
        transform.eulerAngles = new Vector3(0f, 0f, velocityAngle * Mathf.Rad2Deg - 180f);
    }
}
