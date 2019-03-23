using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRcoketMissile : Weapon {

    public float time;
    public float forwardThrust;
    public float speedX;
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
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(launchAngle * Mathf.PI / 180f) * speed,
            Mathf.Sin(launchAngle * Mathf.PI / 180f) * speed), ForceMode2D.Impulse);
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
        redRocketLeft.GetComponent<Weapon>().launchAngle = ship.GetComponent<PlayerController>().cannonAngle;
        redRocketRight.GetComponent<Weapon>().launchAngle = ship.GetComponent<PlayerController>().cannonAngle;
        redRocketLeft.transform.eulerAngles = new Vector3(0f, 0f, ship.GetComponent<PlayerController>().cannonAngle - 90f);
        redRocketRight.transform.eulerAngles = new Vector3(0f, 0f, ship.GetComponent<PlayerController>().cannonAngle - 90f);
        redRocketLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos((ship.GetComponent<PlayerController>().cannonAngle + 90f) * Mathf.PI / 180f) * speedX,
            Mathf.Sin((ship.GetComponent<PlayerController>().cannonAngle + 90f) * Mathf.PI / 180f) * speedX);
        redRocketRight.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos((ship.GetComponent<PlayerController>().cannonAngle - 90f) * Mathf.PI / 180f) * speedX, 
            Mathf.Sin((ship.GetComponent<PlayerController>().cannonAngle - 90f) * Mathf.PI / 180f) * speedX);
    }
}
