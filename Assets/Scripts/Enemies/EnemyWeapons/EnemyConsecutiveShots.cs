
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConsecutiveShots : EnemyWeapon{

    public bool isShootFromTransform;
    public bool isLaser;
    void Update()
    {
        //Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject Shot;
        if (isShootFromTransform)
        {
            Shot = Instantiate(this.gameObject,
                 new Vector3(leftFire.position.x, leftFire.position.y, -2f),
                 Quaternion.identity);
            if(isLaser)
                Shot.transform.parent = leftFire;
            Shot.transform.eulerAngles = new Vector3(0f, 0f, ship.eulerAngles.z - 180f);
        }
        else
        {
            Shot = Instantiate(this.gameObject,
                new Vector3(ship.position.x, ship.position.y, -2f),
                Quaternion.identity);
        }

        Vector2 relativeVelocity = speed * new Vector2(Mathf.Cos((ship.transform.eulerAngles.z - 90f) * Mathf.Deg2Rad),
                Mathf.Sin((ship.transform.eulerAngles.z - 90f)* Mathf.Deg2Rad));
        Shot.GetComponent<Rigidbody2D>().velocity = relativeVelocity;
        //SoundController.Play((int)SFX.ShipLaserFire, 0.3f);
    }

    public override void Kinematics()
    {
        
    }
}
