using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRcoketMissile : Weapon {

    public float time;
    public float forwardThrust;
    void Update()
    {
        Kinematics();
    }
    public override void Kinematics()
    {
        if (time < 0.3f && time >= 0)
            time += Time.deltaTime;
        else
        if (time >= 0.3f)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, forwardThrust), ForceMode2D.Impulse);
            time = -1;
        }

    }
    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject redRocketLeft = Instantiate(gameObject,
        new Vector3(ship.position.x, ship.position.y, 0.01f),
        ship.rotation);
        GameObject redRocketRight = Instantiate(gameObject,
        new Vector3(ship.position.x, ship.position.y, 0.01f),
        ship.rotation);
        redRocketLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * speed, 0f);
        redRocketRight.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0f);
    }
}
