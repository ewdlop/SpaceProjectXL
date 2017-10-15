using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Enemy {

    [Header("Weapons")]
    public GameObject[] enemyWeaponList = new GameObject[5];
    [Header("Shooting settings")]
    public Transform firePosition;

    private PlayerController[] players;

    // Use this for initialization
    new void Start () {
        base.Start(); // Setup the hit flash
        players = FindObjectsOfType<PlayerController>();	
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
                int pIdx = UnityEngine.Random.Range(0, players.Length);
                weapon.Shoot(firePosition, players[pIdx].gameObject.transform);
                
            }
            StartCoroutine(FireCooldown(weapon));
        }
    }

    protected override void Kinematics()
    {
        float deltaX = players[0].gameObject.transform.position.x - gameObject.transform.position.x;
        float deltaY = players[0].gameObject.transform.position.y - gameObject.transform.position.y;

        float facingAngle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, facingAngle + 90f);
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

    IEnumerator FireCooldown(EnemyWeapon weapon)
    {
        weapon.isUnlocked = false;
        yield return new WaitForSeconds(weapon.cooldown);
        weapon.isUnlocked = true;
    }
}
