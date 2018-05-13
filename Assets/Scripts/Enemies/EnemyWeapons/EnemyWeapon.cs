using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWeapon : Weapon
{
    /**
     * The inherited fields from base Weapon class
     * 
        public GameObject hiteffect;
        public int damage;
        public float launchAngle;
        public float speed = 50.0f;
        public bool isUnlocked;

        // Handles where/how the object is instantiated
        public abstract void Shoot(Transform ship, Transform leftFire, Transform rightFire);
        // Handles the projectile's movement
        public abstract void Kinematics();
     *
     **/
    public float fireDelay = 0.3f;  // the delay between each shot
    public int totalShots = 1;      // the number of shots for each wave    
    public float waveDelay = 1.0f;  // the delay between each wave

    public abstract void Shoot(Transform ship, Transform target);

}
