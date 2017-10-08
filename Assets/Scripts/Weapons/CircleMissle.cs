using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMissle : Weapon {

    // CircleMissle: 
    // Spins around the player
    public bool isFiredFromRight;

    private float time;
    private float velocityAngle;

    void Start()
    {
        velocityAngle = 0.0f;
        time = 1.0f / 60.0f;
    }

    void Update()
    {
        Kinematics();
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
        float speedX2 = -1 * angularSpeed * 2 * Mathf.Sin(angularSpeed * time + phase + Mathf.PI / 2);
        float speedY2 = angularSpeed * 2 * Mathf.Cos(angularSpeed * time + phase + Mathf.PI / 2);
        float positionX = transform.position.x + 10 * time * Mathf.Cos(angularSpeed * time + phase + Mathf.PI / 2);
        float positionY = transform.position.y + 10 * time * Mathf.Sin(angularSpeed * time + phase + Mathf.PI / 2);
        gameObject.transform.position = new Vector2(positionX, positionY);

        velocityAngle = Mathf.Atan2(speedY2, speedX2);
        transform.eulerAngles = new Vector3(0f, 0f, velocityAngle * Mathf.Rad2Deg - 180f);
    }

}
