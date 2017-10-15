using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerMissile : Weapon {

    // SeekerMissle: 
    // Targets an enemy and moves toward them
    // TODO: attach the target sprite to SeekerMissile instead of Enemies
    // TODO: abstract the speed to use the weapon.speed field

    public int shotCount = 3;
    public float destroyTimer = 5.0f;
    public GameObject targetSprite;     // Cross hair sprite to show what missle is targeting
    private GameObject target;
    private Enemy[] enemies; 
    
    void Update()
    {      
        Kinematics();
    }

    public override void Shoot(Transform ship, Transform leftFire, Transform rightFire)
    {
        GameObject leftProj, rightProj;

        enemies = FindObjectsOfType<Enemy>();
        // Fire the missle only if there are enemies around
        if (enemies.Length > 0)
        {
            for (int i = 0; i < shotCount; ++i)
            {
                leftProj = Instantiate(this.gameObject, leftFire.position, leftFire.rotation) as GameObject;
                rightProj = Instantiate(this.gameObject, rightFire.position, rightFire.rotation) as GameObject;

                // Destroy the seeker after a certain time to avoid too much on screen
                DestroyObject(leftProj, destroyTimer);
                DestroyObject(rightProj, destroyTimer);
            }
            SoundController.Play((int)SFX.ShipLaserFire, 0.3f);
        }
    }

    public override void Kinematics()
    {
        // Need to update the enemies list each time so that missiles can redirect 
        enemies = FindObjectsOfType<Enemy>();

        // Find a new target for the missile
        if (target == null)
        {
            if (enemies.Length > 0)
            {
                int random = UnityEngine.Random.Range(0, enemies.Length);

                // TODO handle the target crosshair sprite so that it scales with the object
                target = enemies[random].gameObject;
                GameObject tempTarget =
                    Instantiate(targetSprite, target.transform.position, Quaternion.identity) as GameObject;
                tempTarget.transform.parent = target.transform;
                // Get the enemies scale
                targetSprite.transform.localScale = enemies[random].gameObject.GetComponent<Renderer>().bounds.size;
            }
            // If there are no targets just have the missile slow down and destroy itself 
            else
            {
                // Slow down the missile 

                // Shoot explosion effect
                Vector2 velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
                if (Mathf.Approximately(velocity.x, 0.0f) && Mathf.Approximately(velocity.y, 0.0f))
                {
                    DestroyObject(gameObject);
                }
            }
        }
        else
        {
            float deltaXChasingMissiles = target.transform.position.x - gameObject.transform.position.x;
            float deltaYChasingMissiles = target.transform.position.y - gameObject.transform.position.y;
            float angleChasingMissiles = Mathf.Atan2(deltaYChasingMissiles, deltaXChasingMissiles);
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(10 * Mathf.Cos(angleChasingMissiles), 10 * Mathf.Sin(angleChasingMissiles));
            gameObject.transform.eulerAngles = new Vector3(0f, 0f, angleChasingMissiles * Mathf.Rad2Deg - 90f);
            if (targetSprite != null)
            {
                targetSprite.transform.eulerAngles += new Vector3(0f, 0f, 360 * Time.deltaTime);
            }
        }
    }
}
