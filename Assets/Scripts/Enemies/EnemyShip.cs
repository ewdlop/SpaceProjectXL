using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Enemy {

    [Header("Weapons")]
    public GameObject[] enemyWeaponArray = new GameObject[5];
    public bool[] unlockWeapons = new bool[5];
    [Header("Shooting settings")]
    public Transform firePosition;

    private List<GameObject> enemyWeaponList = new List<GameObject>();
    private PlayerController[] players;

    // Use this for initialization
    protected new void Start () {
        base.Start(); // Setup the hit flash
        players = FindObjectsOfType<PlayerController>();	

        // Convert the array to list 
        for (int i = 0; i < enemyWeaponArray.Length; ++i)
        {
            // Check that the weapon is valid
            if (enemyWeaponArray[i] != null)
            {
                enemyWeaponArray[i].GetComponent<Weapon>().isUnlocked = unlockWeapons[i];
                enemyWeaponList.Add(enemyWeaponArray[i]);
               
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        players = FindObjectsOfType<PlayerController>();

        // Control movement 
        // Will have bug if there's two players 
        // Since we could potentially target a different player
        // in the shoot function while having the ship direct to player1
        Kinematics();

        // Shoot weapon
        foreach (GameObject weaponObject in enemyWeaponList)
        {
            EnemyWeapon weapon = weaponObject.GetComponent<EnemyWeapon>();
            // Fire only if the weapon is off cooldown,
            // hence the weapon is unlocked for this enemy
            if (weapon.isUnlocked)
            {
                // Choose a random player to shoot at
                if (players.Length > 0)
                {
                    int pIdx = UnityEngine.Random.Range(0, players.Length);
                    weapon.Shoot(firePosition, players[pIdx].gameObject.transform);
                }
                StartCoroutine(FireCooldown(weapon));
            }  
        }

        if (health <= 0)
        {
            Death();
        }
    }

    protected override void Kinematics()
    {
        if (players.Length > 0)
        {
            float deltaX = players[0].gameObject.transform.position.x - gameObject.transform.position.x;
            float deltaY = players[0].gameObject.transform.position.y - gameObject.transform.position.y;

            float facingAngle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, facingAngle + 90f);
        }
    }

    IEnumerator FireCooldown(EnemyWeapon weapon)
    {
        weapon.isUnlocked = false;
        yield return new WaitForSeconds(weapon.cooldown);
        weapon.isUnlocked = true;
    }

    IEnumerator FireDelay(EnemyWeapon weapon, float angle)
    {
        for (int i = 0; i < weapon.totalShots; i++)
        {
            /*
            switch (weaponName)
            {
                case "Boss1RedBall":

                    ball.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y, -0.01f);
                    ball.GetComponent<ProjectileController>().playerSpaceShipX = transform.position.x;
                    ball.GetComponent<ProjectileController>().playerSpaceShipY = transform.position.y;
                    break;
                case "Boss1YellowBall":

                    ball.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y, -0.01f);
                    ball.GetComponent<Rigidbody2D>().velocity = ball.GetComponent<ProjectileController>().speed * new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                    break;
                case "Boss1BlueBall":
                    ball.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y, -0.01f);
                    ball.GetComponent<Rigidbody2D>().velocity = ball.GetComponent<ProjectileController>().speed * new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                    break;
                case "Boss1GreenBall"://need to fix a small bug later
                    if (gameObject != null)
                        ball.GetComponent<Rigidbody2D>().velocity = playerShip.GetComponent<Rigidbody2D>().velocity + ball.GetComponent<ProjectileController>().speed * new Vector2(Mathf.Cos((gameObject.transform.eulerAngles.z + 270f) *
                         Mathf.Deg2Rad), Mathf.Sin((gameObject.transform.eulerAngles.z + 270f) * Mathf.Deg2Rad));
                    break;
                case "Boss2BrownBall":
                    ball.GetComponent<ProjectileController>().playerSpaceShip = playerShip;

                    break;
                default:
                    ball.GetComponent<Rigidbody2D>().velocity = playerShip.GetComponent<Rigidbody2D>().velocity + ball.GetComponent<ProjectileController>().speed * new Vector2(Mathf.Cos(angle *
                     Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                    break;
            }
            */
            yield return new WaitForSeconds(weapon.fireDelay);
            
        }
    }

    void Death()
    {
        SoundController.Play((int)SFX.ShipDeath);
        Destroy(this.gameObject);
    }

}
