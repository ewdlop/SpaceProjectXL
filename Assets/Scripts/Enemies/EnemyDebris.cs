using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDebris : Enemy {

    // Should be placed onto debris objects that can 
    // only deal physical damage to the player

    public GameObject objectToSpawn;
    public int numberToSpawn;

    [Header("Spawn child object settings")]
    public float maxSpeed = 2.0f;
    public float maxAngle = 120.0f;
    private float speed;
    private float angle;

    new void Start ()
    {
        base.Start();
	}
	
	void Update ()
    {
        //Kinematics();

        if (health <= 0)
        {
            Destroy(this.gameObject);
            GameController.playerScore += score;

            // Spawn child debris objects when parent debris is destroyed 
            // if it has a child debris object
            if (objectToSpawn != null)
            {
                SpawnObjects();
            }
        }
    }

    // Spawn our child objects upon debris being destroyed
    private void SpawnObjects()
    {
        for (int i = 0; i < numberToSpawn; ++i)
        {
            GameObject temp = Instantiate(objectToSpawn,
                new Vector3(transform.position.x, transform.position.y, transform.position.z),
                Quaternion.identity);

            temp.GetComponent<EnemyDebris>().angle = UnityEngine.Random.Range(0.0f, maxAngle);
            temp.GetComponent<EnemyDebris>().speed = UnityEngine.Random.Range(0.2f, maxSpeed);
        }
    }

    protected override void Kinematics()
    {
        GetComponent<Rigidbody2D>().velocity =
               new Vector2(speed * Mathf.Cos((360.0f / numberToSpawn + angle) * Mathf.PI / 180.0f),
                           speed * Mathf.Sin((360.0f / numberToSpawn + angle) * Mathf.PI / 180.0f));
    }

    /*
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            health -= other.gameObject.GetComponent<Weapon>().damage;
            float healthPercentage = Mathf.Clamp((float)health / (float)maxHealth, 0.0f, 1.0f);
            renderer.material.SetFloat("_OcclusionStrength", 1.0f - healthPercentage);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            Instantiate(other.gameObject.GetComponent<Weapon>().hiteffect,
               new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, -0.01f),
               Quaternion.identity);

            Destroy(other.gameObject);
            StartCoroutine(HitFlash());
            health--;
        }
    }
    */
}
