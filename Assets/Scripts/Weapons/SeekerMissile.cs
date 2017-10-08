using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerMissile : Weapon {

    // SeekerMissle: 
    // Targets an enemy and moves toward them

    public GameObject targetSprite;     // Cross hair sprite to show what missle is targeting
    private GameObject playerSpaceShip; 

    void Update()
    {
        Kinematics();
    }

    public override void Kinematics()
    {
        if (playerSpaceShip == null) //playerSpaceShip is the target
        {
            Destructibles[] enemies = FindObjectsOfType<Destructibles>();
            if (enemies.Length > 0)
            {
                int random = Random.Range(0, enemies.Length);
                int count = 0;
                while (enemies[random].isItTakesDamage == false && count < enemies.Length)//need to fix this later
                {
                    random = Random.Range(0, enemies.Length);
                    count++;
                }
                if (targetSprite != null)
                {
                    Destroy(targetSprite);
                }
                playerSpaceShip = enemies[random].gameObject;
                targetSprite = Instantiate(enemies[random].targetSprite, playerSpaceShip.transform.position, Quaternion.identity);
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
            float deltaXChasingMissiles = playerSpaceShip.transform.position.x - gameObject.transform.position.x;
            float deltaYChasingMissiles = playerSpaceShip.transform.position.y - gameObject.transform.position.y;
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
