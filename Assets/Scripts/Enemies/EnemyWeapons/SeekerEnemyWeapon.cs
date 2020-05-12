using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerEnemyWeapon : EnemyWeapon {

    // This weapon follows the player 
    // and will disappear after a specified duration
    public float destroyDuration;

    [SerializeField]
    private Transform target;

    void Update()
    {
        Kinematics();
    }

    // The default shoot option inherited from Weapon
    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        if(GameObject.FindGameObjectWithTag("Player")!= null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject projectile =
            Instantiate(this.gameObject, ship.position, ship.rotation) as GameObject;
        // Set the transform just once since this shot  
        // only goes straight to the target
        projectile.GetComponent<SeekerEnemyWeapon>().target = target;
        Destroy(projectile, destroyDuration);
    }

    public override void Kinematics()
    {
        if(target != null)
        {
            if(gameObject.tag == "Projectile")
            {
                target = GameObject.Find("Boss3(Clone)").transform;
            }
            Vector2 direction = target.position - this.transform.position;
            this.GetComponent<Rigidbody2D>().velocity = speed * direction.normalized;
        }

    }

}
