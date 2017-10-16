using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {
 
    public GameObject hiteffect;
    public int damage = 5;
    public float launchAngle = 0;
    public float speed = 50.0f;
    public bool isUnlocked;

    // Handles where/how the object is instantiated
    public abstract void Shoot(Transform ship, Transform leftFire, Transform rightFire);
    // Handles the projectile's movement
    public abstract void Kinematics();
}
