using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadLaser : Weapon {

    public int shotCount = 10; 
    private Vector2 shotDirection;

    void Update()
    {
        Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        for (int i = 0; i < shotCount; i++)
        {
            GameObject SpreadLaser = Instantiate(this.gameObject,
                new Vector3(ship.position.x, ship.position.y, 0.01f),
                ship.rotation);

            SpreadLaser.GetComponent<SpreadLaser>().shotDirection =
                new Vector2(Mathf.Cos((135f + ship.GetComponent<PlayerController>().cannonAngle - 90f) / 180f * Mathf.PI - 10f / 180f * Mathf.PI * i),
                Mathf.Sin((135f + ship.GetComponent<PlayerController>().cannonAngle - 90f) / 180f * Mathf.PI - 10f / 180f * Mathf.PI * i));
        }
        SoundController.Play((int)SFX.ShipLaserFire, 0.3f);
    }

    public override void Kinematics()
    {
        Vector2 relativeVelocity = speed * shotDirection;
        this.GetComponent<Rigidbody2D>().linearVelocity = relativeVelocity;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(relativeVelocity.y, relativeVelocity.x) * Mathf.Rad2Deg - 90f);
    }
}
