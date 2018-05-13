using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The index of the enum is the what needs to be set in the actionsList for enemyships
// Every new action will need to be added to enum,
// Mapped to the enemyship's action list
// And a function needs to be made for each seperate
[Serializable]
public enum EnemyAction
{
    // Movement functions
    moveDown = 0,          
    chargeStraight,
    followPlayer,
    movePeriodic,

    // Rotation functions
    rotateToPlayer = 4,

    // Non-movement functions
    shoot = 5,

    // Hybrid functions (movement + something else)
    moveDownAndShoot = 6,
    rotateTowardPlayerAndShoot,
    followPlayerAndShoot
};

// Contains all the actions available to enemy ships
public class EnemyActions : MonoBehaviour
{
    // shipPerformingAction parameter is the ship that is performing the action
    public static void runAction(GameObject shipPerformingAction)
    {
        if (shipPerformingAction.GetComponent<EnemyShip>() == null)
        {
            // The gameobject passed in is not an enemyship so just return
            return;
        }

        int[] actionsList = shipPerformingAction.GetComponent<EnemyShip>().actionsList;

        if (actionsList == null || actionsList.Length == 0)
        {
            // if no action is added to the enemy ship, then just have it move straight down
            //MoveDown(shipPerformingAction);
        }

        // Certain ships might have multiple actions, i.e boss ships that have multiple actions
        for (int i = 0; i < actionsList.Length; ++i)
        {
            // TODO somehow wait a bit before performing the next action
            // this is more for bosses
            switch (actionsList[i])
            {
                case (int)EnemyAction.moveDown:
                    Debug.Log("MoveDown");
                    MoveDown(shipPerformingAction);
                    break;
                case (int)EnemyAction.moveDownAndShoot:
                    Debug.Log("MoveDownAndShoot");
                    MoveDownAndShoot(shipPerformingAction);
                    break;
                case (int)EnemyAction.chargeStraight:
                    Debug.Log("ChargeStraight");
                    break;
                case (int)EnemyAction.followPlayer:
                    Debug.Log("FollowPlayer");
                    break;
                // Default action will be just moving down
                case (int)EnemyAction.shoot:
                    Debug.Log("Shoot");
                    Shoot(shipPerformingAction);
                    break;
                case (int)EnemyAction.rotateToPlayer:
                    Debug.Log("RotateToPlayer");
                    RotateToPlayer(shipPerformingAction);
                    break;
                case (int)EnemyAction.rotateTowardPlayerAndShoot:
                    Debug.Log("RotateTowardPlayerAndShoot");
                    RotateTowardPlayerAndShoot(shipPerformingAction);
                    break;
                default:
                    Debug.Log("Default");
                    //MoveDown(shipPerformingAction);
                    break;
            }
        }
    }

    public static void MoveDown(GameObject shipPerformingAction)
    {        
        Rigidbody2D rb2d = shipPerformingAction.GetComponent<Rigidbody2D>();
        //rb2d.AddForce(new Vector2(0f,-5f),ForceMode2D.Impulse);//a kick
        rb2d.velocity = new Vector2(0.0f, -shipPerformingAction.GetComponent<EnemyShip>().speed);

        /*
        shipPerformingAction.transform.position = 
            new Vector3(0.0f, -shipPerformingAction.GetComponent<EnemyShip>().speed * Time.deltaTime, 0.0f);
        */
    }

    public static void RotateToPlayer(GameObject shipPerformingAction)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            float deltaX = player.gameObject.transform.position.x - shipPerformingAction.transform.position.x;
            float deltaY = player.gameObject.transform.position.y - shipPerformingAction.transform.position.y;
            float facingAngle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;
            float shipFacingAngle = shipPerformingAction.transform.eulerAngles.z;
            shipPerformingAction.transform.eulerAngles = shipPerformingAction.transform.eulerAngles + new Vector3(0f, 0f, (facingAngle - shipFacingAngle) + 90f);
        }
    }

    public static void Shoot(GameObject shipPerformingAction)
    {
        // TODO instantiate the weapon to shoot at player
        shipPerformingAction.GetComponent<EnemyShip>().Fire();
    }

    public static void RotateTowardPlayerAndShoot(GameObject shipPerformingAction)
    {
        RotateToPlayer(shipPerformingAction);
        Shoot(shipPerformingAction);
    }

    public static void MoveDownAndShoot(GameObject shipPerformingAction)
    {
        MoveDown(shipPerformingAction);
        Shoot(shipPerformingAction);
    }
}
