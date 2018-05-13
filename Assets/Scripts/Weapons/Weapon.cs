using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {
 
    public GameObject hiteffect;
    public int damage = 5;
    public float launchAngle = 0;
    public float speed = 50.0f;
    public bool isUnlocked = true;
    public float cooldown = 0.2f;
    public float cooldownStamp = 0.0f;
    //public int debugLog = 0;

    [Header("Wave Shot Settings")]  // This is mostly gonna be used with 
    public int totalShots = 1;      // the number of shots for each wave
    public float shotDelay = 0.2f;  // the delay between each shot in the wave

    // Handles where/how the object is instantiated
    public abstract void Shoot(Transform ship, Transform leftFire, Transform rightFire);
    // Handles the projectile's movement
    public abstract void Kinematics();
}
