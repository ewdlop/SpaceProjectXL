using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangShot : Weapon {

    Transform playerShip;
    Vector2 direction;
    public override void Kinematics()
    {
        if (playerShip != null)
        {
            if (Vector2.Distance(transform.position, playerShip.position) <= 0.5f &&
                !playerShip.GetComponent<PlayerController>().IsDead)
            {
                 GetComponent<Rigidbody2D>().velocity = -1 * direction;
            }
            else
                GetComponent<Rigidbody2D>().AddForce(direction.normalized * Time.deltaTime * 20, ForceMode2D.Impulse);
        }
       
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject emitter = Instantiate(gameObject, ship.position + new Vector3(0f,1f,0f), Quaternion.identity);
        emitter.GetComponent<Rigidbody2D>().velocity = speed *
            new Vector2(Mathf.Cos(ship.GetComponent<PlayerController>().cannonAngle * Mathf.PI / 180f),
                        Mathf.Sin(ship.GetComponent<PlayerController>().cannonAngle * Mathf.PI / 180f));
        emitter.GetComponent<BoomerangShot>().playerShip = ship;
        emitter.GetComponent<BoomerangShot>().direction = -1 * emitter.GetComponent<Rigidbody2D>().velocity;
        DestroyObject(emitter, 7.65f);
    }

	void Update () {
        Kinematics();
    }
}
