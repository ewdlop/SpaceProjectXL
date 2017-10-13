using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
 
    public int maxHealth; 
    protected int health;     

    public abstract void Kinematics();
    public abstract void Shoot();
}
