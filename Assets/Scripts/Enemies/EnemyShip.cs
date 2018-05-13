using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Enemy {

    [Header("Weapons")]
    public List<GameObject> enemyWeaponList;
    //public bool[] unlockWeapons = new bool[5];
    public int[] actionsList;
    public float actionsDelay = 0.5f;

    [Header("Shooting settings")]
    public Transform leftFirePosition;
    public Transform rightFirePosition;
    //private List<GameObject> enemyWeaponList = new List<GameObject>();
    private PlayerController player;

    [Header("Movement settings")]
    public float speed = 5.0f;

    private float actionsTimeStamp;
    private float timer;

    // Use this for initialization
    protected new void Start () {
        actionsTimeStamp = 0;
        base.Start(); // Setup the hit flash
        player = FindObjectOfType<PlayerController>();	

        // Convert the array to list 
        for (int i = 0; i < enemyWeaponList.Count; ++i)
        {
            enemyWeaponList[i].GetComponent<Weapon>().isUnlocked = true;      
            //enemyWeaponList.Add(enemyWeaponList[i]);                         
        }
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        // Control movement 
        // Will have bug if there's two players 
        // Since we could potentially target a different player
        // in the shoot function while having the ship direct to player1
        //Kinematics();
        RunAction();
        if (timer > actionsDelay)
        {
            //RunAction();            
            timer = 0;
        }
        
        if (health <= 0)
        {
            Death();
        }
    }

    public void Fire()
    {
        // Shoot weapon
        foreach (GameObject weaponObject in enemyWeaponList)
        {
            Weapon weapon = weaponObject.GetComponent<Weapon>();
            // Fire only if the weapon is off cooldown,
            // hence the weapon is unlocked for this enemy
            if (weapon == null) Debug.Log("weapon is null");
            //Debug.Log(weapon.debugLog++);
            if (weapon.isUnlocked)
            {
                //Debug.Log("weapon is unlocked " + weapon.debugLog);                
                weapon.Shoot(gameObject.transform, leftFirePosition, rightFirePosition);
                StartCoroutine(FireCooldown(weapon));
            }
        }
    }

    // Run the actions associated with that ship
    private void RunAction()
    {
        EnemyActions.runAction(this.gameObject);
    }

    protected override void Kinematics()
    {

    }

    IEnumerator FireCooldown(Weapon weapon)
    {
        weapon.isUnlocked = false;
        yield return new WaitForSeconds(weapon.cooldown);
        weapon.isUnlocked = true;
    }

    /*
    IEnumerator FireDelay(Weapon weapon, float angle)
    {
        for (int i = 0; i < weapon.totalShots; i++)
        {            
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
            
            yield return new WaitForSeconds(weapon.fireDelay);
            
        }
    }
    */

    void Death()
    {
        SoundController.Play((int)SFX.ShipDeath, 0.25f);
        Destroy(this.gameObject);
    }

}
