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
    patrol,      
    chargeStraight,
    followPlayer,
    moveTanh,
    // Rotation functions
    rotateToPlayer,
    
    // Non-movement functions
    shoot,

    // Hybrid functions (movement + something else)
    moveDownAndShoot,
    rotateTowardPlayerAndShoot,
    followPlayerAndShoot,
    moveCircle,
    moveCircleFacingOuter,
    moveCircleFacingCenter,
    
    rotateClockWise,
    rotateCounterClockWise,
    moveHyperbola,
    RandomHeightHorizontalMovement,
    RotatBetweenAngles,
    MoveLeftToRight,
    MoveRightToLeft
};

// Contains all the actions available to enemy ships
public class EnemyActions : MonoBehaviour
{
    public GameObject player;   

    // shipPerformingAction parameter is the ship that is performing the action
    public static void runAction(GameObject shipPerformingAction)
    {
        if (shipPerformingAction.GetComponent<EnemyShip>() == null)
        {
           
            // The gameobject passed in is not an enemyship so just return
            return;
        }

        List<EnemyAction> actionsList = shipPerformingAction.GetComponent<EnemyShip>().actionsList;

        if (actionsList == null || actionsList.Count == 0)
        {
            // if no action is added to the enemy ship, then just have it move straight down
            //MoveDown(shipPerformingAction);
        }

        // Certain ships might have multiple actions, i.e boss ships that have multiple actions
        for (int i = 0; i < actionsList.Count; ++i)
        {
            // TODO somehow wait a bit before performing the next action
            // this is more for bosses
            switch (actionsList[i])
            {
                case EnemyAction.moveDown:
                    MoveDown(shipPerformingAction);
                    break;
                case EnemyAction.patrol:
                    //Patrol(shipPerformingAction);
                    break;
                case EnemyAction.moveDownAndShoot:
                    MoveDownAndShoot(shipPerformingAction);
                    break;
                case EnemyAction.chargeStraight:
                    ChargeStraight(shipPerformingAction);
                    break;
                case EnemyAction.followPlayer:
                    FollowPlayer(shipPerformingAction);
                    break;
                case EnemyAction.followPlayerAndShoot:
                    FollowPlayer(shipPerformingAction);
                    Shoot(shipPerformingAction);
                    break;
                // Default action will be just moving down
                case EnemyAction.shoot:
                    Shoot(shipPerformingAction);
                    break;
                case EnemyAction.rotateToPlayer:
                    RotateToPlayer(shipPerformingAction);
                    break;
                case EnemyAction.rotateTowardPlayerAndShoot:
                    RotateTowardPlayerAndShoot(shipPerformingAction);
                    break;
                case EnemyAction.moveCircle:
                    MoveCircle(shipPerformingAction);
                    break;
                case EnemyAction.moveCircleFacingOuter:
                    MoveCircleFacingOuter(shipPerformingAction);
                    break;
                case EnemyAction.moveCircleFacingCenter:
                    MoveCircleFacingCenter(shipPerformingAction);
                    break;
                case EnemyAction.rotateClockWise:
                    RotateClockWise(shipPerformingAction);
                    break;
                case EnemyAction.rotateCounterClockWise:
                    RotateCounterClockWise(shipPerformingAction);
                    break;
                case EnemyAction.moveTanh:
                    MoveTanh(shipPerformingAction);
                    break;
                case EnemyAction.moveHyperbola:
                    MoveHyperbola(shipPerformingAction);
                    break;
                case EnemyAction.RandomHeightHorizontalMovement:
                    RandomHeightHorizontalMovement(shipPerformingAction);
                    break;
                case EnemyAction.RotatBetweenAngles:
                    RotateBetweenAngles(shipPerformingAction);
                    break;
                case EnemyAction.MoveLeftToRight:
                    MoveLeftToRight(shipPerformingAction);
                    break;
                case EnemyAction.MoveRightToLeft:
                    MoveRightToLeft(shipPerformingAction);
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
        rb2d.velocity = new Vector2(0.0f, -shipPerformingAction.GetComponent<EnemyShip>().speed);
    }

    public static void ChargeStraight(GameObject shipPerformingAction)
    {
        Rigidbody2D rb2d = shipPerformingAction.GetComponent<Rigidbody2D>();
        rb2d.AddForce(new Vector2(0f, -10 * Time.deltaTime),ForceMode2D.Impulse);
    }

    IEnumerator ChargeAndReturn(float delayActions)
    {

        yield return new WaitForSeconds(delayActions);
    }

    public static void ReturnToPosition(GameObject shipPerformingAction, Vector3 originalPos)
    {

    }

    public static void RotateToPlayer(GameObject shipPerformingAction)
    {
        if (shipPerformingAction.GetComponent<EnemyShip>().playerShip == null)
            shipPerformingAction.GetComponent<EnemyShip>().playerShip = GameObject.FindGameObjectWithTag("Player");
        GameObject player = shipPerformingAction.GetComponent<EnemyShip>().playerShip;
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
        shipPerformingAction.GetComponent<EnemyShip>().Fire();
    }
    public static void RotateClockWise(GameObject shipPerformingAction)
    {
        shipPerformingAction.transform.eulerAngles -= new Vector3(0f, 0f, 
            180f/shipPerformingAction.GetComponent<EnemyShip>().rotateSpeedFactor * Time.deltaTime);

    }
    public static void RotateCounterClockWise(GameObject shipPerformingAction)
    {
        shipPerformingAction.transform.eulerAngles += new Vector3(0f, 0f, 
            180f/ shipPerformingAction.GetComponent<EnemyShip>().rotateSpeedFactor * Time.deltaTime);
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

    public static void FollowPlayer(GameObject shipPerformingAction)
    {
        RotateToPlayer(shipPerformingAction);
        if (shipPerformingAction.GetComponent<EnemyShip>().playerShip == null)
            shipPerformingAction.GetComponent<EnemyShip>().playerShip = GameObject.FindGameObjectWithTag("Player");
        GameObject player = shipPerformingAction.GetComponent<EnemyShip>().playerShip;
        float speed = shipPerformingAction.GetComponent<EnemyShip>().speed;
        if (player != null)
        {
            Vector2 difference = (player.transform.position - shipPerformingAction.transform.position);
            difference = speed * 0.05f / difference.magnitude * difference;
            shipPerformingAction.transform.position += new Vector3(difference.x, difference.y, 0f);
        }
        else
            DestroyObject(shipPerformingAction);
    }
    public static void MoveCircle(GameObject shipPerformingAction)
    {
        float radius = 5f;
        float angularSpeed = 60f * Mathf.PI / 180f;
        float shipFacingRadian = (shipPerformingAction.transform.eulerAngles.z + 180f) * Mathf.PI / 180f;
        float deltaX = Mathf.Cos(shipFacingRadian + angularSpeed * Time.deltaTime) - Mathf.Cos(shipFacingRadian);
        float deltaY = Mathf.Sin(shipFacingRadian + angularSpeed * Time.deltaTime) - Mathf.Sin(shipFacingRadian);
        shipPerformingAction.transform.position += radius * new Vector3(deltaX, deltaY, 0f);
        shipPerformingAction.transform.eulerAngles += new Vector3(0f, 0f, angularSpeed * Time.deltaTime * 180f / Mathf.PI);
    }
    public static void MoveCircleFacingOuter(GameObject shipPerformingAction)
    {
        float radius = 5f;
        float angularSpeed = 60f * Mathf.PI / 180f;
        float shipFacingRadian = (shipPerformingAction.transform.eulerAngles.z - 90f) * Mathf.PI / 180f;
        float deltaX = Mathf.Cos(shipFacingRadian + angularSpeed * Time.deltaTime) - Mathf.Cos(shipFacingRadian);
        float deltaY = Mathf.Sin(shipFacingRadian + angularSpeed * Time.deltaTime) - Mathf.Sin(shipFacingRadian);
        shipPerformingAction.transform.position += radius * new Vector3(deltaX, deltaY, 0f);
        shipPerformingAction.transform.eulerAngles += new Vector3(0f, 0f, angularSpeed * Time.deltaTime * 180f / Mathf.PI);
    }

    public static void MoveCircleFacingCenter(GameObject shipPerformingAction)
    {
        float radius = 5f;
        float angularSpeed = 60f * Mathf.PI/180f;
        float shipFacingRadian= (shipPerformingAction.transform.eulerAngles.z + 90f) * Mathf.PI / 180f;
        float deltaX = Mathf.Cos(shipFacingRadian + angularSpeed * Time.deltaTime) - Mathf.Cos(shipFacingRadian);
        float deltaY = Mathf.Sin(shipFacingRadian + angularSpeed * Time.deltaTime) - Mathf.Sin(shipFacingRadian);
        shipPerformingAction.transform.position += radius * new Vector3(deltaX,deltaY,0f);
        shipPerformingAction.transform.eulerAngles += new Vector3(0f, 0f, angularSpeed * Time.deltaTime * 180f/Mathf.PI);
    }
    
    public static void MoveTanh(GameObject shipPerformingAction)
    { 
        //y = 7tanh(x)
        shipPerformingAction.transform.position += new Vector3(
            Time.deltaTime * shipPerformingAction.GetComponent<EnemyShip>().speed
            , 7f * (float)(1 - Math.Tanh(shipPerformingAction.transform.position.x)
            * Math.Tanh(shipPerformingAction.transform.position.x)) * Mathf.Abs(shipPerformingAction.GetComponent<EnemyShip>().speed) * Time.deltaTime, 0f);
    }

    public static void MoveHyperbola(GameObject shipPerformingAction)
    {   
        //y = 0.25 * sqrt(x^2 + 1)
        float dxdt = shipPerformingAction.GetComponent<EnemyShip>().speed;
        float dydx = 0.5f * shipPerformingAction.transform.position.x 
            / (2f * Mathf.Sqrt(shipPerformingAction.transform.position.x 
            * shipPerformingAction.transform.position.x + 1f));
        //slope=dy/dx=angle lol
        float angle = Mathf.Atan2(dydx * dxdt, dxdt) * 180f / Mathf.PI;
        shipPerformingAction.transform.position += new Vector3(dxdt * Time.deltaTime, dydx * dxdt * Time.deltaTime, 0f);
        shipPerformingAction.transform.eulerAngles = new Vector3(0f, 0f, angle + 90f);
    }
    
    public static void RandomHeightHorizontalMovement(GameObject shipPerformingAction)
    {
        float speedX = shipPerformingAction.GetComponent<EnemyShip>().speed;
        if (!shipPerformingAction.GetComponent<EnemyShip>().isRightToLeft)
        {
            shipPerformingAction.GetComponent<Rigidbody2D>().velocity = speedX * new Vector2(1f, 0f);
            shipPerformingAction.transform.position = new Vector2(shipPerformingAction.transform.position.x,
                shipPerformingAction.GetComponent<EnemyShip>().yRange);
            shipPerformingAction.transform.eulerAngles = new Vector3(0f, 0f, 90f);
        }
        else
        {
            shipPerformingAction.GetComponent<Rigidbody2D>().velocity = speedX * new Vector2(-1f, 0f);
            shipPerformingAction.transform.position = new Vector2(shipPerformingAction.transform.position.x,
                shipPerformingAction.GetComponent<EnemyShip>().yRange);
            shipPerformingAction.transform.eulerAngles = new Vector3(0f, 0f, -90f);
        }
    }
    public static void RotateBetweenAngles(GameObject shipPerformingAction)
    {
        float offset = 90f;
        float roateSpeed = shipPerformingAction.GetComponent<EnemyShip>().rotateSpeed * Mathf.PI / 180f;
        float angleA = shipPerformingAction.GetComponent<EnemyShip>().angleA + offset;
        float angleB = shipPerformingAction.GetComponent<EnemyShip>().angleB + offset;
        float start = (angleB + angleA) / 2f;
        float amplitude = (angleB - angleA)/2f;
        float phase = shipPerformingAction.GetComponent<EnemyShip>().phase * Mathf.PI / 180f;
        shipPerformingAction.transform.eulerAngles= new Vector3(0f, 0f, 
            amplitude * Mathf.Sin(roateSpeed * shipPerformingAction.GetComponent<EnemyShip>().timer +
            phase) + start);
    }
    
    public static void MoveLeftToRight(GameObject shipPerformingAction)
    {
        shipPerformingAction.GetComponent<Rigidbody2D>().velocity = shipPerformingAction.GetComponent<EnemyShip>().speed * new Vector2(1f, 0f);
        shipPerformingAction.transform.eulerAngles = new Vector3(0f, 0f, 90f);
    }

    public static void MoveRightToLeft(GameObject shipPerformingAction)
    {
        shipPerformingAction.GetComponent<Rigidbody2D>().velocity = shipPerformingAction.GetComponent<EnemyShip>().speed * new Vector2(-1f, 0f);
        shipPerformingAction.transform.eulerAngles = new Vector3(0f, 0f, -90f);
    }
}
