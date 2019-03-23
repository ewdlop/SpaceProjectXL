using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDebris : Enemy {

    // Should be placed onto debris objects that can 
    // only deal physical damage to the player

    public GameObject objectToSpawn;
    public int numberToSpawn;
    private bool isDebris;
    [Header("Spawn child object settings")]
    public float maxSpeed = 2.0f;
    public float maxAngle = 120.0f;

    float speed;
    float angle;

    new void Start ()
    {
        base.Start();
        // Initialize the debris's movement just once
        // The movement will typically be some random movement downwards
        if(!isDebris)
            Kinematics();
	}
	
	void Update ()
    {

        if (health <= 0)
        {
            PlaySoundBySize();
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

    private void PlaySoundBySize()
    {
        if (objectToSpawn == null)
        {
            SoundController.Play((int)SFX.RockBreakSmall);
        }
        else
        {
            if (numberToSpawn > 3)
            {
                SoundController.Play((int)SFX.RockBreakLarge);
            }
            else
            {
                SoundController.Play((int)SFX.RockBreakMedium);
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

            angle = UnityEngine.Random.Range(0.0f, maxAngle);
            speed = UnityEngine.Random.Range(0.2f, maxSpeed);
            temp.GetComponent<EnemyDebris>().isDebris = true;
            // Update the speed of the object
            temp.GetComponent<Rigidbody2D>().velocity =
               new Vector2(speed * Mathf.Cos((360.0f * i / numberToSpawn + angle) * Mathf.PI / 180.0f),
                           // Take into account the relative speed of the ship, so objects moving away will only slowly move away
                           (GameController.instance.scrollSpeed - speed) * Mathf.Sin((360.0f* i / numberToSpawn + angle) * Mathf.PI / 180.0f));
        }
    }

    protected override void Kinematics()
    {        
        GetComponent<Rigidbody2D>().velocity = 
            new Vector2(3.0f * UnityEngine.Random.Range(-1.0f, 1.0f), GameController.instance.scrollSpeed);

    }

    new void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Weapon>())
        {
            Instantiate(other.gameObject.GetComponent<Weapon>().hiteffect,
               new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, -0.01f),
               Quaternion.identity);
            float healthPercentage = Mathf.Clamp((float)health / (float)maxHealth, 0.0f, 1.0f);
            renderer.material.SetFloat("_OcclusionStrength", 1.0f - healthPercentage);
            if(other.gameObject.tag!="Slicer")
                Destroy(other.gameObject);
            health -= other.gameObject.GetComponent<Weapon>().damage;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Slicer")
        {
            Instantiate(other.gameObject.GetComponent<Weapon>().hiteffect,
               new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -2f),
               Quaternion.identity);
            float healthPercentage = Mathf.Clamp((float)health / (float)maxHealth, 0.0f, 1.0f);
            renderer.material.SetFloat("_OcclusionStrength", 1.0f - healthPercentage);
            health -= other.gameObject.GetComponent<Weapon>().damage;
        }
    }

}
