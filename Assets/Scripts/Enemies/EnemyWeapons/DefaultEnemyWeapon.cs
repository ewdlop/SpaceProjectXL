using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemyWeapon : EnemyWeapon {

    // This is just a standard, shoot-at-player weapon
    private Transform target; 

    void Update()
    {
        Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        return;
    }

    public override void Shoot(Transform ship, Transform target)
    {
        GameObject projectile =
            Instantiate(this.gameObject, ship.position, ship.rotation) as GameObject;
        // Set the transform just once since this shot  
        // only goes straight to the target
        projectile.GetComponent<DefaultEnemyWeapon>().target = target;
    }

    public override void Kinematics()
    {
        Vector2 direction = target.position - this.transform.position;
        this.GetComponent<Rigidbody2D>().velocity = speed * direction;
    }

}
