using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipAction : MonoBehaviour
{
    public float time;
    private List<EnemyWeapon> enemyWeaponList;

    void Update()
    {
        time += Time.deltaTime;
        string enemyName = gameObject.name;
        /*
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
        */
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
