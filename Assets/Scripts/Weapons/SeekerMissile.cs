using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerMissile : Weapon {

    // SeekerMissle: 
    // Targets an enemy and moves toward them
    // TODO: attach the target sprite to SeekerMissile instead of Enemies

    public int shotCount = 3;
    public GameObject targetSprite;     // Cross hair sprite to show what missle is targeting
    private GameObject target;
    private Destructibles[] enemies; 

    void Update()
    {      
        Kinematics();
    }

    public override void Instantiate(Transform ship, Transform leftFire, Transform rightFire)
    {
        for (int i = 0; i < shotCount; ++i)
        {
            Instantiate(this.gameObject, leftFire.position, leftFire.rotation);
            Instantiate(this.gameObject, rightFire.position, rightFire.rotation);
        }
    }

    public override void Kinematics()
    {
        // Need to update the enemies list each time so that missiles can redirect 
        enemies = FindObjectsOfType<Destructibles>();

        if (target == null)
        {
            if (enemies.Length > 0)
            {
                int random = UnityEngine.Random.Range(0, enemies.Length);
                while (enemies[random].isItTakesDamage == false)
                {
                    random = UnityEngine.Random.Range(0, enemies.Length);
                }
                if (targetSprite != null)
                {
                    Destroy(targetSprite);
                }

                // TODO handle the target crosshair sprite so that it scales with the object
                target = enemies[random].gameObject;
                targetSprite = Instantiate(enemies[random].targetSprite, target.transform.position, Quaternion.identity);
                targetSprite.transform.parent = enemies[random].gameObject.transform;
                targetSprite.transform.localScale = enemies[random].targetSprite.transform.localScale;
                targetSprite.SetActive(true);
            }
            else
            {
                DestroyObject(gameObject);
                DestroyObject(gameObject.GetComponent<ProjectileController>().targetSprite);
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
