using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerEnemyWeapon : EnemyWeapon {

    // This weapon follows the player 
    // and will disappear after a specified duration
    public float destroyDuration;
    
    private Transform target;

    void Update()
    {
        Kinematics();
    }

    // The default shoot option inherited from Weapon
    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        return;
    }

    // Shoot at target
    public override void Shoot(Transform ship, Transform target)
    {
        GameObject projectile =
            Instantiate(this.gameObject, ship.position, ship.rotation) as GameObject;
        // Set the transform just once since this shot  
        // only goes straight to the target
        projectile.GetComponent<SeekerEnemyWeapon>().target = target;
        DestroyObject(projectile, destroyDuration);
    }

    public override void Kinematics()
    {
        Vector2 direction = target.position - this.transform.position;
        this.GetComponent<Rigidbody2D>().velocity = speed * direction.normalized;
    }

}
