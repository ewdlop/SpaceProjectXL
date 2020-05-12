using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkCloud : Weapon{
    private void Update()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f, GetComponent<SpriteRenderer>().color.a - 2 * Time.deltaTime);
    }

    public override void Kinematics()
    { }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject darkCloud = Instantiate(gameObject,
         new Vector3(ship.position.x, ship.position.y, 0.01f),
         ship.rotation);
        darkCloud.GetComponent<Rigidbody2D>().velocity = speed * new Vector2(Mathf.Cos((ship.GetComponent<PlayerController>().cannonAngle - 180f) * Mathf.Deg2Rad),
                                                                             Mathf.Sin((ship.GetComponent<PlayerController>().cannonAngle - 180f) * Mathf.Deg2Rad));
        Destroy(darkCloud, 0.5f);
    }
}
