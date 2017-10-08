using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {
 
    public GameObject hiteffect;
    public string name;
    public int damage;
    public float launchAngle;
    public float speed = 50.0f;
    public bool isUnlocked;
    
    // Handles the projectile's movement
    public abstract void Kinematics();
}
