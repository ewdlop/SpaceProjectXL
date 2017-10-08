using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipAction : MonoBehaviour
{


    public float time;

    public float[] shotdeviationArray;
    private float shotDeviation;
    public GameObject playerShip;
    private List<EnemyWeapon> enemyWeaponList;


    void Start()
    {
        // Easy mode 
        if (GameController.difficulty == Difficulty.Easy)
            shotDeviation = shotdeviationArray[(int)Difficulty.Easy];
        // Medium mode
        else if (GameController.difficulty == Difficulty.Medium)
            shotDeviation = shotdeviationArray[(int)Difficulty.Medium];
        // Hard mode 
        else if (GameController.difficulty == Difficulty.Hard)
            shotDeviation = shotdeviationArray[(int)Difficulty.Hard];

        // Make value positive so we can use in Random.Range later on
        if (shotDeviation < 0)
            shotDeviation *= -1;

        //Set Up Weapons
        string enemyName = gameObject.name;
        switch (enemyName)
        {
            case "Boss1itSelf":
                if (playerShip != null)
                {
                    enemyWeaponList = EnemyWeaponSpawnManger.boss1WeaponList;
                }
                break;
            case "Boss2itSelf":
                if (playerShip != null)
                {
                    enemyWeaponList = EnemyWeaponSpawnManger.boss2WeaponList;
                }
                break;
            case "Boss1LeftTurret":
                if (playerShip != null)
                {
                    enemyWeaponList = EnemyWeaponSpawnManger.boss1LeftTurretWeaponList;
                }
                break;
            case "Boss1RightTurret":
                if (playerShip != null)
                {
                    enemyWeaponList = EnemyWeaponSpawnManger.boss1RightTurretWeaponList;
                }
                break;
            case "Boss1BackTurret":
                {
                    enemyWeaponList = EnemyWeaponSpawnManger.boss1BackTurretWeaponList;
                }
                break;
        }
    }
    void Update()
    {
        time += Time.deltaTime;
        string enemyName = gameObject.name;
        switch (enemyName)
        {
            case "Boss1itSelf":
                if (playerShip != null)
                {
                    float deltaX = playerShip.gameObject.transform.position.x - gameObject.transform.position.x;
                    float deltaY = playerShip.gameObject.transform.position.y - gameObject.transform.position.y;

                    float facingAngle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;
                    transform.eulerAngles = new Vector3(0, 0, facingAngle + 90f);
                    FireAtPlayer(facingAngle, playerShip);
                }
                break;
            case "Boss2itSelf":
                if (playerShip != null)
                {
                    float deltaX = playerShip.gameObject.transform.position.x - gameObject.transform.position.x;
                    float deltaY = playerShip.gameObject.transform.position.y - gameObject.transform.position.y;

                    float facingAngle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;
                    transform.eulerAngles = new Vector3(0, 0, facingAngle + 90f);
                    FireAtPlayer(facingAngle, playerShip);
                }
                break;
            case "Boss1LeftTurret":
                if (playerShip != null)
                {
                    transform.eulerAngles = new Vector3(0, 0, 100 * time);

                    FireAtPlayer(transform.eulerAngles.z + 270f, playerShip);
                }
                break;
            case "Boss1RightTurret":
                if (playerShip != null)
                {
                    transform.eulerAngles = new Vector3(0, 0, -100 * time);
                    FireAtPlayer(transform.eulerAngles.z + 270f, playerShip);
                }
                break;
            case "Boss1BackTurret":
                {
                    FireAtPlayer(transform.eulerAngles.z + 270f, playerShip);//let the blueball "decays" 4 (15/(duratio of the blue ball-1)-1) times before refire. 
                }
                break;
            case "enemyBlueShipRight(Clone)":
                transform.position = new Vector3(10 - 4 * time, 15f / (10f - 4 * time));
                break;
            case "enemyBlueShipLeft(Clone)":
                transform.position = new Vector3(-10 + 4 * time, -15f / (-10f + 4 * time), -0.01f);
                break;
            default:
                break;
        }

    }



    void FireAtPlayer(float angle, GameObject playerShip)
    {

        angle += Random.Range(-shotDeviation, shotDeviation);
        foreach (EnemyWeapon weapon in enemyWeaponList)
        {
            if (weapon.isCanShoot)
            {
                weapon.isCanShoot = false;

                StartCoroutine(FireCooldown(weapon, angle));
            }

        }


    }

    IEnumerator FireDelay(EnemyWeapon weapon, float angle)
    {
        for (int j = 0; j < weapon.numberOfShotsPerSet; j++)
        {
            GameObject ball = Instantiate(weapon.enemyWeaponPrefab, new Vector3(transform.position.x, transform.position.y, 0.01f), Quaternion.Euler(0, 0, angle - 90f));

            string weaponName = weapon.enemyWeaponPrefab.name;

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
                    if(gameObject!=null)
                    ball.GetComponent<Rigidbody2D>().velocity = playerShip.GetComponent<Rigidbody2D>().velocity + ball.GetComponent<ProjectileController>().speed * new Vector2(Mathf.Cos((gameObject.transform.eulerAngles.z+270f) *
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

    IEnumerator FireDelayBetweenSets(EnemyWeapon weapon, float angle)
    {
        for (int i = 0; i < weapon.numberOfSets; i++)
        {

            StartCoroutine(FireDelay(weapon, angle));
            yield return new WaitForSeconds(weapon.fireDelayBetweenSets);
        }


    }

    IEnumerator FireCooldown(EnemyWeapon weapon, float angle)
    {
        StartCoroutine(FireDelayBetweenSets(weapon, angle));
        yield return new WaitForSeconds(weapon.coolDown);
        weapon.isCanShoot = true;
    }

    //this was for the red ship with black circle.
    /*void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("PlayerShip") == true)
        {
            
            float deltaX = collision.gameObject.transform.position.x - gameObject.transform.position.x;
            float deltaY = collision.gameObject.transform.position.y - gameObject.transform.position.y;
            float facingAngle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg - 90f;
            transform.eulerAngles = new Vector3(0, 0, facingAngle);
            FireAtPlayer(facingAngle,collision.gameObject);
        }
    }*/
}
